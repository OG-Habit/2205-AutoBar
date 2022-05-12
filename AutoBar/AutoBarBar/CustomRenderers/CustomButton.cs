using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoBarBar.CustomRenderers
{
    public class CustomButton : Button
    {
        public static readonly BindableProperty PaddingTopProperty = BindableProperty.Create(nameof(PaddingTopProperty), typeof(float), typeof(CustomButton), 0);
        public float PaddingTop
        {
            get => (float)GetValue(PaddingTopProperty);
            set => SetValue(PaddingTopProperty, value);
        }

        public static readonly BindableProperty PaddingLeftProperty = BindableProperty.Create(nameof(PaddingLeftProperty), typeof(float), typeof(CustomButton), 0);
        public float PaddingLeft
        {
            get => (float)GetValue(PaddingLeftProperty);
            set => SetValue(PaddingLeftProperty, value);
        }

        public static readonly BindableProperty PaddingRightProperty = BindableProperty.Create(nameof(PaddingRightProperty), typeof(float), typeof(CustomButton), 0);
        public float PaddingRight
        {
            get => (float)GetValue(PaddingRightProperty);
            set => SetValue(PaddingRightProperty, value);
        }

        public static readonly BindableProperty PaddingBottomProperty = BindableProperty.Create(nameof(PaddingBottomProperty), typeof(float), typeof(CustomButton), 0);
        public float PaddingBottom
        {
            get => (float)GetValue(PaddingBottomProperty);
            set => SetValue(PaddingBottomProperty, value);
        }
    }
}
