using System;
using System.Linq;
using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleViewer.Properties;

namespace Grabacr07.KanColleViewer.ViewModels.Contents
{
	public static class ShipSpeedExtensions
	{
		public static string ToDisplayString(this ShipSpeed? speed) => ToDisplayString(speed ?? ShipSpeed.Immovable);

		public static string ToDisplayString(this ShipSpeed speed)
		{
			switch (speed)
			{
				case ShipSpeed.Fastest:
					return Resources.Fleets_Speed_Fastest;
				case ShipSpeed.Faster:
					return Resources.Fleets_Speed_Faster;
				case ShipSpeed.Fast:
					return Resources.Fleets_Speed_Fast;
				case ShipSpeed.Slow:
					return Resources.Fleets_Speed_Slow;
				default:
					return "";
			}
		}
	}
}
