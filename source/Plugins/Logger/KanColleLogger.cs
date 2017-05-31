﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Logger.Models;
using Logger.ViewModels;
using Logger.Views;

namespace Logger
{
	[Export(typeof(IPlugin))]
	[Export(typeof(ISettings))]
    [Export(typeof(ITool))]
	[ExportMetadata("Guid", "B33A80D8-F529-430C-AB56-1B6C486AA4D9")]
	[ExportMetadata("Title", "KanColleLogger")]
	[ExportMetadata("Description", "File logging back-end")]
	[ExportMetadata("Version", "2.0")]
	[ExportMetadata("Author", "@Xiatian")]
	public class KanColleLogger : IPlugin, ISettings, ITool
	{
        private readonly ToolViewModel vm;

        public KanColleLogger()
        {
            this.vm = new ToolViewModel();
        }

        public string Name => "Logger";

        public object View => new ToolView { DataContext = this.vm, };

        public static bool IsWindows8OrGreater { get; set; }

		public static LoggerSettings Settings { get; private set; }

		public static ObservableCollection<LoggerBase> Loggers { get; set; }

		public Size SettingsSize => new Size(450, 800);

		private LoggerViewModel viewmodel;

		object ISettings.View => new LoggerView { DataContext = this.viewmodel, };

		public void Initialize()
		{
			var version = Environment.OSVersion.Version;

			IsWindows8OrGreater = (version.Major == 6 && version.Minor >= 2) || version.Major > 6;

			Loggers = new ObservableCollection<LoggerBase>
			{
				new ItemLog(KanColleClient.Current.Proxy),
				new ConstructionLog(KanColleClient.Current.Proxy),
				new BattleLog(KanColleClient.Current.Proxy),
				new MaterialsLog(KanColleClient.Current.Proxy),
			};

			Settings = new LoggerSettings(Loggers);

			foreach (var logger in Loggers)
				logger.Initialize();

			this.viewmodel = new LoggerViewModel();
		}
	}
}
