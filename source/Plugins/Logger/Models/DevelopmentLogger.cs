using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models.Raw;
using Grabacr07.KanColleWrapper;
using Livet;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;

namespace Logger.Models
{
    public class DevelopmentLogger : NotificationObject, ILogger
    {

		KanColleProxy proxy = KanColleClient.Current.Proxy;

		//Public interaction variables
		public static DevelopmentLogger Current { get; } = new DevelopmentLogger();
		public ObservableCollection<ConstructionJSON> LogData { get; set; }

		private string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smooth and Flat", "KanColleViewer", "Data");

		private string itemName;
		private int[] materialCost = new int[5];
		private string secretary;

		public DevelopmentLogger()
		{
			this.LogData = new ObservableCollection<ConstructionJSON>();
			LoadData();
			proxy.api_req_kousyou_createitem.TryParse<kcsapi_createitem>().Subscribe(x => this.CreateItem(x.Data, x.Request));
		}

		private void CreateItem(kcsapi_createitem data, NameValueCollection req)
		{
			this.itemName = data.api_create_flag == 1 ? KanColleClient.Current.Master.SlotItems[data.api_slot_item.api_slotitem_id].Name : "FAILED";
			this.materialCost[0] = Int32.Parse(req["api_item1"]); //fuel
			this.materialCost[1] = Int32.Parse(req["api_item2"]); //ammo
			this.materialCost[2] = Int32.Parse(req["api_item3"]); //steel
			this.materialCost[3] = Int32.Parse(req["api_item4"]); //baux
			this.materialCost[4] = data.api_create_flag == 1 ? 1 : 0;
			this.secretary = KanColleClient.Current.Homeport.Organization.Fleets[1].Ships[0].Info.Name;

			this.LogData.Add(new ConstructionJSON(LogData.Count + 1,
																this.itemName,
																this.materialCost,
																DateTime.Now.ToString(),
																this.secretary));
			SaveData();
		}

		private void LoadData()
		{
			var path = Path.Combine(dataPath, "Development.json");
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

			using (StreamWriter dataWriter = new StreamWriter(Path.Combine(dataPath, "Development.json")))
			{
				List<ConstructionJSON> exportData = LogData.ToList();
				string output = JsonConvert.SerializeObject(exportData);
				dataWriter.Write(output);
			}
		}
	}
}
