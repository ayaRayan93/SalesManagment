using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSystem
{
    public class Bill_Items
    {
        public string Code { get; set; }
        public string Product_Type { get; set; }
        public string Product_Name { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Sort { get; set; }
        public double Quantity { get; set; }
        public double Cost { get; set; }
        public double Total_Cost { get; set; }
        public string Store_Name { get; set; }
    }
}
