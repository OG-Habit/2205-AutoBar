using System;
using System.Collections.Generic;
using System.Text;
using AutoBar.Models;

namespace AutoBar.Models
{
    public class CustomerForAdmin
    {
        public int ID { get; set; }
        public string QRKey { get; set; }
        public User UserDetails { get; set; }
        public decimal Balance { get; set; }
        public decimal Points { get; set; }
    }
}
