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

        //for jomer's module
        public int CardStatus { get; set; }
        public string LastTransactionAt { get; set; }
        public string QRKey { get; set; }

        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CardIssued { get; set; }
        public string Contact { get; set; }
        public double CurrentBalance { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string ImageLink { get; set; }
        
    }
}
