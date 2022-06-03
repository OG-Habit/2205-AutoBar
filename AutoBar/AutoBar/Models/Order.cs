using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBar.Models
{
    public class Order 
    {
        public int Id { get; set; }  
        public int CustomerId { get; set; }
        public bool OrderStatus { get; set; }
        public bool HasReward { get; set; }
        public double TotalPrice { get; set; }
        public decimal PointsEarned { get; set; }
        public DateTime OpenedOn { get; set; }
        public DateTime ClosedOn { get; set; }
        public string CustomerName { get; set; }
        public string BartenderName { get; set; } //supposed to be in orderline pero will adjust soon na lang
        public string Reward{ get; set; }
    }
}
