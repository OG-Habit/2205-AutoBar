using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class User
    {
        public int ID { get; set; }
        public int UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageLink { get; set; }
        public string CreatedOn { get; set; }
        public int IsDeleted { get; set; }

        public string FullName { get; set; }
    }
}
