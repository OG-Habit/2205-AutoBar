using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class Order : BaseModel
    {
        public int ID { get; set; }  
        public int CustomerID { get; set; }
        public int OrderStatus { get; set; }

        double _totalPrice;
        public double TotalPrice {
            get => _totalPrice;
            set => SetProperty(ref _totalPrice, value);
        }

        decimal _pointsEarned;
        public decimal PointsEarned {
            get => _pointsEarned;
            set => SetProperty(ref _pointsEarned, value);
        }

        public int HasReward { get; set; }
        public string OpenedOn { get; set; }
        public string ClosedOn { get; set; }
        public string Remarks { get; set; }


        public string CustomerName { get; set; }
        public string BartenderName { get; set; }
        public string Reward{ get; set; }
    }
}
