using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlamyPOS.Models
{
    public class Product : ObservableObject
    {
        private int _Id;
        public int ID
        {
            get => _Id;
            set { _Id = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        private int _categoryID;
        public int CategoryID
        {
            get => _categoryID;
            set { _categoryID = value; OnPropertyChanged(); }
        }
                
        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }
    }
}
