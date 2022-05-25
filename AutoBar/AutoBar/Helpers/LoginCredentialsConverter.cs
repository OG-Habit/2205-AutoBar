using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AutoBar.Helpers
{
    public class LoginCredentialsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length ==0 || values[0] == null || values[1] == null)
            {
                return null;
            }

            string email = (string)values[0];
            string password = (string)values[1];

            return new List<string> { email, password };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
