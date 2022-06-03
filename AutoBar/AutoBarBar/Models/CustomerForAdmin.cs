using System;

namespace AutoBarBar.Models
{
    public class CustomerForAdmin : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime CardIssued { get; set; }
        public string Contact { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public decimal TotalPoints { get; set; }
        public string ImageLink { get; set; }
        public string Status { get; set; }
    }

}
