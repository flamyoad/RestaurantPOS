using FlamyPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlamyPOS.Commands;
using FlamyPOS.Utilities;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using FlamyPOS.Data;

namespace FlamyPOS.ViewModels
{
    public class OrderingWindowViewModel : ObservableObject
    {
        private ObservableCollection<Product> _listOfProducts;
        public ObservableCollection<Product> ListOfProducts
        {
            get => _listOfProducts;
            set { _listOfProducts = value; OnPropertyChanged(); }
        }

        // Binded into View
        private ObservableCollection<OrderLine> _orderLines = new ObservableCollection<OrderLine>();
        public ObservableCollection<OrderLine> OrderLines
        {
            get => _orderLines;
            set => _orderLines = value;
        }

        private double _totalBill = 0;
        public double TotalBill
        {
            get => _totalBill;
            set { _totalBill = value; OnPropertyChanged(); }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        private string _tableName;
        public string TableName
        {
            get => _tableName;
            set { _tableName = value; OnPropertyChanged(); }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set { _isAdmin = value; OnPropertyChanged(); }
        }

        private bool _paymentEnabled;
        public bool PaymentEnabled
        {
            get => _paymentEnabled;
            set { _paymentEnabled = value; OnPropertyChanged(); }
        }

        private bool _tableChangeEnabled;
        public bool TableChangeEnabled
        {
            get => _tableChangeEnabled;
            set { _tableChangeEnabled = value; OnPropertyChanged(); }
        }

        public Dictionary<string, int> TableIdByName;
        public Staff CurrentUser { get; set; }
        public int OrderId { get; set; }

        #region Storing previous values
        public ObservableCollection<OrderLine> prevTransaction { get; set; }
        public double prevTotalBill { get; private set; }
        #endregion

        #region MenuItemCommands
        public ObservableCollection<Product> Appetizers { get; set; } 
        public ObservableCollection<Product> MainDishes { get; set; } 
        public ObservableCollection<Product> Beverages { get; set; } 
        public ObservableCollection<Product> Desserts { get; set; } 
        public ObservableCollection<Product> Others { get; set; }  
        
        public ICommand AppetizersMenuCommand { get; set; }
        public ICommand MainDishesMenuCommand { get; set; }
        public ICommand BeveragesMenuCommand { get; set; }
        public ICommand DessertsMenuCommand { get; set; }
        public ICommand OthersMenuCommand { get; set; }
        public ICommand ClickMenuItemCommand { get; set; }
        public ICommand ReturnMenuCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand PaymentCommand { get; set; }
        public ICommand ChangeTableCommand { get; set; }
        #endregion


        public OrderingWindowViewModel()
        {
            #region RelayCommands

            AppetizersMenuCommand = new RelayCommand(() => ListOfProducts = Appetizers);
            MainDishesMenuCommand = new RelayCommand(() => ListOfProducts = MainDishes);
            BeveragesMenuCommand = new RelayCommand(() => ListOfProducts = Beverages);
            DessertsMenuCommand = new RelayCommand(() => ListOfProducts = Desserts);
            OthersMenuCommand = new RelayCommand(() => ListOfProducts = Others);
            ClickMenuItemCommand = new RelayCommand<Product>(ClickMenuItem);
            ReturnMenuCommand = new RelayCommand(ReturnMenu);
            SubmitCommand = new RelayCommand(Submit);
            PaymentCommand = new RelayCommand(Payment);
            LogOutCommand = new RelayCommand(Logout);
            ChangeTableCommand = new RelayCommand(ChangeTable);
            #endregion
        }

        public void Initialize()
        {
            Appetizers = Database.GetProducts(ProductCategory.Appetizers);
            MainDishes = Database.GetProducts(ProductCategory.MainDishes);
            Beverages = Database.GetProducts(ProductCategory.Beverages);
            Desserts = Database.GetProducts(ProductCategory.Desserts);
            Others = Database.GetProducts(ProductCategory.Others);
            TableIdByName = Mains.TableIdByName;
        }

        public void NotifyTableClicked(string tableNumb, 
                                       int orderId, 
                                       Staff staff, 
                                       ObservableCollection<OrderLine> orderlines,
                                       DateTime dt)
        {
            if (tableNumb == null) return;

            this.TableName = tableNumb;
            this.CurrentUser = staff;
            this.OrderId = orderId;
            this.OrderLines = orderlines == null ? new ObservableCollection<OrderLine>() : orderlines;
            this.Date = dt == default(DateTime) ? DateTime.Now : dt;

            //TotalBill = OrderLines.Sum(x => x.Quantity * x.SelectedProduct.Price);
            TotalBill = OrderLines.Sum(x => x.TotalPrice);

            prevTransaction = OrderLines.GetShallowClones();
            prevTotalBill = TotalBill;
            PaymentEnabled = prevTotalBill != 0;
            TableChangeEnabled = prevTransaction.Count() != 0;
        }

        private void ReturnMenu()
        {
            Mains.TableOverviewVM.OrderlinesByTable[TableName] = prevTransaction;
            Helpers.SwitchWindow(this, new TablesOverview());
        }

        private void Submit()
        {
            if(OrderLines.Count() == 0)
            {
                Helpers.SwitchWindow(this, new TablesOverview());
                return;
            }

            Mains.TableOverviewVM.OrderlinesByTable[TableName] = OrderLines;

            string orderId = OrderId == 0 ? "NULL" : OrderId.ToString();
            int lastId = Database.InsertOrder($@"{orderId},
                                              {CurrentUser.Id},
                                              {TableIdByName[TableName]},
                                              '{Date.ToString("yyyy-MM-dd HH:mm:ss")}',
                                              {TotalBill},
                                               0"                  
                                               
                                               ,TotalBill);
            OrderId = lastId;
            Mains.TableOverviewVM.OrderIdByTable[TableName] = lastId;

            foreach (var line in OrderLines)
            {
                string orderLineId = line.Id == -1 ? "NULL" : line.Id.ToString();
                int id = Database.Replace(Columns.ORDERLINE, $@"{orderLineId},
                                                                {lastId},
                                                                {line.SelectedProduct.ID},
                                                                {line.TotalPrice},
                                                                {line.Quantity}");
                line.Id = id;
            }

            prevTransaction.Clear();
            Database.SetTableTaken(TableName, true);

            Helpers.SwitchWindow(this, new TablesOverview());
        }

        private void Logout()
        {
            Helpers.SwitchWindow(this, new MainWindow());
        }

        private void ClickMenuItem(Product product)
        {
            var orderLine = OrderLines.SingleOrDefault 
                (x => x.SelectedProduct.Name == product.Name);

            if(orderLine == null)
            {
                OrderLines.Add(new OrderLine()
                {
                    SelectedProduct = product,
                    Quantity = 1,
                    TotalPrice = product.Price
                });
            }
            else
            {
                orderLine.Quantity++;
                orderLine.TotalPrice += product.Price;
            }
            TotalBill += product.Price;
        }

        private void Payment()
        {
            if(prevTotalBill == 0)
            {
                return;
            }

            Mains.PaymentVM.TableNumber = this.TableName;
            Mains.PaymentVM.TotalBill = this.TotalBill;
            Mains.PaymentVM.OrderId = this.OrderId;
            var window = new PaymentWindow();
            window.ShowDialog();
        }

        private void ChangeTable()
        {
            if (prevTransaction.Count() == 0)
            {               
                return;
            }

            Mains.TableChangeVM.NotifyTableClicked(TableName, CurrentUser);
            Helpers.SwitchWindow(this, new TableChange());
        }

        public void ReturnToTableMenu()
        {
            Helpers.SwitchWindow(this, new TablesOverview());
        }
    }
}
