using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlamyPOS.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int StaffID { get; set; }
        public int TableID { get; set; }
        public DateTime OrderDateTime { get; set; }

        public double BillTotal { get; set; }
        public bool IsSettled { get; set; }
    }
}
