using FlamyPOS.Commands;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class ChangeDueViewModel : ObservableObject
    {
        public ICommand ReturnCommand { get; set; }

        private double _changeDue;
        public double ChangeDue
        {
            get => _changeDue;
            set { _changeDue = value; OnPropertyChanged(); }
        }

        public ChangeDueViewModel()
        {
            ReturnCommand = new RelayCommand(Return);
        }

        private void Return()
        {
            Mains.PaymentVM.TotalPayment = String.Empty;
            Mains.OrderVM.ReturnToTableMenu();
            Helpers.CloseWindow(this);
        }
    }
}
