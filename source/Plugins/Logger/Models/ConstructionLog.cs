using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models.Raw;
using System.Collections.Specialized;

namespace Logger.Models
{
    class ConstructionJSON
    {
        int id;
        string name;
        int[] mats;
        string datetime;
    }

    class ConstructionLog
    {
        public static ConstructionLog Current { get; } = new ConstructionLog();
        KanColleProxy proxy = KanColleClient.Current.Proxy;

        //Variables updated whenever new ship is created
        string shipName;
        int[] materialCost = new int[5]; 

        public ConstructionLog()
        {
            LoadData();
            proxy.api_req_kousyou_createship.TryParse<kcsapi_createship>().Subscribe(x => this.UpdateCost(x.Request));
        }

        private void UpdateCost(NameValueCollection req)
        {

        }

        //Loads previously saved JSON data
        private void LoadData()
        {

        }

        //Saves JSON data (perform after each construction)
        private void SaveData()
        {

        }

        //call when the api endpoint is called
        private void UpdateName()
        {

        }
    }
}
