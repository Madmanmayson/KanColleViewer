using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Models
{
	public class ConstructionJSON : NotificationObject, IComparable<ConstructionJSON>
	{

		#region id

		private int _ID;

		public int id
		{
			get { return this._ID; }
			set
			{
				if (this._ID != value)
				{
					this._ID = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		#region name

		private string _Name;

		public string name
		{
			get { return this._Name; }
			set
			{
				if (this._Name != value)
				{
					this._Name = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		#region mats

		private int[] _Mats;

		public int[] mats
		{
			get { return this._Mats; }
			set
			{
				if (this._Mats != value)
				{
					this._Mats = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		#region datetime

		private string _Datetime;

		public string datetime
		{
			get { return this._Datetime; }
			set
			{
				if (this._Datetime != value)
				{
					this._Datetime = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		#region secretary

		private string _Secretary;

		public string secretary
		{
			get { return this._Secretary; }
			set
			{
				if (this._Secretary != value)
				{
					this._Secretary = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public ConstructionJSON(int id, string name, int[] mats, string datetime, string secretary)
		{
			this.id = id;
			this.name = name;
			this.mats = new int[5];
			Array.Copy(mats, this.mats, mats.Length);
			this.datetime = datetime;
			this.secretary = secretary;
		}

		public int CompareTo(ConstructionJSON other)
		{
			return other.id - this.id;
		}
	}
}
