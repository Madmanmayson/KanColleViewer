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

namespace Logger.Models
{
    class ConstructionJSON : IComparable<ConstructionJSON>
    {
        public int id;
        public string name;
        public int[] mats;
        public string datetime;

        public ConstructionJSON(int id, string name, int[] mats, string datetime)
        {
            this.id = id;
            this.name = name;
            this.mats = mats;
            this.datetime = datetime;
        }

        public int CompareTo(ConstructionJSON other)
        {
            return other.id - this.id;
        }
    }

    class ConstructionLogger
    {
        public static ConstructionLogger Current { get; } = new ConstructionLogger();
        KanColleProxy proxy = KanColleClient.Current.Proxy;

        //global list of constructed ships
        private string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smooth and Flat", "KanColleViewer", "Data");
        public ObservableCollection<ConstructionJSON> constructedShips;

        //Variables updated whenever new ship is created
        string shipName;
        int[] materialCost = new int[5];
        int DockID = -1;
        bool waitingForShip;

        public ConstructionLogger()
        {
            this.constructedShips = new ObservableCollection<ConstructionJSON>();
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

        }

        //Gets ship name from api response
        private void FindShip(kcsapi_kdock[] docks)
        {
            foreach (var dock in docks.Where(dock => this.waitingForShip && dock.api_id == this.DockID))
            {
                this.shipName = KanColleClient.Current.Master.Ships[dock.api_created_ship_id].Name;
                this.constructedShips.Add(new ConstructionJSON(constructedShips.Count + 1,
                                                                this.shipName,
                                                                this.materialCost,
                                                                DateTime.Now.ToString()));
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
                    constructedShips.Add(log);
                }
            }
        }

        //Saves JSON data (perform after each construction)
        private void SaveData()
        {
            Directory.CreateDirectory(dataPath);

            using (StreamWriter dataWriter = new StreamWriter(Path.Combine(dataPath, "ConstructionLog.json")))
            {
                List<ConstructionJSON> exportData = constructedShips.ToList();
                string output = JsonConvert.SerializeObject(exportData);
                dataWriter.Write(output);
            }
        }
    }
}
