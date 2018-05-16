using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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

		//Filter Variables
		#region Filter_Name
		private string _Filter_Name;

		public string Filter_Name
		{
			get { return this._Filter_Name; }
			set
			{
				if (this._Filter_Name != value)
				{
					this._Filter_Name = value;
					this.RaisePropertyChanged();
					Filter();
				}
			}
		}
		#endregion
		#region Filter_Secretary
		private string _Filter_Secretary;

		public string Filter_Secretary
		{
			get { return this._Filter_Secretary; }
			set
			{
				if (this._Filter_Secretary != value)
				{
					this._Filter_Secretary = value;
					this.RaisePropertyChanged();
					Filter();
				}
			}
		}
		#endregion
		#region Filter_Fuel
		private int _Filter_Fuel;

		public int Filter_Fuel
		{
			get { return this._Filter_Fuel; }
			set
			{
				if (this._Filter_Fuel != value)
				{
					this._Filter_Fuel = value;
					if(_Filter_Fuel > 7000)
					{
						_Filter_Fuel = 7000;
					}
					this.RaisePropertyChanged();
					Filter();
				}
			}
		}
		#endregion
		#region Filter_Steel
		private int _Filter_Steel;

		public int Filter_Steel
		{
			get { return this._Filter_Steel; }
			set
			{
				if (this._Filter_Steel != value)
				{
					this._Filter_Steel = value;
					if (_Filter_Steel > 7000)
					{
						_Filter_Steel = 7000;
					}
					this.RaisePropertyChanged();
					Filter();
				}
			}
		}
		#endregion
		#region Filter_Ammo
		private int _Filter_Ammo;

		public int Filter_Ammo
		{
			get { return this._Filter_Ammo; }
			set
			{
				if (this._Filter_Ammo != value)
				{
					this._Filter_Ammo = value;
					if (_Filter_Ammo > 7000)
					{
						_Filter_Ammo = 7000;
					}
					this.RaisePropertyChanged();
					Filter();
				}
			}
		}
		#endregion
		#region Filter_Baux
		private int _Filter_Baux;

		public int Filter_Baux
		{
			get { return this._Filter_Baux; }
			set
			{
				if (this._Filter_Baux != value)
				{
					this._Filter_Baux = value;
					if (_Filter_Baux > 7000)
					{
						_Filter_Baux = 7000;
					}
					this.RaisePropertyChanged();
					Filter();
				}
			}
		}
		#endregion

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

			//init numberic filters
			Filter_Fuel = 0;
			Filter_Ammo = 0;
			Filter_Steel = 0;
			Filter_Baux = 0;
		}

		private void Update(object sender, NotifyCollectionChangedEventArgs e)
		{
			Filter();
		}

		private void Filter()
		{
			Output = new ObservableCollection<ConstructionJSON>();
			foreach(ConstructionJSON ship in Log.LogData)
			{
				if (!String.IsNullOrEmpty(Filter_Name) && !ship.name.ToLower().Contains(Filter_Name.ToLower())) continue;
				if (!String.IsNullOrEmpty(Filter_Secretary) && !ship.secretary.ToLower().Contains(Filter_Secretary.ToLower())) continue;
				if (Filter_Fuel !=0 && ship.mats[0] != Filter_Fuel) continue;
				if (Filter_Steel != 0 && ship.mats[1] != Filter_Steel) continue;
				if (Filter_Ammo != 0 && ship.mats[2] != Filter_Ammo) continue;
				if (Filter_Baux != 0 && ship.mats[3] != Filter_Baux) continue;
				Output.Add(ship);
			}
		}

		public void Reset()
		{
			Filter_Name = "";
			Filter_Secretary = "";
			Filter_Fuel = 0;
			Filter_Ammo = 0;
			Filter_Steel = 0;
			Filter_Baux = 0;
			this.Output = new ObservableCollection<ConstructionJSON>(Log.LogData);
		}
	}
}
