using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleViewer.Models.Settings;
using Livet;

namespace Grabacr07.KanColleViewer.ViewModels
{
	public class StartContentViewModel : ViewModel
	{
		public NavigatorViewModel Navigator { get; }


		public bool ClearCacheOnNextStartup
		{
			get => GeneralSettings.ClearCacheOnNextStartup.Value;
			set => GeneralSettings.ClearCacheOnNextStartup.Value = value;
		}

		#region CanSetRegionCookie 変更通知プロパティ 

		private bool _CanSetRegionCookie = true;

		public bool CanSetRegionCookie
		{
			get { return this._CanSetRegionCookie; }
			set
			{
				if (this._CanSetRegionCookie != value)
				{
					this._CanSetRegionCookie = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region SetRegionCookieButtonContent 変更通知プロパティ 
		private string _SetRegionCookieButtonContent = Properties.Resources.StartContent_SetRegionCookieButton;

		public string SetRegionCookieButtonContent
		{
			get { return this._SetRegionCookieButtonContent; }
			set
			{
				if (this._SetRegionCookieButtonContent != value)
				{
					this._SetRegionCookieButtonContent = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		public void SetRegionCookie()
		{
			if (!(WindowService.Current.MainWindow is KanColleWindowViewModel)) return;
			var navigator = ((KanColleWindowViewModel)WindowService.Current.MainWindow).Navigator;

			navigator.CookieNavigate();
			this.SetRegionCookieButtonContent = Properties.Resources.StartContent_SetRegionCookieMessage;
			this.CanSetRegionCookie = false;
		}

		public StartContentViewModel(NavigatorViewModel navigator)
		{
			this.Navigator = navigator;
		}
	}
}
