using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLLoader
{    class Order
    {
        public int No { get; set; }
        public DateTime RegDate { get; set; }
        public double Sum { get; set; }
        public List<Product> Products { get; set; }
        public User User { get; set; }
    }
}
