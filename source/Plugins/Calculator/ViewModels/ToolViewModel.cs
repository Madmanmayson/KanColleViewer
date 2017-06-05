using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Calculator;
using Calculator.Models;
using Livet.EventListeners;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleViewer.ViewModels.Catalogs;
using System.Reactive.Subjects;
using System.Reactive;
using System.Reactive.Linq;

namespace Calculator.ViewModels
{
    class ToolViewModel : ViewModel
    {
        private readonly ShipCatalogWindowViewModel ShipVM = new ShipCatalogWindowViewModel();

        private readonly Subject<Unit> updateSource = new Subject<Unit>();
        private readonly Homeport homeport = KanColleClient.Current.Homeport;

        public ExperienceCalculator calculator;

        public IEnumerable<string> MapList { get; private set; }
        public IEnumerable<string> ResultList { get; private set; }

        public ShipCatalogSortWorker SortWorker { get; private set; }

        #region Ships

        private IReadOnlyCollection<ShipViewModel> _Ships;

        public IReadOnlyCollection<ShipViewModel> Ships
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
                        this.calculator.CurrentShip = this._CurrentShip;
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
                    this.calculator.battleResult = _SelectedResult;
                    this.RaisePropertyChanged();
                    this.calculator.UpdateExperience();
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
                    this.RaisePropertyChanged();
                    this.calculator.isFlagship = _IsFlagship;
                    this.calculator.UpdateExperience();
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
                    this.RaisePropertyChanged();
                    this.calculator.isMVP = _IsMVP;
                    this.calculator.UpdateExperience();
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
                if (this._TargetLevel != value && value >= 1 && value <= 150)
                {
                    this._TargetLevel = value;
                    this.RaisePropertyChanged();
                    this.calculator.UpdateExperience();
                }
            }
        }

        #endregion

        #region RemainingExperience

        private int _RemianingExperience;

        public int RemainingExpereince
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
                    this.calculator.UpdateExperience();
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
                    this.calculator.selectedMap = _SelectedMap;
                    this.calculator.UpdateExperience();
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        public ToolViewModel(Calculator plugin)
        {
            calculator = new ExperienceCalculator();
            this.MapList = this.calculator.sortieExperienceTable.Keys.ToList();
            this.ResultList = this.calculator.battleResults.ToList();

            //this.SortWorker = new ShipCatalogSortWorker();
            //this.SortWorker.SetFirst(ShipCatalogSortWorker.LevelColumn);

            this.updateSource
                .Do(_ => this.IsReloading = true)
                .Throttle(TimeSpan.FromMilliseconds(7.0))
                .Do(_ => this.Update())
                .Subscribe(_ => this.IsReloading = false);

            if (homeport != null)
                this.CompositeDisposable.Add(new PropertyChangedEventListener(homeport)
            {
                { () => homeport.Organization.Ships, (sender, args) => this.Update() },
            });

            this.CompositeDisposable.Add(new PropertyChangedEventListener(this.calculator)
            {
                {
                    () => this.calculator.remainingBattles,
                    (_, __) => this.RaisePropertyChanged(() => this.RemainingExpereince)
                },
                {
                    () => this.calculator.remainingBattles,
                    (_, __) => this.RaisePropertyChanged(() => this.RemainingBattles)
                },
            });
        }

        public void Update()
        {
            this.RaisePropertyChanged("AllShipTypes");
            this.Ships = ShipVM.Ships;
        }
    }
}
