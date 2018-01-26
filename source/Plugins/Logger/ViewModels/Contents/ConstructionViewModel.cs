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
using System.Collections.Specialized;

namespace Logger.ViewModels.Contents
{
    class ConstructionViewModel : TabItemViewModel
    {
        public ConstructionLogger Log = ConstructionLogger.Current;

        private Queue<ConstructionJSON> latest = new Queue<ConstructionJSON>();

        #region Output
        private ObservableCollection<ConstructionJSON> _Output;

        public ObservableCollection<ConstructionJSON> Output
        {
            get { return this._Output; }
            set
            {
                if (this._Output != value)
                {
                    this._Output = value;
                    this.RaisePropertyChanged();
                }
            }
        }
        #endregion

        public override string Name
        {
            get { return "Construction"; }
            protected set { throw new NotImplementedException(); }
        }

        public ConstructionViewModel()
        {
            Log.constructedShips.CollectionChanged += Update;
            LoadLatest();
        }

        private void LoadLatest()
        {
            var loadedData = Log.constructedShips.ToArray();
            if (loadedData.Length > 5)
            {
                for (int i = 5; i >=1; i--)
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
            this.Output = new ObservableCollection<ConstructionJSON>(this.latest.Reverse());
        }

        private void Update(object sender, NotifyCollectionChangedEventArgs e)
        {
            latest.Dequeue();
            latest.Enqueue(Log.constructedShips.Last());
            this.Output = new ObservableCollection<ConstructionJSON>(this.latest.Reverse());
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
