using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using System.Collections.ObjectModel;
using Calculator.Models;
using Grabacr07.KanColleViewer.ViewModels;
using MetroTrilithon.Mvvm;

namespace Calculator.ViewModels
{
    class TrackedShipWindowViewModel : WindowViewModel
    {
        public TrackingData Tracking { get; } = TrackingData.Current;


        public TrackedShipWindowViewModel()
        {
            this.Title = "Tracked Ships";
        }
    }
}
