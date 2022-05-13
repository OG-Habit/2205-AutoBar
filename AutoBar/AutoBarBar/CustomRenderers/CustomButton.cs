using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoBarBar.CustomRenderers
{
    public class CustomButton : Button
    {
        public static readonly BindableProperty PaddingTopProperty = BindableProperty.Create(nameof(PaddingTopProperty), typeof(int), typeof(CustomButton), 0);
        public int PaddingTop
        {
            get => (int)GetValue(PaddingTopProperty);
            set => SetValue(PaddingTopProperty, value);
        }

        public static readonly BindableProperty PaddingLeftProperty = BindableProperty.Create(nameof(PaddingLeftProperty), typeof(int), typeof(CustomButton), 0);
        public int PaddingLeft
        {
            get => (int)GetValue(PaddingLeftProperty);
            set => SetValue(PaddingLeftProperty, value);
        }

        public static readonly BindableProperty PaddingRightProperty = BindableProperty.Create(nameof(PaddingRightProperty), typeof(int), typeof(CustomButton), 0);
        public int PaddingRight
        {
            get => (int)GetValue(PaddingRightProperty);
            set => SetValue(PaddingRightProperty, value);
        }

        public static readonly BindableProperty PaddingBottomProperty = BindableProperty.Create(nameof(PaddingBottomProperty), typeof(int), typeof(CustomButton), 0);
        public int PaddingBottom
        {
            get => (int)GetValue(PaddingBottomProperty);
            set => SetValue(PaddingBottomProperty, value);
        }
    }
}
