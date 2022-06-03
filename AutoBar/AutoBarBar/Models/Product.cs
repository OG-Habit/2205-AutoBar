namespace AutoBarBar.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public string ImageLink { get; set; }
        public int OrderFrequencyToday { get; set; } //bar admsin
        public int OrderFrequencyPast7Days { get; set; } //bar admin
        public int OrderFrequencyOverall { get; set; } //bar admin
    }
}
