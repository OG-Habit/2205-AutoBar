using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Models
{
    public class OrderLine : BaseModel
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public double Price { get; set; }
        int quantity;
        public int Quantity 
        { 
            get => quantity; 
            set => SetProperty(ref quantity, value);
        } 
        public string CreatedOn { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        double subTotal;
        public double SubTotal 
        {
            get => subTotal;
            set => SetProperty(ref subTotal, value);
        }
        public string ProductImgUrl { get; set; }
    }
}
