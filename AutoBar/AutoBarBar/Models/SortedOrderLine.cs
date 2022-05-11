using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class SortedOrderLine
    {
        public string Time { get; set; }
        public double Total { get; set; }
        public List<OrderLine> OrderLineList { get; set; }
    }
}
