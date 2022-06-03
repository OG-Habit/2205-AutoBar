namespace AutoBarBar.Models
{
    public class Revenue : BaseModel
    {
        public int TotalOrders { get; set; }
        public int TotalWeekOrders { get; set; }

        double _totalRevenue;
        public double TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        double _totalWeekRevenue;
        public double TotalWeekRevenue
        {
            get => _totalWeekRevenue;
            set => SetProperty(ref _totalWeekRevenue, value);
        }
    }
}
