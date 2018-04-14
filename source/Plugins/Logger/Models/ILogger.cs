using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Models
{
	interface ILogger
	{
		ObservableCollection<ConstructionJSON> LogData { get; set; }
	}
}
