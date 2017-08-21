using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using System.Collections.ObjectModel;
using Calculator.Models.Raw;
using MetroTrilithon.Mvvm;

namespace Calculator.ViewModels
{
    class TrackedShipWindowViewModel : WindowViewModel
    {

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

        public TrackedShipWindowViewModel(ObservableCollection<TrackedShip> TrackedShips)
        {
            this.TrackedShips = TrackedShips;
        }

    }
}
