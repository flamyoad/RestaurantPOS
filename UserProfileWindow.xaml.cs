using FlamyPOS.Data;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using FlamyPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for UserProfileWindow.xaml
    /// </summary>
    public partial class UserProfileWindow : Window
    {
        public UserProfileWindow()
        {
            InitializeComponent();
            DataContext = Mains.UserProfileVM;
        }

        private void Change_Passcode_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(CurrentPasscodeBox.Password) ||
                String.IsNullOrWhiteSpace(NewPasscodeBox.Password)     ||
                String.IsNullOrWhiteSpace(ConfirmPasscodeBox.Password))
            {
                MessageBox.Show("Please fill in all the columns!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            if (!Helpers.IsNumericString(CurrentPasscodeBox.Password) ||
                !Helpers.IsNumericString(NewPasscodeBox.Password) ||
                !Helpers.IsNumericString(ConfirmPasscodeBox.Password))
            {
                MessageBox.Show("Must contain numeric passcodes only!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            Staff user = Mains.UserProfileVM.CurrentUser;
            string hashedPasscodeInput = Security.GetHash(CurrentPasscodeBox.Password);
            /*Mains.LoginVM.StaffDictionary*/ //Passcode ... StaffID
            string currentHashedPasscode = Mains.LoginVM.StaffDictionary
                                                        .FirstOrDefault(x => x.Value == user.Id)
                                                        .Key;
            if (hashedPasscodeInput != currentHashedPasscode)
            {
                MessageBox.Show("Please enter the correct passcode!");
                return;
            }

            if (NewPasscodeBox.Password != ConfirmPasscodeBox.Password)
            {
                MessageBox.Show("New passcode and Confirm passcode must match!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            string hashedNewPasscode = Security.GetHash(NewPasscodeBox.Password);
            bool status = Database.UpdatePasscode(user.Name, hashedNewPasscode);

            if (status)
            {
                Mains.LoginVM.StaffDictionary.Remove(Security.GetHash(CurrentPasscodeBox.Password));
                Mains.LoginVM.StaffDictionary.Add(hashedNewPasscode, user.Id);
                MessageBox.Show("Password changed successfully!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Information);
                CurrentPasscodeBox.Clear();
                NewPasscodeBox.Clear();
                ConfirmPasscodeBox.Clear();
            }
            else
            {
                MessageBox.Show("Duplicate passcode!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Staff user = Mains.UserProfileVM.CurrentUser;
            string passcode = DeleteAccountPasswordBox.Password;
            if (String.IsNullOrWhiteSpace(passcode))
            {
                return;
            }

            if (user.IsAdmin)
            {
                bool onlyOneAdminRemaining = Mains.AdminVM.StaffList.Count(x => x.IsAdmin) == 1;
                if (onlyOneAdminRemaining)
                {
                    MessageBox.Show("Can't delete the only one admin!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    DeleteAccountPasswordBox.Clear();
                    return;
                }
            }

            string hashedPasscodeInput = Security.GetHash(passcode);

            /*Mains.LoginVM.StaffDictionary*/ //Passcode ... StaffID
            string currentHashedPasscode = Mains.LoginVM.StaffDictionary
                                                        .FirstOrDefault(x => x.Value == user.Id)
                                                        .Key;

            if (hashedPasscodeInput != currentHashedPasscode)
            {
                MessageBox.Show("Wrong passcode input!", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
            else
            {
                bool status = Database.UpdateStaffPosition(user.Name, Access.Disabled);
                if (!status)
                {
                    MessageBox.Show("Failed to remove account", "FlamyPOS", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    var list = Mains.AdminVM.StaffList
                        .Where(x => x.Name != user.Name)
                        .ToList();
                    Mains.AdminVM.StaffList = new ObservableCollection<Staff>(list);
                    new MainWindow().Show();
                    this.Close();
                }
            }
        }
    }
}
