using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBar.Models
{
    public class Order 
    {
        public int Id { get; set; }  
        public int CustomerId { get; set; }
        public int OrderStatus { get; set; }
        public bool HasReward { get; set; }
        public double TotalPrice { get; set; }
        public double PointsEarned { get; set; }
        public DateTime OpenedOn { get; set; }
        public DateTime ClosedOn { get; set; }
        public string CustomerName { get; set; }
        public string BartenderName { get; set; }
        public string Reward{ get; set; }
    }
}
