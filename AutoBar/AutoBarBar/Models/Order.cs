using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class Order 
    {
        public string Id { get; set; }  
        public string CustomerId { get; set; }
        public bool OrderStatus { get; set; }
        public bool HasReward { get; set; }
        public double TotalPrice { get; set; }
        public int PointsEarned { get; set; }
        public string OpenedOn { get; set; }
        public string ClosedOn { get; set; }

        
        public string CustomerName { get; set; }

    }
}
