using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AutoBar.Droid.Services;
using AutoBar.Services;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(ToastService))]
namespace AutoBar.Droid.Services
{
    class ToastService : IToastService
    {
        public void ShowLongMessage(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }

        public void ShowShortMessage(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}