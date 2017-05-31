using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleViewer.ViewModels;
using Logger.ViewModels.Contents;
using MetroTrilithon.Mvvm;


namespace Logger.ViewModels
{
    class ToolViewModel: ViewModel
    {

        //Tab Items
        public ConstructionViewModel Construction { get; }
        public DevelopmentViewModel Development { get; }

        public IList<TabItemViewModel> LogItems { get; set; }

        #region SelectedItem 変更通知プロパティ

        private TabItemViewModel _SelectedItem;

        public TabItemViewModel SelectedItem
        {
            get { return this._SelectedItem; }
            set
            {
                if (this._SelectedItem != value)
                {
                    this._SelectedItem = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        public ToolViewModel()
        {
            this.LogItems = new List<TabItemViewModel>
            {
                (this.Construction = new ConstructionViewModel().AddTo(this)),
                (this.Development = new DevelopmentViewModel().AddTo(this)),
            };

            this.SelectedItem = this.LogItems.FirstOrDefault();
        }


    }
}
