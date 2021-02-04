using FlamyPOS.Commands;
using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class ChangePriceViewModel : ObservableObject
    {
        public ChangePriceViewModel()
        {
            ConfirmPriceChangeCommand = new RelayCommand(ConfirmPriceChange);
            NumberBtnClickCommand = new RelayCommand<string>(NumberBtnClick);
        }

        //private Regex rgx = new Regex(@"/^[0-9]+.{1}[0-9]{1,2}$/");
        public Product SelectedItem { get; set; }

        private string _currentPrice;
        public string CurrentPrice
        {
            get => _currentPrice;
            set { _currentPrice = value; OnPropertyChanged(); }
        }

        private string _newPrice;
        public string NewPrice
        {
            get => _newPrice;
            set { _newPrice = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmPriceChangeCommand { get; set; }
        private void ConfirmPriceChange()
        {
            string s = String.Concat(
                       NewPrice.SkipWhile(x => Char.IsLetter(x))
                       );
            bool status = Double.TryParse(s, NumberStyles.Currency, CultureInfo.InvariantCulture, out double price);

            if(price == 0)
            {
                MessageBox.Show("Product Price cannot be zero !", "Flamy POS", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if(status)
            {
                SelectedItem.Price = price;
            }
            else
            {
                return;
            }

            // insert database code here
            Database.ChangePrice(SelectedItem);

            // very ugly code please do not repeat this mistake
            var orderlineListCollection = Mains.TableOverviewVM.OrderlinesByTable.Values.ToList();
            foreach (var orderlineList in orderlineListCollection)
            {
                if (orderlineList.Any(x => x.SelectedProduct.ID == SelectedItem.ID))
                {
                    var orderline = orderlineList.FirstOrDefault(x => x.SelectedProduct.ID == SelectedItem.ID);
                    orderline.SelectedProduct.Price = price;
                    orderline.TotalPrice = orderline.Quantity * orderline.SelectedProduct.Price;
                }
            }
            Helpers.CloseWindow(this);
        }

        public ICommand NumberBtnClickCommand { get; set; }
        private void NumberBtnClick(string param)
        {
            char chr = param[0];

            if (chr == 'C')
            {
                NewPrice = String.Empty;
            }
            else if (chr == '.')
            {
                if (String.IsNullOrEmpty(NewPrice))
                    NewPrice += "RM 0.";
                else
                    NewPrice += ".";
            }
            else if (NewPrice == "0")
            {
                return;
            }
            else
            {
                if (String.IsNullOrEmpty(NewPrice))
                {
                    NewPrice = "RM " + param;
                }
                else
                {
                    NewPrice += chr;
                }
            }
        }

    }
}
