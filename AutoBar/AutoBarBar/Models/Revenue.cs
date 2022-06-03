namespace AutoBarBar.Models
{
    public class Revenue : BaseModel
    {
        int _totalOrders;
        public int TotalOrders 
        {
            get => _totalOrders;
            set => SetProperty(ref _totalOrders, value);
        }

        int _totalWeekOrders;
        public int TotalWeekOrders 
        {
            get => _totalWeekOrders;
            set => SetProperty(ref _totalWeekOrders, value);
        }

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
