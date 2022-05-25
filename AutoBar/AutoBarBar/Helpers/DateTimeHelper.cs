using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar
{
    public static class DateTimeHelper
    {
        public static string GetPHTime()
        {
            return DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
