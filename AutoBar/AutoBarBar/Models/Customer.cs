using System;

namespace AutoBarBar.Models
{
    public class Customer : BaseModel
    { 
        public int ID { get; set; }
        public int UserID { get; set; }

        decimal _balance;
        public decimal Balance { 
            get => _balance; 
            set => SetProperty(ref _balance, value); 
        }

        decimal _points;
        public decimal Points {
            get => _points;
            set => SetProperty(ref _points, value);
        }

        public int CardStatus { get; set; }
        public string LastTransactionAt { get; set; }
        public string QRKey { get; set; }
    }
}
