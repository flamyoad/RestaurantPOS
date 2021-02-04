using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
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

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for ChangePriceDialog.xaml
    /// </summary>
    public partial class ChangePriceDialog : Window
    {
        public ChangePriceDialog()
        {
            InitializeComponent();
            DataContext = Mains.ChangePriceVM;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
