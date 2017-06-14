using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Livet.EventListeners;
using Calculator.Models;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleViewer.Models;
using Grabacr07.KanColleViewer.ViewModels.Catalogs;
using MetroTrilithon.Mvvm;

namespace Calculator.ViewModels
{
    class CalculatorViewModel : ViewModel
    {
        public static CalculatorViewModel Current { get; } = new CalculatorViewModel();

        private Homeport homeport = KanColleClient.Current.Homeport;

        public Dictionary<string, int> sortieExperienceTable = SortieExperienceTable.SortieExperience;
        public int[] experienceTable = ExperienceTable.experienceByLevel;
        private int[] experiencetable = ExperienceTable.experienceByLevel;
        public IEnumerable<string> MapList { get; private set; }
        public IEnumerable<string> ResultList { get; private set; }
        private string[] battleResults = { "S", "A", "B", "C", "D", "E" };
        private int sortieExperience = 0;


        public ShipCatalogSortWorker SortWorker { get; private set; }

        #region Ships

        private IEnumerable<Ship> _Ships;

        public IEnumerable<Ship> Ships
        {
            get { return this._Ships; }
            set
            {
                if (this._Ships != value)
                {
                    this._Ships = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region CurrentShip

        private Ship _CurrentShip;

        public Ship CurrentShip
        {
            get { return this._CurrentShip; }
            set
            {
                if (this._CurrentShip != value)
                {
                    this._CurrentShip = value;
                    if (value != null)
                    {
                        this._CurrentShip = value;
                        this.TargetLevel = Math.Min(this._CurrentShip.Level + 1, 155);
                        this.RaisePropertyChanged();
                    }
                }
            }
        }

        #endregion

        #region SelectedResult

        private string _SelectedResult;

        public string SelectedResult
        {
            get { return this._SelectedResult; }
            set
            {
                if (this._SelectedResult != value)
                {
                    this._SelectedResult = value;
                    this.UpdateExperience();
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region IsFlagship

        private bool _IsFlagship;

        public bool IsFlagship
        {
            get { return this._IsFlagship; }
            set
            {
                if (this._IsFlagship != value)
                {
                    this._IsFlagship = value;
                    this.UpdateExperience();
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region IsMVP

        private bool _IsMVP;

        public bool IsMVP
        {
            get { return this._IsMVP; }
            set
            {
                if (this._IsMVP != value)
                {
                    this._IsMVP = value;
                    this.UpdateExperience();
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region TargetLevel

        private int _TargetLevel;

        public int TargetLevel
        {
            get { return this._TargetLevel; }
            set
            {
                if (this._TargetLevel != value && value >= 1 && value <= 155)
                {
                    this._TargetLevel = value;
                    this.UpdateExperience();
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region RemainingExperience

        private int _RemianingExperience;

        public int RemainingExperience
        {
            get { return this._RemianingExperience; }
            set
            {
                if (this._RemianingExperience != value)
                {
                    this._RemianingExperience = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region RemainingBattles

        private int _RemainingBattles;

        public int RemainingBattles
        {
            get { return this._RemainingBattles; }
            set
            {
                if (this._RemainingBattles != value)
                {
                    this._RemainingBattles = value;
                    RaisePropertyChanged();
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
                    this.UpdateExperience();
                }
            }
        }

        #endregion

        #region SelectedMap

        private string _SelectedMap;

        public string SelectedMap
        {
            get { return this._SelectedMap; }
            set
            {
                if (this._SelectedMap != value)
                {
                    this._SelectedMap = value;
                    this.UpdateExperience();
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        public CalculatorViewModel()
        {
            this.MapList = this.sortieExperienceTable.Keys.ToList();
            this.ResultList = this.battleResults.ToList();

            this.SortWorker = new ShipCatalogSortWorker();
            this.SortWorker.SetFirst(ShipCatalogSortWorker.LevelColumn);

            KanColleClient.Current.Proxy.api_start2.Subscribe(_ => this.InitializeHomeport());

            SelectedResult = battleResults.FirstOrDefault();
            SelectedMap = MapList.FirstOrDefault();
        }

        private void InitializeHomeport()
        {
            this.homeport = KanColleClient.Current.Homeport;
            KanColleClient.Current.Proxy.api_port.Throttle(TimeSpan.FromMilliseconds(50)).Subscribe(_ => this.Update());
        }

        public void Update()
        {
            this.homeport = KanColleClient.Current.Homeport;
            var list = this.homeport.Organization.Ships.Values;
            this.Ships = this.SortWorker.Sort(list).Reverse();
        }

        private void UpdateExperience()
        {

            if(CurrentShip != null) { 
            //taken from YunYun's calculator plugin
            double multiplier = (this.IsFlagship ? 1.5 : 1) * (this.IsMVP ? 2 : 1) * (this.SelectedResult == "S" ? 1.2 : (this.SelectedResult == "C" ? 0.8 : (this.SelectedResult == "D" ? 0.7 : (this.SelectedResult == "E" ? 0.5 : 1))));

            this.sortieExperience = (int)Math.Round(sortieExperienceTable[SelectedMap] * multiplier);
            this.RemainingExperience = this.experienceTable[TargetLevel] - this.CurrentShip.Exp;
            this.RemainingBattles = (int)Math.Ceiling(this.RemainingExperience / (double)this.sortieExperience);
            }
        }
    }
}
