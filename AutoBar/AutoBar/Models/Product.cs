using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBar.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public decimal Price { get; set; }
    }
}
