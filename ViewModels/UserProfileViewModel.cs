using FlamyPOS.Commands;
using FlamyPOS.Models;
using FlamyPOS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlamyPOS.ViewModels
{
    public class UserProfileViewModel : ObservableObject
    {
        public Staff CurrentUser { get; set; }

        public ICommand ReturnCommand { get; set; }

        public UserProfileViewModel()
        {
            ReturnCommand = new RelayCommand(Return);
        }

        private void Return()
        {
            Helpers.SwitchWindow(this, new TablesOverview());
        }
    }
}
