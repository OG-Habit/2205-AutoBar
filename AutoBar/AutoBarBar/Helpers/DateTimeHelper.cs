using System;
using System.Collections.Generic;
using System.Text;
using static AutoBarBar.Constants;

namespace AutoBarBar
{
    public static class DateTimeHelper
    {
        public static string GetPHTimeForDB()
        {
            return DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetPHTimeForUI()
        {
            return DateTime.UtcNow.AddHours(8).ToString();
        }

        public static Dictionary<string, string> GetPHTimeForBoth()
        {
            DateTime dt = DateTime.UtcNow.AddHours(8);
            return new Dictionary<string, string>() 
            {
                {KEY_DB, dt.ToString("yyyy-MM-dd HH:mm:ss")},
                {KEY_UI, dt.ToString()}
            };
        }
    }
}
