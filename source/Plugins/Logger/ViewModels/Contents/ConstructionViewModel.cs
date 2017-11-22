using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleViewer.ViewModels;
using Logger.Models;
using Livet.Messaging;
using Livet.EventListeners;
using System.Collections.ObjectModel;

namespace Logger.ViewModels.Contents
{
    class ConstructionViewModel : TabItemViewModel
    {
        public ConstructionLogger Log = ConstructionLogger.Current;

        private Queue<ConstructionJSON> latest = new Queue<ConstructionJSON>();
        public ObservableCollection<ConstructionJSON> output = new ObservableCollection<ConstructionJSON>();

        public override string Name
        {
            get { return "Construction"; }
            protected set { throw new NotImplementedException(); }
        }

        public ConstructionViewModel()
        {
            this.CompositeDisposable.Add(new PropertyChangedEventListener(Log.constructedShips)
            {
                { () => this.Log.constructedShips, (sender, args) => this.Update() },
            });

            LoadLatest();
        }

        private void LoadLatest()
        {
            var loadedData = Log.constructedShips.ToArray();
            if (loadedData.Length > 5)
            {
                for (int i = 1; i >= 5; i++)
                {
                    latest.Enqueue(loadedData[loadedData.Length - i]);
                }
            }
            else
            {
                for(int i = loadedData.Length; i > 0; i--)
                {
                    latest.Enqueue(loadedData[i - 1]);
                }
            }
            this.output = new ObservableCollection<ConstructionJSON>(this.latest);
        }

        private void Update()
        {
            latest.Dequeue();
            latest.Enqueue(Log.constructedShips.Last());
            this.output = new ObservableCollection<ConstructionJSON>(this.latest);
        }

        public void ExportCSV()
        {

        }

        public void OpenNewWindow()
        {
            var message = new TransitionMessage("Show/TrackedWindow")
            {
                TransitionViewModel = new ConstructionWindowViewModel()
            };
            this.Messenger.RaiseAsync(message);
        }
    }
}
