using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class Order
    {
        public string Id { get; set; }  
        public string CustomerId { get; set; }
        public string OrderStatus { get; set; }
        public bool HasReward { get; set; }
        public double TotalPrice { get; set; }
        public double PointsEarned { get; set; }
        public string OpenedOn { get; set; }
        public string ClosedOn { get; set; }

        
        public string CustomerName { get; set; }

    }
}
