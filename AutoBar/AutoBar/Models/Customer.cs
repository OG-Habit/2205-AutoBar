using System;
using System.Collections.Generic;
using System.Text;
using AutoBar.Models;

namespace AutoBar.Models
{
    public class Customer
    {
        public int ID { get; set; }
        public string QRKey { get; set; }
        public int CardStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal Points { get; set; }
        public User UserDetails { get; set; }
    }
}
