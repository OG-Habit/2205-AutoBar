using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AutoBarBar.Helpers
{
    public class CardStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ans;
            switch((int)value)
            {
                case 0:
                    ans = "Not Issued";
                    break;
                case 1:
                    ans = "Not Claimed";
                    break;
                case 2:
                    ans = "Issued";
                    break;
                case 3:
                    ans = "Missing";
                    break;
                default:
                    ans = "Error";
                    break;  
            }
            return ans;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
