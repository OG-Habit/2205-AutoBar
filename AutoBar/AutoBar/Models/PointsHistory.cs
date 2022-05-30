using System;
using System.Collections.Generic;
using System.Text;
using AutoBar.Models;

namespace AutoBar.Models
{
    public class PointsHistory
    {
        public DateTime TimeStamp { get; set; }
        public string Type { get; set; }
        public decimal Points { get; set; }

    }
}
