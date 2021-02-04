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
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlamyPOS
{
    /// <summary>
    /// Interaction logic for AdminOverview.xaml
    /// </summary>
    public partial class AdminOverview : Window
    {
        public AdminOverview()
        {
            InitializeComponent();
            DataContext = Mains.AdminVM;
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            int len = ((PasswordBox)sender).Password.Length;
            if (len > 0)
            {
                passwordBox.Tag = true.ToString();
            }
            else
            {
                passwordBox.Tag = false.ToString();
            }
        }

        private void submitAddStaff(object sender, RoutedEventArgs e)
        {
            string user = usernameBox.Text;
            string pw = passwordBox.Password;

            if (String.IsNullOrWhiteSpace(user) || 
                String.IsNullOrWhiteSpace(pw))
            {
                Mains.AdminVM.AddStaffStatusMessage = "Data cannot be empty";
                return;
            }

            if (Helpers.IsNumericString(pw) == false) 
            {
                Mains.AdminVM.AddStaffStatusMessage = "Numeric passcodes only!";
                passwordBox.Clear();
                return;
            }

            if (Mains.AdminVM.StaffList.Any(x => x.Name == user)) 
            {
                Mains.AdminVM.AddStaffStatusMessage = "Duplicate username";
                usernameBox.Clear();
                passwordBox.Clear();
                return;
            }

            pw = Security.GetHash(pw);

            var btn = positionBtnPanel.Children.OfType<RadioButton>()
                                      .FirstOrDefault(r => r.IsChecked == true);
            int position = (int)Enum.Parse(typeof(Access), (string)btn.Tag);
                
            DateTime joinDate = DateTime.Now;
            DateTime leaveDate = new DateTime(9999, 12, 31, 11, 59, 59);

            string unescapedName = Helpers.GetUnescapedString(usernameBox.Text);
            bool status = Database.Insert(Columns.STAFFS, $@"NULL, 
                                                            '{unescapedName}',
                                                            '{pw}', 
                                                            '{joinDate.ToString("yyyy-MM-dd HH:mm:ss")}',
                                                            '{leaveDate.ToString("yyyy-MM-dd HH:mm:ss")}',
                                                             {position}",
                                          out int newStaffId);

            if (status)
            {
                Mains.AdminVM.AddStaffStatusMessage = "Data is submitted";
                Mains.AdminVM.StaffList.Add(new Staff()
                {
                    Id = newStaffId,
                    Name = user,
                    Position = Enum.GetName(typeof(Access), position),
                    JoinedDate = DateTime.Now,
                    LeaveDate = new DateTime(9999, 12, 31, 6, 6, 6)
                });
                Mains.LoginVM.StaffDictionary.Add(pw, newStaffId);
            }
            else
            {
                Mains.AdminVM.AddStaffStatusMessage = "Duplicate data";
            }
            usernameBox.Clear();
            passwordBox.Clear();
            Keyboard.ClearFocus();
        }

        private void MenuChoiceTabChanged(object sender, SelectionChangedEventArgs e)
        {
            bool eventFiredByMenuTab = ReferenceEquals(e.OriginalSource, this.menuTab);
            if(eventFiredByMenuTab == false)
            {
                return;
            }

            int index = ((TabControl)sender).SelectedIndex;
            switch(index)
            {
                case 0:
                    Mains.AdminVM.ProductList = Mains.OrderVM.Appetizers;
                    break;

                case 1:
                    Mains.AdminVM.ProductList = Mains.OrderVM.MainDishes;
                    break;

                case 2:
                    Mains.AdminVM.ProductList = Mains.OrderVM.Beverages;
                    break;

                case 3:
                    Mains.AdminVM.ProductList = Mains.OrderVM.Desserts;
                    break;

                case 4:
                    Mains.AdminVM.ProductList = Mains.OrderVM.Others;
                    break;
            }

            // Prevents SelectionChangedEvent routed event from bubbling up to SideTab
            e.Handled = true;
        }

        private void SideTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool eventFiredBySideTab = ReferenceEquals(e.OriginalSource, this.sideTab);
            if(eventFiredBySideTab == false)
            {
                return;
            }

            int index = ((TabControl)sender).SelectedIndex;
            switch (index)
            {
                case 0:
                    Keyboard.ClearFocus();
                    break;

                case 4:
                    var window = new TablesOverview();
                    window.Show();
                    this.Close();
                    break;
            }
      
        }
    }
}
