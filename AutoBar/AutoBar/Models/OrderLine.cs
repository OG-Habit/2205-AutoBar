using System;

namespace AutoBar.Models
{
    public class OrderLine : BaseModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        int quantity;
        public int Quantity 
        { 
            get => quantity; 
            set => SetProperty(ref quantity, value);
        } 
        public DateTime CreatedOn { get; set; }
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
