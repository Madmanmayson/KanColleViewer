using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Models.Raw;
using Calculator.Models;
using Grabacr07.KanColleWrapper;
using Livet;
using System.IO;
using Livet.EventListeners;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Newtonsoft.Json;
using Grabacr07.KanColleViewer;
using MetroTrilithon.Mvvm;

namespace Calculator.Models
{
    class TrackingData : NotificationObject
    {

        public static TrackingData Current { get; } = new TrackingData();

        private string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smooth and Flat", "KanColleViewer", "Data");

        #region Tracked Ships

        private ObservableCollection<TrackedShip> _TrackedShips;

        public ObservableCollection<TrackedShip> TrackedShips
        {
            get { return this._TrackedShips; }
            set
            {
                if (this._TrackedShips != value)
                {
                    this._TrackedShips = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region IsReloading 変更通知プロパティ

        private bool _IsReloading;

        public bool IsReloading
        {
            get { return this._IsReloading; }
            set
            {
                if (this._IsReloading != value)
                {
                    this._IsReloading = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        public TrackingData()
        {
            this.LoadData();
        }

        public void Update()
        {
            IsReloading = true;
            while (IsReloading)
            {
                foreach (var track in TrackedShips)
                {
                    if (KanColleClient.Current.Homeport.Organization.Ships.Keys.Contains(track.export_data.ship_id))
                    {
                        track.Update();
                        IsReloading = false;
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() => this.TrackedShips.Remove(track)));
                        this.SaveData();
                        IsReloading = true;
                        break;
                    }
                }
            }
        }

        public void SaveData()
        {
            Directory.CreateDirectory(dataPath);

            using (StreamWriter dataWriter = new StreamWriter(Path.Combine(dataPath, "ExpTracking.json")))
            {
                List<SavedShip> exportData = new List<SavedShip>();

                foreach (var ship in TrackedShips)
                {
                    exportData.Add(ship.export_data);
                }

                string output = JsonConvert.SerializeObject(exportData);

                dataWriter.Write(output);
            }
        }

        private void LoadData()
        {
            this.TrackedShips = new ObservableCollection<TrackedShip>();
            var path = Path.Combine(dataPath, "ExpTracking.json");
            if (File.Exists(path))
            {
                string json = File.ReadAllText(Path.Combine(path));
                List<SavedShip> exportedData = JsonConvert.DeserializeObject<List<SavedShip>>(json);
                foreach (SavedShip ship in exportedData)
                {
                    TrackedShips.Add(new TrackedShip(ship));
                }
            }
        }

    }
}
