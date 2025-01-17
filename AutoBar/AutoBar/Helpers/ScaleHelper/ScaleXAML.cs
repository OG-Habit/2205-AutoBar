﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AutoBar
{
    public class ScaleXAML : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter.ToString().Contains("="))
            {
                var paramType = parameter.ToString().Remove(parameter.ToString().IndexOf("=", StringComparison.CurrentCulture));
                var param = parameter.ToString().Remove(0, paramType.Length + 1);

                if(param.Contains("|"))
                {
                    string [] paramPlatform = param.Split('|');
                    param = Device.RuntimePlatform == Device.Android ? paramPlatform[0] : paramPlatform[1];
                }

                double convertedValue;

                if(paramType.Contains("height"))
                {
                    if (double.TryParse(param, NumberStyles.Number, CultureInfo.InvariantCulture, out convertedValue))
                    {
                        if(paramType.Contains("Int"))
                            return (int)convertedValue.ScaleHeight();
                        else
                            return convertedValue.ScaleHeight();
                    }
                }
                else if(paramType.Contains("width"))
                {
                    if (double.TryParse(param, NumberStyles.Number, CultureInfo.InvariantCulture, out convertedValue))
                    {
                        if (paramType.Contains("Int"))
                            return (int)convertedValue.ScaleWidth();
                        else
                            return convertedValue.ScaleWidth();
                    }
                }
                else if(paramType.Contains("thickness"))
                {
                    return ConverToThicknessProperty(param);
                }
                else if(paramType.Contains("fontSize"))
                {
                    if(double.TryParse(param, NumberStyles.Number, CultureInfo.InvariantCulture, out convertedValue))
                        return convertedValue.ScaleHeight() - (Device.RuntimePlatform == Device.iOS ? 0.5 : 0);
                }

                throw new InvalidOperationException($"[{parameter.ToString()}] is an Invalid parameters. Supported parameters are height, width, thickness, fontSize, absolute-WH, absolute-NONE");
            }

            if(parameter.ToString().Contains(",") == true)
            {
                return ConverToThicknessProperty(parameter.ToString());
            }

            return (double.Parse(parameter.ToString()) * (App.ScreenHeight / 568.0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        object ConverToThicknessProperty(string param)
        {
            double l, t, r, b;
            string[] thickness = param.Split(',');

            if(thickness.Length == 4)
            {
                if(double.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out l) &&
                    double.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out t) &&
                    double.TryParse(thickness[2], NumberStyles.Number, CultureInfo.InvariantCulture, out r) &&
                    double.TryParse(thickness[3], NumberStyles.Number, CultureInfo.InvariantCulture, out b))
                {
                    return new Thickness(l * (App.ScreenWidth / 320.0), t * (App.ScreenHeight / 568.0), r * (App.ScreenWidth / 320.0), b * (App.ScreenHeight / 568.0));
                }   
            } else if (thickness.Length == 2)
            {
                if(double.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out l) &&
                    double.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out t))
                {
                    return new Thickness(l * (App.ScreenWidth / 320.0), t * (App.ScreenHeight / 568.0));
                }
            }
            throw new InvalidOperationException("Cannot convert thickness");
        }

        object ConvertToPlatformThicknessProperty(string param)
        {
            string[] paramPlatform = param.Split('|');
            double l, t, r, b; 
            string[] thickness = Device.RuntimePlatform == Device.Android ? paramPlatform[0].Split(',') : paramPlatform[1].Split(',');

            if(thickness.Length == 4)
            {
                if(double.TryParse(thickness[0], NumberStyles.Number, CultureInfo.InvariantCulture, out l) &&
                    double.TryParse(thickness[1], NumberStyles.Number, CultureInfo.InvariantCulture, out t) &&
                    double.TryParse(thickness[2], NumberStyles.Number, CultureInfo.InvariantCulture, out r) &&
                    double.TryParse(thickness[3], NumberStyles.Number, CultureInfo.InvariantCulture, out b))
                {
                    return new Thickness(l * (App.ScreenWidth / 320.0), t * (App.ScreenHeight / 568.0), r * (App.ScreenWidth / 320.0), b * (App.ScreenHeight / 568.0));
                }
            }

            throw new InvalidOperationException("Cannot convert platform-thickness");
        }
    }
}
