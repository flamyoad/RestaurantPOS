using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlamyPOS.Models
{
    public class OrderLine : ObservableObject
    {
        public OrderLine()
        {
            Id = -1;
        }

        public int Id { get; set; }

        public Product SelectedProduct { get; set; }
        
        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalPrice)); }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set { _totalPrice = value; OnPropertyChanged(); }
        }

        public OrderLine Clone()
        {
            return (OrderLine)MemberwiseClone();
        }

    }
}
