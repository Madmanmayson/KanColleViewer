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
		public int DISPLAY_QUANTITY = 15;

		public ILogger Log;

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
			get;
			protected set; 
        }

		public string Header { get; set; }

        public ConstructionViewModel(string title, ILogger logger)
        {
			this.Log = logger;
			this.Name = title;
			this.Header = "Latest " + title + "s";
            logger.LogData.CollectionChanged += Update;
            LoadLatest();
        }

        private void LoadLatest()
        {
            var loadedData = Log.LogData.ToArray();
            if (loadedData.Length > DISPLAY_QUANTITY)
            {
                for (int i = DISPLAY_QUANTITY; i >=1; i--)
                {
                    latest.Enqueue(loadedData[loadedData.Length - i]);
                }
            }
            else
            {
                for(int i = loadedData.Length; i > 0; i--)
                {
                    latest.Enqueue(loadedData[loadedData.Length - i]);
                }
            }
            this.Output = new ObservableCollection<ConstructionJSON>(this.latest.Reverse());
        }

        private void Update(object sender, NotifyCollectionChangedEventArgs e)
        {
			if(latest.Count >= DISPLAY_QUANTITY) latest.Dequeue();
            latest.Enqueue(Log.LogData.Last());
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
