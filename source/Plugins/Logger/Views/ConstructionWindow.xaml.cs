using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Logger.Views
{
    /// <summary>
    /// Interaction logic for ConstructionWindow.xaml
    /// </summary>
    public partial class ConstructionWindow
    {
        public ConstructionWindow()
        {
            InitializeComponent();

			Application.Current.MainWindow.Closed += (sender, args) => this.Close();
		}

		private void NumbersOnly(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]{4}");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
