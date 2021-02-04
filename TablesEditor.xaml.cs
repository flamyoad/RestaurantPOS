using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for TablesModify.xaml
    /// </summary>
    public partial class TableEditor : Window, INotifyPropertyChanged
    {
        private double LayoutHeight;
        private double LayoutWidth;
        private List<Table> TableList;
        private Dictionary<string, int> TableIdByName;

        public TableEditor()
        {
            InitializeComponent();
            DataContext = this;
            TableList = Mains.TableButtonList;

            TableIdByName = TableList.Select(x => new { x.Name, x.Id })
                .ToDictionary(x => x.Name, x => x.Id);
        }

        private string _tableNameTextBoxContent;
        public string TableNameTextBoxContent
        {
            get => _tableNameTextBoxContent;
            set { _tableNameTextBoxContent = value; OnPropertyChanged(); }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            string tableName = (tableNameTextBox.Text).Trim();

            if (String.IsNullOrWhiteSpace(TableNameTextBoxContent))
            {
                MessageBox.Show("Table name cannot be empty!", "Table Layout Editor", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TableList.Any(x => x.Name == tableName))
            {
                MessageBox.Show("Duplicate table name!", "Table Layout Editor", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            var btn = new TableButton(TableLayout, tableName)
            {
                Height = 80,
                Width = 125,
            };
            TableLayout.Children.Add(btn);
            tableNameTextBox.Clear();
        }

        private void Show_Data_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            foreach (TableButton btn in TableLayout.Children)
            {
                builder.Append($"[Table {btn.TableName}], ".PadRight(20));
                builder.Append($"X = {btn.PrevX}, ".PadRight(25));
                builder.Append($"Y = {btn.PrevY}".PadRight(25));
                builder.Append("\n");                   
            }
            MessageBox.Show(builder.ToString());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutHeight = TableLayout.ActualHeight;
            LayoutWidth = TableLayout.ActualWidth;
            foreach (var item in TableList)
            {
                var button = new TableButton(TableLayout, item.Id, item.Name, item.CoordsX, item.CoordsY, item.IsTaken)
                {
                    Height = 80,
                    Width = 125,
                };
                TableLayout.Children.Add(button);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            new TablesOverview().Show();
            this.Close();
        }

        private void SaveAndReturn_Click(object sender, RoutedEventArgs e)
        {
            var TableTakenByTableName = TableList.ToDictionary(x => x.Name, x => x.IsTaken);
            TableList.Clear();
            foreach (TableButton button in TableLayout.Children)
            {
                TableIdByName.TryGetValue(button.TableName, out int id);
                //if (!tableExists)
                //{
                //    Mains.OrderVM.TableIdByName.Add(button.TableName, button.TableId);
                //}
                TableTakenByTableName.TryGetValue(button.TableName, out bool isTaken);
                TableList.Add(new Table()
                {
                    Id = id,
                    UUID = button.UUID,
                    Name = button.TableName,
                    IsTaken = isTaken,
                    CoordsX = button.PrevX,
                    CoordsY = button.PrevY
                });
            }
            e.Handled = true;
            //Mains.TableButtonList = tableList;
            Database.OverwriteTables(TableList);
            new TablesOverview().Show();
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
