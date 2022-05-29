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
                if(Points == 0)
                {
                    return Name + " - Free";
                }
                else if(Points == -1)
                {
                    return Name;
                } else
                {
                    return Name + " - " + Points;
                }
            }
        }
    }
}
