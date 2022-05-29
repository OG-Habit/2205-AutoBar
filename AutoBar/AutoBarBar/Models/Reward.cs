namespace AutoBarBar.Models
{
    public class Reward
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public decimal Points { get; set; }

        public string NamePoints
        {
            get
            {
                if(string.Equals(Name, "-- None --"))
                {
                    return Name;
                }
                else if (Points == 0)
                {
                    return Name + " - Free";
                }
                else
                {
                    return Name + " - " + Points;
                }
            }
        }
    }
}
