using FlamyPOS.Commands;
using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class PaymentViewModel : ObservableObject
    {
        private double _totalBill;
        public double TotalBill
        {
            get => _totalBill;
            set { _totalBill = value; OnPropertyChanged(); }
        }

        private string _totalPayment;
        public string TotalPayment
        {
            get => _totalPayment;
            set { _totalPayment = value; OnPropertyChanged(); }
        }

        public string TableNumber { get; set; }
        public int OrderId { get; set; }

        public ICommand NumberBtnClickCommand { get; set; }
        public ICommand CancelBtnCommand { get; set; }
        public ICommand ConfirmBtnCommand { get; set; }

        public PaymentViewModel()
        {
            NumberBtnClickCommand = new RelayCommand<string>(NumberBtnClick);
            CancelBtnCommand = new RelayCommand(CancelBtn);
            ConfirmBtnCommand = new RelayCommand(ConfirmClick);
        }

        private void NumberBtnClick(string param)
        {
            char chr = param[0];

            if (chr == 'C')
            {
                TotalPayment = String.Empty;
            }
            else if (chr == '.')
            {
                if (String.IsNullOrEmpty(TotalPayment))
                    TotalPayment += "RM 0.";
                else
                    TotalPayment += ".";
            }
            else if (TotalPayment == "0")
            {
                return;
            }
            else
            {
                if(String.IsNullOrEmpty(TotalPayment))
                {
                    TotalPayment = "RM " + param;
                }
                else
                {
                    TotalPayment += chr;
                }
            }            
        }

        private void CancelBtn()
        {
            Helpers.CloseWindow(this);
            TotalPayment = String.Empty;
        }

        private void ConfirmClick()
        {
            if (String.IsNullOrEmpty(TotalPayment))
                return;

            string numericValues = TotalPayment.Trim('R', 'M', ' ');
            double totalPayment = double.Parse(numericValues);
            if(totalPayment < TotalBill)
            {
                MessageBox.Show("Not enough");
            }
            else
            {
                Mains.TableOverviewVM.OrderlinesByTable.Remove(TableNumber);
                Mains.OrderVM.OrderLines.Clear();
                Mains.OrderVM.prevTransaction.Clear();

                Database.SetTableTaken(TableNumber, false);
                Database.CommencePayment(OrderId);

                //Helpers.CloseWindow(this);
                //Mains.OrderVM.ReturnToTableMenu();
                Mains.ChangeDueVM.ChangeDue = totalPayment - TotalBill;
                Helpers.CloseWindow(this);
                new ChangeDueWindow().ShowDialog();
            }
        }
    }
}
