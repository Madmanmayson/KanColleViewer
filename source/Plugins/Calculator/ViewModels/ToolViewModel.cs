using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;

namespace Calculator.ViewModels
{
    class ToolViewModel : ViewModel
    {
        #region Calculator

        private CalculatorViewModel _Calculator;

        public CalculatorViewModel Calculator
        {
            get { return this._Calculator; }
            set
            {
                if(this._Calculator != value)
                {
                    this._Calculator = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        public ToolViewModel()
        {
            this.Calculator = CalculatorViewModel.Current;
        }
    }
}
