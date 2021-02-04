using FlamyPOS.Commands;
using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class AddProductViewModel : ObservableObject
    {
        public ICommand NumberBtnClickCommand { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddProductViewModel()
        {
            NumberBtnClickCommand = new RelayCommand<string>(NumberBtnClick);
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        private string _newProdName;
        public string NewProdName
        {
            get => _newProdName;
            set { _newProdName = value; }
        }

        private string _newProdPrice;
        public string NewProdPrice
        {
            get => _newProdPrice;
            set { _newProdPrice = value; OnPropertyChanged(); }
        }

        private int _prodCategoryIndex;
        public int ProdCategoryIndex
        {
            get => _prodCategoryIndex;
            set { _prodCategoryIndex = value; OnPropertyChanged(); }
        }

        private void NumberBtnClick(string param)
        {
            char chr = param[0];

            if (chr == 'C')
            {
                NewProdPrice = String.Empty;
            }
            else if (chr == '.')
            {
                if (String.IsNullOrEmpty(NewProdPrice))
                    NewProdPrice += "RM 0.";
                else
                    NewProdPrice += ".";
            }
            else if (NewProdPrice == "0")
            {
                return;
            }
            else
            {
                if (String.IsNullOrEmpty(NewProdPrice))
                {
                    NewProdPrice = "RM " + param;
                }
                else
                {
                    NewProdPrice += chr;
                }
            }
        }

        private void Confirm()
        {
            if (String.IsNullOrWhiteSpace(NewProdName) || String.IsNullOrWhiteSpace(NewProdPrice))
            {
                MessageBox.Show("Please complete the details!", "FlamyPOS",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            ObservableCollection<Product> list = Helpers.GetProdCategory(ProdCategoryIndex);

            // Trims the trailing whitespace eg: "Sotong " becomes "Sotong"
            NewProdName = NewProdName.Trim();

            bool hasDuplicate = CheckForDuplicate(NewProdName);
            if (hasDuplicate)
            {
                MessageBox.Show("Duplicate item!", "FlamyPOS", 
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                NewProdName = String.Empty;
                NewProdPrice = String.Empty;
                return;
            }

            string s = NewProdPrice.Trim('R', 'M', ' ');
            bool parseStatus = Double.TryParse(s, NumberStyles.Currency, 
                CultureInfo.InvariantCulture, out double price);

            if (price == 0)
            {
                MessageBox.Show("Product Price cannot be zero !", "Flamy POS", 
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (parseStatus == false)
            {
                MessageBox.Show("Failed to parse currency string");
                return;
            }

            string newProdName = Helpers.GetUnescapedString(NewProdName);
            bool dbStatus = Database.Insert(Columns.PRODUCTS, $@"NULL,
                                                               '{newProdName}',
                                                                {price},
                                                                {ProdCategoryIndex + 1},
                                                                1
                                         ", out int lastUsedId);


            if (dbStatus)
            {
                list.Add(new Product()
                {
                    ID = lastUsedId,
                    Name = NewProdName,
                    Price = price
                });
            }


            NewProdName = String.Empty;
            NewProdPrice = String.Empty;
            Helpers.CloseWindow(this);
        }

        private void Cancel()
        {
            NewProdName = String.Empty;
            NewProdPrice = String.Empty;
            Helpers.CloseWindow(this);
        }

        private bool CheckForDuplicate(string itemName)
        {
            var list = Helpers.GetProdCategory(0)
                .Union(Helpers.GetProdCategory(1))
                .Union(Helpers.GetProdCategory(2))
                .Union(Helpers.GetProdCategory(3))
                .Union(Helpers.GetProdCategory(4));

            return list.Any(x => x.Name == itemName);
        }
    }
}
