using FlamyPOS.Commands;
using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace FlamyPOS.ViewModels
{
    public class AdminViewModel : ObservableObject
    {
        public Staff CurrentUser { get; set; }

        #region Main
        public AdminViewModel()
        {
            StaffList = new ObservableCollection<Staff>();
            RemoveStaffCommand = new RelayCommand<int>(RemoveStaff);
            PromoteStaffCommand = new RelayCommand<int>(PromoteStaff);
            DerankStaffCommand = new RelayCommand<int>(DerankStaff);
            ChangePriceCommand = new RelayCommand<int>(ChangePrice);
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand<int>(DeleteProduct);
        }

        public void Initialize()
        {
            StaffList = new ObservableCollection<Staff>(Database.GetAllStaffs());

            // Monthly Sales
            ProductsSold = Database.GetAllProductsOnMonth(DateTime.Now);
            ProductsSoldShown = new ObservableCollection<Item>(ProductsSold.Select(x => new Item(x)));
            MonthlySalesDate = DateTime.Now;
            var list = Enum.GetNames(typeof(ProductCategory));
            FilterCollection = new ObservableCollection<string>(list);

            // Product Sales
            TopProducts = Database.GetTopProductsOnMonth(DateTime.Now);
            for(int i = 0; i < TopProducts.Count; i++)
            {
                TopProducts[i].Brush = Mains.GetBrushFromList[i];
            }
            TotalTopSales = TopProducts.Sum(x => x.TotalSale);
            DrawPieChart();            
        }

        private string _date;
        public string Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        private string _time;
        public string Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }
        #endregion

        #region AddStaff
        private string _addStaffStatusMessage;
        public string AddStaffStatusMessage
        {
            get => _addStaffStatusMessage;
            set { _addStaffStatusMessage = value; OnPropertyChanged(); }
        }
        #endregion

        #region ModifyStaff
        private ObservableCollection<Product> _productList;
        public ObservableCollection<Product> ProductList
        {
            get => _productList;
            set { _productList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Staff> _staffList;
        public ObservableCollection<Staff> StaffList
        {
            get => _staffList;
            set { _staffList = value; OnPropertyChanged(); }
        }

        private bool ModifyStaffPosition(int index, Access access)
        {
            if (index == -1)
                return false;

            if (StaffList[index].Id == CurrentUser.Id)
            {
                MessageBox.Show("Cannot modify/delete yourself!",
                    "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return false;
            }

            bool status = Database.UpdateStaffPosition(StaffList[index].Name, access);
            if (status == false)
            {
                MessageBox.Show("Database query failed");
            }
            return true;
        }

        public ICommand RemoveStaffCommand { get; set; }
        private void RemoveStaff(int index)
        {
            bool status = ModifyStaffPosition(index, Access.Disabled);
            if (status)
            {
                StaffList.RemoveAt(index);
            }
        }

        public ICommand PromoteStaffCommand { get; set; }
        private void PromoteStaff(int index)
        {
            bool status = ModifyStaffPosition(index, Access.Supervisor);
            if (status)
            {
                StaffList[index].Position = nameof(Access.Supervisor);
            }
        }

        public ICommand DerankStaffCommand { get; set; }
        private void DerankStaff(int index)
        {
            bool status = ModifyStaffPosition(index, Access.Staff);
            if (status)
            {
                StaffList[index].Position = nameof(Access.Staff);
            }
        }
        #endregion

        #region ModifyMenu
        private int _menuTabIndex;
        public int MenuTabIndex
        {
            get => _menuTabIndex;
            set { _menuTabIndex = value; OnPropertyChanged(); }
        }

        public ICommand AddProductCommand { get; set; }
        private void AddProduct()
        {
            Mains.AddProdVM.ProdCategoryIndex = MenuTabIndex;
            var addProductDialog = new AddProductDialog();
            addProductDialog.ShowDialog();
        }

        public ICommand ChangePriceCommand { get; set; }
        private void ChangePrice(int productIndex)
        {
            if (productIndex == -1)
                return;

            var selectedList = Helpers.GetProdCategory(MenuTabIndex);

            Mains.ChangePriceVM.SelectedItem = selectedList[productIndex];
            Mains.ChangePriceVM.NewPrice = String.Empty;
            Mains.ChangePriceVM.CurrentPrice = Mains.ChangePriceVM.SelectedItem.Price.ToString();
            var changePriceDialog = new ChangePriceDialog();
            changePriceDialog.ShowDialog();
        }

        public ICommand DeleteProductCommand { get; set; }
        private void DeleteProduct(int productIndex)
        {
            if (productIndex == -1)
                return;

            ObservableCollection<Product> list = Helpers.GetProdCategory(MenuTabIndex);
            Product item = list[productIndex];

            var orderlinesByTable = Mains.TableOverviewVM.OrderlinesByTable;
            bool itemInUse = orderlinesByTable
                .SelectMany(x => x.Value)
                .Any(l => l.SelectedProduct.ID == item.ID);

            if (itemInUse)
            {
                MessageBox.Show("Item is still in use !", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }


            var msgBoxResult = MessageBox.Show($"Are you sure you want to delete {item.Name}?",
                "FlamyPOS", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (msgBoxResult == MessageBoxResult.No)
                return;

            Database.DeleteProduct(item);
            list.RemoveAt(productIndex);
        }
        #endregion

        #region Report Module

        #region Monthly Sales
        private ObservableCollection<Item> _productsSold;
        public ObservableCollection<Item> ProductsSold
        {
            get => _productsSold;
            set { _productsSold = value; OnPropertyChanged(); }
        }

        private ObservableCollection<Item> _productsSoldShown;
        public ObservableCollection<Item> ProductsSoldShown
        {
            get => _productsSoldShown;
            set { _productsSoldShown = value; OnPropertyChanged(); }
        }

        private DateTime _monthlySalesDate;
        public DateTime MonthlySalesDate
        {
            get => _monthlySalesDate;
            set
            {
                _monthlySalesDate = value;
                OnPropertyChanged();
                ProductsSold = Database.GetAllProductsOnMonth(MonthlySalesDate);
                //ProductsSoldShown = new ObservableCollection<Item>(ProductsSold.Select(x => new Item(x)));
                string monthYear = MonthlySalesDate.ToString("MMMM yyyy");
                MonthlySalesMonthLabel = "Product Sales in " + monthYear;
                FilterChoice = Enum.GetName(typeof(ProductCategory), ProductCategory.All);
            }
        }

        private string _monthlySalesMonthLabel;
        public string MonthlySalesMonthLabel
        {
            get => _monthlySalesMonthLabel;
            set { _monthlySalesMonthLabel = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _filterList;
        public ObservableCollection<string> FilterCollection
        {
            get => _filterList;
            set { _filterList = value; OnPropertyChanged(); }
        }

        private string _filterChoice;
        public string FilterChoice
        {
            get => _filterChoice;
            set
            {
                _filterChoice = value;
                OnPropertyChanged();
                if (value == "All") {
                    ProductsSoldShown = new ObservableCollection<Item>(ProductsSold.Select(x => new Item(x)));
                }
                else
                {
                    ProductsSoldShown = new ObservableCollection<Item>(ProductsSold
                                                    .Where(x => x.ProductCategory == value)
                                                    .Select(x => new Item(x)));
                }
            }
        }
        #endregion

        #region Top Products
        public double PieWidth { get; set; } = 475;
        public double PieHeight { get; set; } = 475;        
        public List<SolidColorBrush> GetBrushFromList = Mains.GetBrushFromList;

        private ObservableCollection<Item> _topProducts;
        public ObservableCollection<Item> TopProducts
        {
            get => _topProducts;
            set { _topProducts = value; OnPropertyChanged(); }
        }

        private double _totalTopSales;
        public double TotalTopSales
        {
            get => _totalTopSales;
            set { _totalTopSales = value; OnPropertyChanged(); }
        }

        private DateTime _reportDate = DateTime.Now;
        public DateTime ReportDate
        {
            get => _reportDate;
            set
            {
                _reportDate = value;
                OnPropertyChanged();
                var monthYear = ReportDate.ToString("MMMM yyyy");
                TopProductMonthLabel = "Top Sales in " + monthYear;
                TopProducts = Database.GetTopProductsOnMonth(ReportDate);
                for (int i = 0; i < TopProducts.Count; i++)
                {
                    TopProducts[i].Brush = Mains.GetBrushFromList[i];
                }
                TotalTopSales = TopProducts.Sum(x => x.TotalSale);
                RefreshPieChart();
            }
        }

        private DrawingImage _pieChartImage = new DrawingImage();
        public DrawingImage PieChartImg
        {
            get => _pieChartImage;
            set { _pieChartImage = value; OnPropertyChanged(); }
        }

        private string _topProductMonthLabel = "Top Sales in " + DateTime.Now.ToString("MMMM yyyy");
        public string TopProductMonthLabel
        {
            get => _topProductMonthLabel;
            set { _topProductMonthLabel = value; OnPropertyChanged(); }
        }

        private bool _showPieChart = true;
        public bool ShowPieChart
        {
            get => _showPieChart;
            set { _showPieChart = value; OnPropertyChanged(); }
        }

        private bool _showLegendListBox = true;
        public bool ShowLegendListBox
        {
            get => _showLegendListBox;
            set { _showLegendListBox = value; OnPropertyChanged(); }
        }

        private void RefreshPieChart()
        {
            if(TopProducts.Count == 0)
            {
                TopProductMonthLabel = "No sales in " + ReportDate.ToString("MMMM yyyy");
                ShowPieChart = false;
                ShowLegendListBox = false;
                return;
            }
            DrawPieChart();
            ShowPieChart = true;
            ShowLegendListBox = true;
        }

        private void DrawPieChart()
        {
            var drawingGroup = new DrawingGroup();
            PieChartImg.Drawing = drawingGroup;

            Point startPoint = new Point(PieWidth / 2, 0);
            double radians = 0;
            for (int i = 0; i < TopProducts.Count; i++)
            {
                double sale = TopProducts[i].TotalSale;
                double percentage = sale / TotalTopSales;
                Brush brush = GetBrushFromList[i];
                Point endPoint;
                var angle = 360 * percentage;
                if (i + 1 == TopProducts.Count)
                {
                    endPoint = new Point(PieWidth / 2, 0);
                }
                else
                {
                    radians += (Math.PI / 180) * angle;
                    var endPointX = Math.Sin(radians) * PieHeight / 2 + PieHeight / 2;
                    var endPointY = PieWidth / 2 - Math.Cos(radians) * PieWidth / 2;
                    endPoint = new Point(endPointX, endPointY);
                }
                drawingGroup.Children.Add(CreatePathGeometry(brush, startPoint, endPoint, angle > 180));
                startPoint = endPoint;
            }
        }

        private GeometryDrawing CreatePathGeometry(Brush brush, Point startPoint, Point arcPoint, bool isLargeArc)
        {
            var midPoint = new Point(PieWidth / 2, PieHeight / 2);
            var drawing = new GeometryDrawing { Brush = brush };
            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure
            {
                StartPoint = midPoint
            };
            var ls1 = new LineSegment(startPoint, false);
            var arc = new ArcSegment
            {
                SweepDirection = SweepDirection.Clockwise,
                Size = new Size(PieWidth / 2, PieHeight / 2),
                Point = arcPoint,
                IsLargeArc = isLargeArc
            };

            var ls2 = new LineSegment(midPoint, false);
            drawing.Geometry = pathGeometry;
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments.Add(ls1);
            pathFigure.Segments.Add(arc);
            pathFigure.Segments.Add(ls2);

            return drawing;
        }
        #endregion

        #endregion
    }
}
