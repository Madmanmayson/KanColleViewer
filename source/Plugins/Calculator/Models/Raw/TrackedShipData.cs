using Calculator.ViewModels;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using System;

namespace Calculator.Models.Raw
{
    class ImportData
    {
        public SavedShip[] imported { get; set; }
    }

    class SavedShip : NotificationObject
    {
        public int ship_id;
        #region target_level

        private int _TargetLevel;

        public int target_level
        {
            get { return this._TargetLevel; }
            set
            {
                if (this._TargetLevel != value)
                {
                    this._TargetLevel = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region selected_map

        private string _selected_map;

        public string selected_map
        {
            get { return this._selected_map; }
            set
            {
                if (this._selected_map != value)
                {
                    this._selected_map = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region selected_rank

        private string _selected_rank;

        public string selected_rank
        {
            get { return this._selected_rank; }
            set
            {
                if (this._selected_rank != value)
                {
                    this._selected_rank = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region is_flagship

        private bool _is_flagship;

        public bool is_flagship
        {
            get { return this._is_flagship; }
            set
            {
                if (this._is_flagship != value)
                {
                    this._is_flagship = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion
        #region is_mvp

        private bool _is_mvp;

        public bool is_mvp
        {
            get { return this._is_mvp; }
            set
            {
                if (this._is_mvp != value)
                {
                    this._is_mvp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion
    }

    class TrackedShip : NotificationObject
    {
        #region export_data

        private SavedShip _export_data;

        public SavedShip export_data
        {
            get { return this._export_data; }
            set
            {
                if (this._export_data != value)
                {
                    this._export_data = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region remaining_Exp

        private int _remaining_Exp;

        public int remaining_Exp
        {
            get { return this._remaining_Exp; }
            set
            {
                if (this._remaining_Exp != value)
                {
                    this._remaining_Exp = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region remaining_Battles

        private int _remaining_Battles;

        public int remaining_Battles
        {
            get { return this._remaining_Battles; }
            set
            {
                if (this._remaining_Battles != value)
                {
                    this._remaining_Battles = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        private ToolViewModel plugin;

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
                        this.RaisePropertyChanged();
                    }
                }
            }
        }

        #endregion


        //Constructor for adding a new ship
        public TrackedShip(int ship_id, int target_level, string selected_map, string selected_rank, bool is_flagship, bool is_mvp, ToolViewModel plugin)
        {
            this.export_data = new SavedShip();
            this.export_data.ship_id = ship_id;
            this.export_data.target_level = target_level;
            this.export_data.selected_map = selected_map;
            this.export_data.selected_rank = selected_rank;
            this.export_data.is_flagship = is_flagship;
            this.export_data.is_mvp = is_mvp;
            this.plugin = plugin;
            this.CurrentShip = this.plugin.homeport.Organization.Ships[ship_id];
            this.Update();
        }

        //Loading constructor
        public TrackedShip(SavedShip loadedData, ToolViewModel plugin)
        {
            this.export_data = loadedData;
            this.plugin = plugin;
        }

        public void Delete()
        {
            this.plugin.TrackedShips.Remove(this);
            this.plugin.SaveData();
        }

        public void Update()
        {
            if(this.plugin.homeport.Organization.Ships != null)
            {
                if (CurrentShip != null && CurrentShip.Level >= export_data.target_level)
                {
                    this.Delete();
                }
                else
                {
                    this.CurrentShip = this.plugin.homeport.Organization.Ships[export_data.ship_id];

                    double multiplier = (this.export_data.is_flagship ? 1.5 : 1) * (this.export_data.is_mvp ? 2 : 1) * (this.export_data.selected_rank == "S" ? 1.2 : (this.export_data.selected_rank == "C" ? 0.8 : (this.export_data.selected_rank == "D" ? 0.7 : (this.export_data.selected_rank == "E" ? 0.5 : 1))));

                    var sortieExperience = (int)Math.Round(plugin.sortieExperienceTable[this.export_data.selected_map] * multiplier);
                    this.remaining_Exp = this.plugin.experienceTable[this.export_data.target_level] - this.CurrentShip.Exp;
                    this.remaining_Battles = (int)Math.Ceiling(this.remaining_Exp / (double)sortieExperience);
                }
            }
            
        }
    }
}
