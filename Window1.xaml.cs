using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            var buttonList = Mains.TableButtonList;

            foreach (var item in buttonList)
            {
                var button = new Button();
                Canvas.SetLeft(button, item.CoordsX);
                Canvas.SetTop(button, item.CoordsY);
                button.Width = 125;
                button.Height = 80;
                button.Content = item.Name;
                canvas.Children.Add(button);
            }
        }
    }
}
