using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBar.Models
{
    public class TransactionHistory
    {
        public string TimeStamp { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}
