using System;

namespace AutoBar.Models
{
    public class OrderLine : BaseModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        int quantity;
        public int Quantity 
        { 
            get => quantity; 
            set => SetProperty(ref quantity, value);
        } 
        public DateTime CreatedOn { get; set; }
        public string CustomerName { get; set; }

        //supposed to be in naa pay bartender here pero will adjust soon na lang
        public string ProductName { get; set; }
        decimal subTotal;
        public decimal SubTotal 
        {
            get => subTotal;
            set => SetProperty(ref subTotal, value);
        }
        public string ProductImgUrl { get; set; }
    }
}
