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
        int id;
        string name;
        int[] mats;
        string datetime;

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

    class ConstructionLog
    {
        public static ConstructionLog Current { get; } = new ConstructionLog();
        KanColleProxy proxy = KanColleClient.Current.Proxy;

        //global list of constructed ships
        private string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smooth and Flat", "KanColleViewer", "Data");
        ObservableCollection<ConstructionJSON> constructedShips;

        //Variables updated whenever new ship is created
        string shipName;
        int[] materialCost = new int[5];
        int waitingForShipID = -1;

        public ConstructionLog()
        {
            this.constructedShips = new ObservableCollection<ConstructionJSON>();
            LoadData();
            proxy.api_req_kousyou_createship.TryParse<kcsapi_createship>().Subscribe(x => this.UpdateCost(x.Request));
            proxy.api_get_member_kdock.TryParse<kcsapi_kdock[]>().Subscribe(x => this.FindShip(x.Data));
        }

        //Gets material cost from request
        private void UpdateCost(NameValueCollection req)
        {
            this.waitingForShipID = Int32.Parse(req["api_kdock_id"]);
            this.materialCost[0] = Int32.Parse(req["api_item1"]); //fuel
            this.materialCost[1] = Int32.Parse(req["api_item2"]); //ammo
            this.materialCost[2] = Int32.Parse(req["api_item3"]); //steel
            this.materialCost[3] = Int32.Parse(req["api_item4"]); //baux
            this.materialCost[4] = Int32.Parse(req["api_item5"]); //construction mats

        }

        //Gets ship name from api response
        private void FindShip(kcsapi_kdock[] docks)
        {
            if(this.waitingForShipID != -1)
            {
                this.shipName = KanColleClient.Current.Master.Ships[docks[this.waitingForShipID].api_created_ship_id].Name;
                this.constructedShips.Add(new ConstructionJSON(constructedShips.Count + 1, 
                                                                this.shipName, 
                                                                this.materialCost, 
                                                                DateTime.Now.ToString()));
                this.waitingForShipID = -1;
            }
        }

        //Loads previously saved JSON data
        private void LoadData()
        {
            Directory.CreateDirectory(dataPath);

            using(StreamWriter dataWriter = new StreamWriter(Path.Combine(dataPath, "ConstructionLog.json")))
            {
                List<ConstructionJSON> exportData = constructedShips.ToList();

                string output = JsonConvert.SerializeObject(exportData);

                dataWriter.Write(output);
            }
        }

        //Saves JSON data (perform after each construction)
        private void SaveData()
        {
            var path = Path.Combine(dataPath, "ConstructionLog.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(Path.Combine(path));
                List<ConstructionJSON> exportedData = JsonConvert.DeserializeObject<List<ConstructionJSON>>(json);
                foreach(ConstructionJSON log in exportedData)
                {
                    constructedShips.Add(log);
                }
            }
        }
    }
}
