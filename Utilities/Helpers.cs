using FlamyPOS.Models;
using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace FlamyPOS.Utilities
{
    public static class Helpers
    {
        /// <summary>
        /// First argument = this, Second argument = New window to be opened
        /// [Works in viewmodel classes only]
        /// </summary>
        /// <param name="param"></param>
        public static void SwitchWindow(object oldWindow, Window newWindow)
        {
            newWindow.Show();
            CloseWindow(oldWindow);
        }

        /// <summary>
        /// [Works in viewmodel classes only]
        /// </summary>
        /// <param name="param"></param>
        public static void CloseWindow(object param)
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == param) item.Close();
            }
        }

        public static bool IsNumericString(string str)
        {
            if (str == null) { return false; }

            bool isNumeric = str.All(Char.IsDigit);
            return isNumeric;          
        }

        public static string GetUnescapedString(string str)
        {
            return str.Replace("'", "''")
                      .Replace(@"\", @"\\")
                      .Replace(";", "");
        }

        public static ObservableCollection<OrderLine> GetShallowClones
            (this ObservableCollection<OrderLine> collection)
        {
            var query =
                from n in collection
                select n.Clone();
            return new ObservableCollection<OrderLine>(query);
        }

        public static ObservableCollection<Product> GetProdCategory(int ProdCategoryIndex)
        {
            switch (ProdCategoryIndex)
            {
                case 0:
                    return Mains.OrderVM.Appetizers;
                case 1:
                    return Mains.OrderVM.MainDishes;
                case 2:
                    return Mains.OrderVM.Beverages;
                case 3:
                    return Mains.OrderVM.Desserts;
                case 4:
                    return Mains.OrderVM.Others;
                default:
                    Debug.WriteLine("Failed to get product list [Helpers.cs]");
                    return new ObservableCollection<Product>();
            }
        }
    }
}
