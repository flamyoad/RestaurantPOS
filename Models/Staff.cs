using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlamyPOS.Models
{
    public class Staff : ObservableObject
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private DateTime _joinedDate;
        public DateTime JoinedDate
        {
            get => _joinedDate;
            set { _joinedDate = value; OnPropertyChanged(); }
        }

        private DateTime _leaveDate;
        public DateTime LeaveDate
        {
            get => _leaveDate;
            set { _leaveDate = value; OnPropertyChanged(); }
        }

        private string _position;
        public string Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }

        public bool IsAdmin
        {
            get => Position == "Supervisor";
        }
    }
}
