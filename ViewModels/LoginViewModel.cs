using FlamyPOS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlamyPOS.Utilities;
using FlamyPOS.Models;
using FlamyPOS.Data;

namespace FlamyPOS.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        //// ICommands ///
        public ICommand NumpadClickCommand { get; set; }
        public ICommand LoginCommand { get; set; }

        private int failCount;
        private SecureString StoredPW { get; set; }
        public Dictionary<string, int> StaffDictionary; // Password ... StaffID

        private string _passcode;
        public string Passcode
        {
            get => _passcode;
            set { _passcode = value; OnPropertyChanged(); }
        }

        private string _warningMessage;
        public string WarningMessage
        {
            get => _warningMessage;
            set { _warningMessage = value; OnPropertyChanged(); }
        }

        private bool _showWarningMessage;
        public bool ShowWarningMessage
        {
            get => _showWarningMessage;
            set { _showWarningMessage = value; OnPropertyChanged(); }
        }

        private bool _warningMessageVisibility;
        public bool WarningMessageVisibility
        {
            get => _warningMessageVisibility;
            set { _warningMessageVisibility = value; OnPropertyChanged(); }
        }

        private bool _buttonEnabled;
        public bool ButtonEnabled
        {
            get => _buttonEnabled;
            set { _buttonEnabled = value; OnPropertyChanged(); }
        }

        public LoginViewModel()
        {
            StoredPW = new SecureString();
            StaffDictionary = new Dictionary<string, int>();
            ButtonEnabled = true;
            failCount = 0;
            WarningMessageVisibility = true;

            NumpadClickCommand = new RelayCommand<string>(NumpadClick);
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            if (StoredPW.Length == 0)
                return;

            if (ValidatePasscode())
            {
                failCount = 0;
                Helpers.SwitchWindow(this, new TablesOverview());
            }
            else
            {
                WarningMessageVisibility = true;
                ShowWarningMessage = true;
                if (failCount < 3)
                {
                    failCount++;
                }
                if (failCount >= 3)
                {
                    WarningMessageVisibility = false;
                    ButtonEnabled = false;
                    var timer = new Timer { Interval = 7000, Enabled = true }; // 7 sec timer
                    timer.Elapsed += ((sender, e) =>
                    {
                        ButtonEnabled = true;
                        timer.Stop();
                        timer.Close();
                        failCount = 0;
                    });
                }
                else
                {
                    var timer = new Timer { Interval = 1000, Enabled = true }; // 1 sec timer
                    timer.Elapsed += ((sender, e) =>
                    {
                        ShowWarningMessage = false;
                        timer.Stop();
                        timer.Close();
                    });
                }
            }
        }

        private bool ValidatePasscode()
        {
            string input = SecureStringToString(StoredPW);

            // ***********************************************************
            // first-time login only, remove this code segment
            if (input == "9999")
            {
                var user = new Staff()
                {
                    Id = 9999,
                    Name = "Default",
                    Position = "Supervisor",
                    JoinedDate = new DateTime(),
                    LeaveDate = new DateTime()
                };
                Mains.TableOverviewVM.CurrentUser = user;
                Mains.AdminVM.CurrentUser = user;
                Mains.UserProfileVM.CurrentUser = user;
                Mains.TableOverviewVM.IsAdmin = user.IsAdmin;
                Mains.OrderVM.IsAdmin = user.IsAdmin;
                return true;
            }
            // ************************************************************

            string hashed = Security.GetHash(input);
            Passcode = String.Empty;
            StoredPW.Clear();
            if(StaffDictionary.ContainsKey(hashed))
            {
                int id = StaffDictionary[hashed];
                var user = Database.GetStaff(id);  // CONNECTION IS OPEN 
                Mains.TableOverviewVM.CurrentUser = user;
                Mains.AdminVM.CurrentUser = user;
                Mains.UserProfileVM.CurrentUser = user;
                Mains.TableOverviewVM.IsAdmin = user.IsAdmin;
                Mains.OrderVM.IsAdmin = user.IsAdmin;
                return true;
            }
            else
            {
                WarningMessage = "Invalid passcode. Please try again";
                return false;
            }
        }

        private void NumpadClick(string str)
        {
            if(str == "Backspace")
            {
                if(Passcode == null || Passcode.Length == 0) { return; }
                StoredPW.RemoveAt(Passcode.Length - 1);
                Passcode = Passcode.Substring(0, Passcode.Length - 1);
            }
            else
            {
                Passcode += "*";
                StoredPW.AppendChar(Convert.ToChar(str));
            }
        }

        public void Initialize()
        {
            StaffDictionary = Database.GetPasscodes();
        }

        private static string GetHash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static string SecureStringToString(SecureString s)
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToBSTR(s);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }
    }
}


