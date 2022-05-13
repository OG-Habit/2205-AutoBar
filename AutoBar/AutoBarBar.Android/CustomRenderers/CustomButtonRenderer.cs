using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AutoBarBar.CustomRenderers;
using AutoBarBar.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace AutoBarBar.Droid.CustomRenderers
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);   

            if(Control != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Green);
                Control.SetPadding(0, 0, 0, 0);
            }
        }
    }
}