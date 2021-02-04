using FlamyPOS.Models;
using FlamyPOS.Utilities;
using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    /// Interaction logic for OrderingWindow.xaml
    /// </summary>
    public partial class OrderingWindow : Window
    {
        public OrderingWindow()
        {
            DataContext  = Mains.OrderVM;
            InitializeComponent();
        }
    }
}
