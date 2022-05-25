using System;

namespace AutoBarBar.Models
{
    public class OrderLine : BaseModel
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        int quantity;
        public int Quantity 
        { 
            get => quantity; 
            set => SetProperty(ref quantity, value);
        } 
        public int CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public int IsCompleted { get; set; }


        public string TempID { get; set; }
        public string CustomerName { get; set; }
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
