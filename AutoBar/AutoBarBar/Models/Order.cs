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
        public DateTime OpenedOn { get; set; }
        public DateTime ClosedOn { get; set; }


        public string CustomerName { get; set; }
        public string BartenderName { get; set; }
        public string Reward{ get; set; }
    }
}
