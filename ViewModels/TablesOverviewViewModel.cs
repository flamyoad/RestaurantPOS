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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace FlamyPOS.ViewModels
{
    public class TablesOverviewViewModel : ObservableObject
    {
        public ICommand OpenTableCommand { get; set; }
        public ICommand AdminOptionsCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand TableEditorOptionsCommand { get; set; }
        public ICommand UserProfileCommand { get; set; }

        public Staff CurrentUser { get; set; }

        public Dictionary<string, int> OrderIdByTable;
        public Dictionary<string, ObservableCollection<OrderLine>> OrderlinesByTable;
        public Dictionary<string, DateTime> OrderDateByTable;

        private string _date;
        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                Mains.AdminVM.Date = value;
                OnPropertyChanged();
            }
        }

        private string _time;
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                Mains.AdminVM.Time = value;
                if (Mains.TableChangeVM != null) { Mains.TableChangeVM.Time = value; }
                OnPropertyChanged();
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            // get => CurrentUser.Position == Access.Supervisor ? true : false;
            get => _isAdmin;
            set { _isAdmin = value; OnPropertyChanged(); }
        }

        public int OrderId { get; private set; }

        public TablesOverviewViewModel()
        {
            OpenTableCommand = new RelayCommand<string>(OpenTable);
            AdminOptionsCommand = new RelayCommand(OpenAdminOptions);
            LogOutCommand = new RelayCommand(LogOut);
            TableEditorOptionsCommand = new RelayCommand(TableEditorOptions);
            UserProfileCommand = new RelayCommand(UserProfile);

            var currentDate = DateTime.Now.Date;
            Date = DateTime.Now.Date.ToString("dddd, dd MMMM yyyy");
            Time = DateTime.Now.ToString("hh:mm:ss tt");

            #region DispatcherTimer
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += ((sender, e) => { Time = DateTime.Now.ToString("h:mm:ss tt"); });
            timer.Tick += ((sender, e) =>
            {
                if (DateTime.Now.Date != currentDate.Date)
                {
                    Date = DateTime.Now.Date.ToString("dddd, dd MMMM yyyy");
                }
            });
            timer.Start();
            #endregion
        }

        public void Initialize()
        {
            OrderIdByTable = Database.GetOrderIdByTable();
            OrderDateByTable = Database.GetOrderDateByTable();
            OrderlinesByTable = new Dictionary<string, ObservableCollection<OrderLine>>();
            foreach (var item in OrderIdByTable)
            {
                ObservableCollection<OrderLine> orderLines = Database.GetOrderLine(item.Value);
                OrderlinesByTable.Add(item.Key, orderLines);
            }
        }

        private void OpenTable(string tableName)
        {
            OrderIdByTable.TryGetValue(tableName, out int orderId);
            OrderlinesByTable.TryGetValue(tableName, out ObservableCollection<OrderLine> orderline);
            OrderDateByTable.TryGetValue(tableName, out DateTime dt);
            Mains.OrderVM.NotifyTableClicked(tableName, orderId, CurrentUser, orderline, dt);
            Helpers.SwitchWindow(this, new OrderingWindow());
        }

        private void TableEditorOptions()
        {
            Helpers.SwitchWindow(this, new TableEditor());
        }

        private void OpenAdminOptions()
        {
            Helpers.SwitchWindow(this, new AdminOverview());
        }

        private void LogOut()
        {
            Helpers.SwitchWindow(this, new MainWindow());
        }

        private void UserProfile()
        {
            Helpers.SwitchWindow(this, new UserProfileWindow());
        }
    }
}
