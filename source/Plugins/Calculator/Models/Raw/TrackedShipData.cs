﻿using Calculator.ViewModels;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Calculator.Models.Raw;
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
        //Designed to be Editable in a future release so they implement RaisePropertyChanged()

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
        //Contained the SavedShip Object
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

        //Display Info
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

        //For implementing editing in a future release
        #region isComplete

        private bool _isComplete;

        public bool isComplete
        {
            get { return _isComplete;  }
            set
            {
                if(this._isComplete != value)
                {
                    this._isComplete = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        //Holds Vital Info about the ship from the API
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
        public TrackedShip(int ship_id, int target_level, string selected_map, string selected_rank, bool is_flagship, bool is_mvp)
        {
            this.export_data = new SavedShip();
            this.export_data.ship_id = ship_id;
            this.export_data.target_level = target_level;
            this.export_data.selected_map = selected_map;
            this.export_data.selected_rank = selected_rank;
            this.export_data.is_flagship = is_flagship;
            this.export_data.is_mvp = is_mvp;
            this.isComplete = false;
            this.CurrentShip = KanColleClient.Current.Homeport.Organization.Ships[ship_id];
            this.Update();
        }

        //Constructor for loading a ship
        public TrackedShip(SavedShip loadedData)
        {
            this.export_data = loadedData;
            this.isComplete = false;
        }

        public void Delete()
        {
            TrackingData.Current.TrackedShips.Remove(this);
            TrackingData.Current.SaveData();
        }

        public void Update()
        {
            if(KanColleClient.Current.Homeport.Organization.Ships != null && !isComplete)
            {
                if (CurrentShip != null && CurrentShip.Level >= export_data.target_level)
                {
                    this.isComplete = true;
                    this.remaining_Battles = 0;
                    this.remaining_Exp = 0;
                }
                else
                {
                    this.CurrentShip = KanColleClient.Current.Homeport.Organization.Ships[export_data.ship_id];

                    double multiplier = (this.export_data.is_flagship ? 1.5 : 1) * (this.export_data.is_mvp ? 2 : 1) * (this.export_data.selected_rank == "S" ? 1.2 : (this.export_data.selected_rank == "C" ? 0.8 : (this.export_data.selected_rank == "D" ? 0.7 : (this.export_data.selected_rank == "E" ? 0.5 : 1))));

                    var sortieExperience = (int)Math.Round(SortieExperienceTable.SortieExperience[this.export_data.selected_map] * multiplier);
                    this.remaining_Exp = ExperienceTable.experienceByLevel[this.export_data.target_level] - this.CurrentShip.Exp;
                    this.remaining_Battles = (int)Math.Ceiling(this.remaining_Exp / (double)sortieExperience);
                }
            }
        }
    }
}
