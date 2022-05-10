using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Birthday { get; set; }
        public string Sex { get; set; }
        public string CardIssued { get; set; }
        public double CurrentBalance { get; set; }
        public string TotalPoints { get; set; }
    }
}
