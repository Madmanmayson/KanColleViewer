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
using Livet.Messaging;
using System.Diagnostics;
using MetroTrilithon.Mvvm;

namespace Calculator.ViewModels
{
    class ToolViewModel : ViewModel
    {
        public static ToolViewModel Current { get; } = new ToolViewModel();
        
        private readonly Subject<Unit> updateSource = new Subject<Unit>();


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

        public ToolViewModel()
        {
            this.Calculator = new CalculatorViewModel();

            this.updateSource
                .Throttle(TimeSpan.FromMilliseconds(1500))
                .Do(_ => this.UpdateCore());

            this.CompositeDisposable.Add(this.updateSource);

            KanColleClient.Current
                .Subscribe(nameof(KanColleClient.IsStarted), InitializePlugin, false);
        }

        public void InitializePlugin()
        {
            KanColleClient.Current.Homeport.Organization.Subscribe(nameof(Organization.Ships), Update, false);
        }

        private void Update()
        {
            this.updateSource.OnNext(Unit.Default);
        }

        private void UpdateCore()
        {
            this.Calculator.Update();
            TrackingData.Current.Update();  
        }

        public void OpenNewWindow()
        {
            var message = new TransitionMessage("Show/TrackedWindow")
            {
                TransitionViewModel = new TrackedShipWindowViewModel()
            };
            this.Messenger.RaiseAsync(message);
        }
    }
}
