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

namespace Calculator.ViewModels
{
    class ToolViewModel : ViewModel
    {
        private static string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Smooth and Flat", "KanColleViewer", "Data");
        public Homeport homeport = KanColleClient.Current?.Homeport;
        private readonly Subject<Unit> updateSource = new Subject<Unit>();

        public Dictionary<string, int> sortieExperienceTable = SortieExperienceTable.SortieExperience;
        public int[] experienceTable = ExperienceTable.experienceByLevel;

        #region Calculator

        private CalculatorViewModel _Calculator;

        public CalculatorViewModel Calculator
        {
            get { return this._Calculator; }
            set
            {
                if(this._Calculator != value)
                {
                    this._Calculator = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Tracked Ships

        private ObservableCollection<TrackedShip> _TrackedShips;

        public ObservableCollection<TrackedShip> TrackedShips
        {
            get { return this._TrackedShips; }
            set
            {
                if(this._TrackedShips != value)
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

        public ToolViewModel()
        {
            this.LoadData();
            this.Calculator = new CalculatorViewModel(this);

            this.updateSource
                .Do(_ => this.IsReloading = true)
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Do(_ => this.UpdateCore())
                .Subscribe(_ => this.IsReloading = false);
            this.CompositeDisposable.Add(this.updateSource);

            Thread thread = new Thread(new ThreadStart(InitializePlugin));

            KanColleClient.Current.Proxy.api_start2.
                //Throttle(TimeSpan.FromSeconds(5)).
                Subscribe(_ => thread.Start());

            
        }

        private void InitializePlugin()
        {
            while(this.homeport == null)
            {
                Thread.Sleep(1000);
                this.homeport = KanColleClient.Current.Homeport;
            }
            this.CompositeDisposable.Add(new PropertyChangedEventListener(this.homeport.Organization)
            {
                { () => this.homeport.Organization.Ships, (sender, args) => this.Update() },
            });
        }

        private void Update()
        {
            this.updateSource.OnNext(Unit.Default);
        }

        private void UpdateCore()
        {
            this.Calculator.Update();
            foreach(var track in TrackedShips)
            {
                if (homeport.Organization.Ships.Keys.Contains(track.export_data.ship_id))
                {
                    track.Update();
                }
                else
                {
                    track.Delete();
                }
                
            };
        }

        public void SaveData()
        {
            Directory.CreateDirectory(dataPath);

            using (StreamWriter dataWriter = new StreamWriter(Path.Combine(dataPath, "ExpTracking.json")))
            {
                List<SavedShip> exportData = new List<SavedShip>();

                foreach(var ship in TrackedShips)
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
                    TrackedShips.Add(new TrackedShip(ship, this));
                }
            }
        }
    }
}
