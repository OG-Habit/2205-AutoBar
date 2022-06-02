namespace AutoBarBar.Models
{
    public class Revenue : BaseModel
    {
        public int TotalOrders { get; set; }
        public int TotalWeekOrders { get; set; }

        decimal _totalRevenue;
        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        decimal _totalWeekRevenue;
        public decimal TotalWeekRevenue
        {
            get => _totalWeekRevenue;
            set => SetProperty(ref _totalWeekRevenue, value);
        }
    }
}
