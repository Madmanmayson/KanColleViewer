using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Logger.Models;
using MetroTrilithon.Mvvm;

namespace Logger.ViewModels
{
    class ConstructionWindowViewModel : WindowViewModel
    {
		public ILogger Log;

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

		public ConstructionWindowViewModel(string title, ILogger data)
		{
			this.Title = title;
			this.Log = data;
			Log.LogData.CollectionChanged += Update;
			this.Output = new ObservableCollection<ConstructionJSON>(Log.LogData);
		}

		private void Update(object sender, NotifyCollectionChangedEventArgs e)
		{

		}
	}
}
