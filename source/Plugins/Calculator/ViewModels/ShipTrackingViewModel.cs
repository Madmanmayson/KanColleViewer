using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Calculator.Models;

namespace Calculator.ViewModels
{
    class ShipTrackingViewModel : ViewModel
    {
        public TrackingData Tracking { get; } = TrackingData.Current;
    }
}
