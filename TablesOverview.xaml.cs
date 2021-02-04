using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FlamyPOS.ViewModels;

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for TablesOverview.xaml
    /// </summary>
    public partial class TablesOverview : Window
    {
        public TablesOverview()
        {
            InitializeComponent();
            this.DataContext = Mains.TableOverviewVM;
            var buttonlist = Mains.TableButtonList;

            foreach (var item in buttonlist)
            {
                var button = new Button
                {
                    Width = 125,
                    Height = 80,
                    Content = item.Name,
                    Tag = item.IsTaken.ToString(),
                    Style = this.FindResource("TableButton") as Style,                    
                };
                button.SetBinding(Button.CommandProperty, new Binding("OpenTableCommand"));
                Binding commandParameterBinding = new Binding
                {
                    RelativeSource = RelativeSource.Self,
                    Path = new PropertyPath(Button.ContentProperty)
                };
                button.SetBinding(Button.CommandParameterProperty, commandParameterBinding);
                Canvas.SetLeft(button, item.CoordsX);
                Canvas.SetTop(button, item.CoordsY);
                TableLayout.Children.Add(button);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string s = $"Width: {TableLayout.ActualWidth}\nHeight: {TableLayout.ActualHeight}";
        }

    }
}
