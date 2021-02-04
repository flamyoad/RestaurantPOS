using FlamyPOS.Data;
using FlamyPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlamyPOS.ViewModels
{
    public static class Mains
    {
        public static AdminViewModel AdminVM;
        public static OrderingWindowViewModel OrderVM;
        public static LoginViewModel LoginVM;
        public static TableChangeViewModel TableChangeVM;
        public static TablesOverviewViewModel TableOverviewVM;
        public static PaymentViewModel PaymentVM;
        public static ChangePriceViewModel ChangePriceVM;
        public static AddProductViewModel AddProdVM;
        public static TablesEditorViewModel TableEditVM;
        public static ChangeDueViewModel ChangeDueVM;
        public static UserProfileViewModel UserProfileVM;

        public static Dictionary<int, Product> ProductById;
        public static Dictionary<string, int> TableIdByName;
        public static List<Table> TableButtonList;
        
        public static void Initialize()
        {
            AdminVM = new AdminViewModel();
            OrderVM = new OrderingWindowViewModel();
            LoginVM = new LoginViewModel();
            TableOverviewVM = new TablesOverviewViewModel();
            TableChangeVM = new TableChangeViewModel();
            PaymentVM = new PaymentViewModel();
            ChangePriceVM = new ChangePriceViewModel();
            AddProdVM = new AddProductViewModel();
            TableEditVM = new TablesEditorViewModel();
            ChangeDueVM = new ChangeDueViewModel();
            UserProfileVM = new UserProfileViewModel();

            ProductById = Database.GetProductById();
            TableIdByName = Database.GetTableIdByName();
            TableButtonList = Database.GetTables();
        }

        public static List<SolidColorBrush> GetBrushFromList = new List<SolidColorBrush>()
        {
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.Tomato,
            Brushes.Tan,
            Brushes.SteelBlue,
            Brushes.Azure,
            Brushes.BlanchedAlmond,
            Brushes.Orange,
            Brushes.Cyan,
            Brushes.CornflowerBlue,
            Brushes.Cornsilk,
            Brushes.Crimson,
        };
    }
}
