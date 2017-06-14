using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Grabacr07.KanColleViewer.Composition;
using Calculator.Views;
using Calculator.ViewModels;

namespace Calculator
{
    [Export(typeof(IPlugin))]
    [Export(typeof(ITool))]
    [ExportMetadata("Guid", "4eea0aeb-7aa9-4a39-8ac9-ea79f62116ae")]
    [ExportMetadata("Title", "Calculator")]
    [ExportMetadata("Description", "Kancolle Experience Farming Calculator")]
    [ExportMetadata("Version", "1.0.0")]
    [ExportMetadata("Author", "@Madmanmayson, @Zharay")]

    public class Calculator : ITool, IPlugin
    {
        private ToolViewModel toolVM;

        public string Name => "Exp Calculator";
        public object View => new ToolView { DataContext = this.toolVM };

        public Calculator()
        {
            this.toolVM = new ToolViewModel();
        }

        public void Initialize()
        {

        }
    }
}
