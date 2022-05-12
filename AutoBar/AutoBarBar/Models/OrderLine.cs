using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class OrderLine
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; } 
        public string CreatedOn { get; set; }

        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public double SubTotal { get; set; }
        public string ProductImgUrl { get; set; }
    }
}
