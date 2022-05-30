using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AutoBarBar;

using Xamarin.Forms;
using AndroidX.AppCompat.App;
using AutoBarBar.Droid;

[Activity(Label = "AutoBarBar", Icon = "@mipmap/ic_launcher", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnResume()
    {
        base.OnResume();
        StartActivity(typeof(MainActivity));
    }
}