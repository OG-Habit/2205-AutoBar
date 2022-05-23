using System;

namespace AutoBarBar.Models
{
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public DateTime CardIssued { get; set; }
        public double CurrentBalance { get; set; }
        public string TotalPoints { get; set; }
        public string ImageLink { get; set; }
        public string Status { get; set; }
    }
}
