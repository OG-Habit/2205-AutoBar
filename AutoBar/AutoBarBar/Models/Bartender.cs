using System;

namespace AutoBarBar.Models
{
    public class Bartender
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public DateTime Birthday { get; set; }
        public string Sex { get; set; }
        public string ImageLink { get; set; }
        public decimal RevenueGeneratedToday { get; set; } // bar admin
        public decimal RevenueGeneratedPast7Days { get; set; } // bar admin
        public decimal RevenueGeneratedOverall { get; set; } // bar admin
    }
}
