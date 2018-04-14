using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Livet;
using System.ComponentModel;

namespace Logger.Models
{
    public class ConstructionLogger : NotificationObject, ILogger
	{
		//Public variables for interface
        public static ConstructionLogger Current { get; } = new ConstructionLogger();
		public ObservableCollection<ConstructionJSON> LogData { get; set; }

		KanColleProxy proxy = KanColleClient.Current.Proxy;

        //Data save location
        private string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smooth and Flat", "KanColleViewer", "Data");
		
        //Variables updated whenever new ship is created
        private string shipName;
        private int[] materialCost = new int[5];
        private int DockID = -1;
        private bool waitingForShip;
		private string secretary; 

        public ConstructionLogger()
        {
            this.LogData = new ObservableCollection<ConstructionJSON>();
            LoadData();
            proxy.api_req_kousyou_createship.TryParse<kcsapi_createship>().Subscribe(x => this.UpdateCost(x.Request));
            proxy.api_get_member_kdock.TryParse<kcsapi_kdock[]>().Subscribe(x => this.FindShip(x.Data));
        }

        //Gets material cost from request
        private void UpdateCost(NameValueCollection req)
        {
            this.waitingForShip = true;
            this.DockID = Int32.Parse(req["api_kdock_id"]);
            this.materialCost[0] = Int32.Parse(req["api_item1"]); //fuel
            this.materialCost[1] = Int32.Parse(req["api_item2"]); //ammo
            this.materialCost[2] = Int32.Parse(req["api_item3"]); //steel
            this.materialCost[3] = Int32.Parse(req["api_item4"]); //baux
            this.materialCost[4] = Int32.Parse(req["api_item5"]); //construction mats
			this.secretary = KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info.Name;

        }

        //Gets ship name from api response
        private void FindShip(kcsapi_kdock[] docks)
        {
            foreach (var dock in docks.Where(dock => this.waitingForShip && dock.api_id == this.DockID))
            {
                this.shipName = KanColleClient.Current.Master.Ships[dock.api_created_ship_id].Name;
                this.LogData.Add(new ConstructionJSON(LogData.Count + 1,
                                                                this.shipName,
                                                                this.materialCost,
                                                                DateTime.Now.ToString(),
																this.secretary));
                this.DockID = -1;
                this.SaveData();
            }
                if (this.DockID != -1)
            {
                
            }
        }

        //Loads previously saved JSON data
        private void LoadData()
        {
            var path = Path.Combine(dataPath, "ConstructionLog.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(Path.Combine(path));
                List<ConstructionJSON> exportedData = JsonConvert.DeserializeObject<List<ConstructionJSON>>(json);
                foreach (ConstructionJSON log in exportedData)
                {
                    LogData.Add(log);
                }
            }
        }

        //Saves JSON data (perform after each construction)
        private void SaveData()
        {
            Directory.CreateDirectory(dataPath);

            using (StreamWriter dataWriter = new StreamWriter(Path.Combine(dataPath, "ConstructionLog.json")))
            {
                List<ConstructionJSON> exportData = LogData.ToList();
                string output = JsonConvert.SerializeObject(exportData);
                dataWriter.Write(output);
            }
        }
    }
}
