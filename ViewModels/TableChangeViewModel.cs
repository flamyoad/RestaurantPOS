using FlamyPOS.Commands;
using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class TableChangeViewModel : ObservableObject
    {
        public Dictionary<string, int> OrderIdByTable;
        public Dictionary<string, ObservableCollection<OrderLine>> OrderlinesByTable;
        public Staff CurrentUser;

        private string _time;
        public string Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        private string _previousTableName;
        public string PreviousTableName
        {
            get => _previousTableName;
            set { _previousTableName = value; OnPropertyChanged(); }
        }

        public ICommand ChangeTableCommand { get; set; }
        public ICommand ReturnCommand { get; set; }

        public TableChangeViewModel()
        {
            ChangeTableCommand = new RelayCommand<string>(ChangeTable);
            ReturnCommand = new RelayCommand(Return);
        }

        public void Initialize()
        {
            OrderIdByTable = Mains.TableOverviewVM.OrderIdByTable;
            OrderlinesByTable = Mains.TableOverviewVM.OrderlinesByTable;
        }

        public void NotifyTableClicked(string previousTableName, Staff currentUser)
        {
            this.PreviousTableName = previousTableName;
            this.CurrentUser = currentUser;
        }

        private void ChangeTable(string newTableName)
        {
            if (newTableName == PreviousTableName)
            {
                MessageBox.Show("You should not choose the same table!", "Flamy POS", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
                
            ObservableCollection<OrderLine> listPrevTable;
            ObservableCollection<OrderLine> listNewTable;
            OrderlinesByTable.TryGetValue(PreviousTableName, out listPrevTable);
            bool tableExists = OrderlinesByTable.TryGetValue(newTableName, out listNewTable);

            // newOrderlistcollection contains orders from previous table, 
            // The list gets changed if the new table has orders, else it has same orderlines from previous table
            ObservableCollection<OrderLine> newOrderlistCollection = listPrevTable.GetShallowClones();

            // If listNewTable is null, means the new table chosen is still empty
            if (listNewTable != null)
            {
                var combinedList = listPrevTable.Concat(listNewTable)                   
                    .GroupBy(x => x.SelectedProduct.ID)
                    .Select(g => new OrderLine()
                    {
                        Id = 0,
                        SelectedProduct = g.First().SelectedProduct,
                        Quantity = g.Sum(x => x.Quantity),
                        TotalPrice = g.Sum(x => x.TotalPrice),
                    });
                newOrderlistCollection = new ObservableCollection<OrderLine>(combinedList);
            }

            OrderIdByTable.TryGetValue(PreviousTableName, out int orderIdOfPreviousTable);
            OrderIdByTable.TryGetValue(newTableName, out int orderIdOfNewTable);

            Database.ChangeTable(newOrderlistCollection, CurrentUser, newTableName, PreviousTableName, orderIdOfPreviousTable, orderIdOfNewTable);
            this.Return();
        }

        private void Return()
        {
            Helpers.SwitchWindow(this, new TablesOverview());
        }
    }
}
