using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class Order 
    {
        public int ID { get; set; }  
        public int CustomerID { get; set; }
        public int OrderStatus { get; set; }
        public double TotalPrice { get; set; }
        public decimal PointsEarned { get; set; }
        public int HasReward { get; set; }
        public string OpenedOn { get; set; }
        public string ClosedOn { get; set; }
        public string Remarks { get; set; }


        public string CustomerName { get; set; }
        public string BartenderName { get; set; }
        public string Reward{ get; set; }
    }
}
