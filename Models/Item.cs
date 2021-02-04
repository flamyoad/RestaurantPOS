using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;

namespace FlamyPOS.Models
{
    public class Item : ObservableObject
    {
        public Item()
        {

        }

        // Copy constructor
        public Item(Item item)
        {
            this.Name = item.Name;
            this.ProductCategory = item.ProductCategory;
            this.TotalSale = item.TotalSale;
            this.QuantitySold = item.QuantitySold;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private string _productCategegory;
        public string ProductCategory
        {
            get => _productCategegory;
            set { _productCategegory = value; OnPropertyChanged(); }
        }

        private double _totalSale;
        public double TotalSale
        {
            get => _totalSale;
            set { _totalSale = value; OnPropertyChanged(); }
        }

        private int _quantitySold;
        public int QuantitySold
        {
            get => _quantitySold;
            set { _quantitySold = value; OnPropertyChanged(); }
        }

        private SolidColorBrush _brush;
        public SolidColorBrush Brush
        {
            get => _brush;
            set { _brush = value; OnPropertyChanged(); }
        }
    }
}
