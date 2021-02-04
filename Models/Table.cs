using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlamyPOS.Models
{
    public class Table : ObservableObject
    {
        public Table()
        {
            Id = 0;
        }

        private int _tableId;
        public int Id
        {
            get => _tableId;
            set { _tableId = value; OnPropertyChanged(); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private Guid _uuid;
        public Guid UUID
        {
            get => _uuid;
            set { _uuid = value; OnPropertyChanged(); }
        }

        private double _coordsX;
        public double CoordsX
        {
            get => _coordsX;
            set { _coordsX = value; OnPropertyChanged(); }
        }

        private double _coordsY;
        public double CoordsY
        {
            get => _coordsY;
            set { _coordsY = value; OnPropertyChanged(); }
        }

        private bool _isTaken;
        public bool IsTaken
        {
            get => _isTaken;
            set { _isTaken = value; OnPropertyChanged(); }
        }
    }
}
