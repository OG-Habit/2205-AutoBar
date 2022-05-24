using System;

namespace AutoBarBar.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal Balance { get; set; }
        public decimal Points { get; set; }
        public int CardStatus { get; set; }
        public string LastTransactionAt { get; set; }
        public string QRKey { get; set; }
    }
}
