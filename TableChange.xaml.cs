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
    /// Interaction logic for TableChange.xaml
    /// </summary>
    public partial class TableChange : Window
    {
        public TableChange()
        {
            InitializeComponent();
            DataContext = Mains.TableChangeVM;
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
                button.SetBinding(Button.CommandProperty, new Binding("ChangeTableCommand"));
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
    }
}
