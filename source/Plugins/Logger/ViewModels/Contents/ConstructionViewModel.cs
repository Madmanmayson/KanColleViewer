using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleViewer.ViewModels;
using Logger.Models;

namespace Logger.ViewModels.Contents
{
    class ConstructionViewModel : TabItemViewModel
    {
        public static ConstructionLogger Log = ConstructionLogger.Current;

        public override string Name
        {
            get { return "Construction"; }
            protected set { throw new NotImplementedException(); }
        }

        public ConstructionViewModel()
        {

        }
    }
}
