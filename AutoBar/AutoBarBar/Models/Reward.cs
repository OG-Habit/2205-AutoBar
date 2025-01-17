﻿namespace AutoBarBar.Models
{
    public class Reward
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public decimal Points { get; set; }
        public int ClaimFrequencyToday { get; set; } //bar admin
        public int ClaimFrequencyPast7Days { get; set; } //bar admin
        public int ClaimFrequencyOverall { get; set; } //bar admin

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
