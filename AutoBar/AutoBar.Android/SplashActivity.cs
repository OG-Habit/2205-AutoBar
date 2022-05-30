using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AutoBar;

using Xamarin.Forms;
using AndroidX.AppCompat.App;
using AutoBar.Droid;

[Activity(Label = "AutoBar", Icon = "@mipmap/ic_launcher", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
public class SplashActivity : AppCompatActivity
{
    protected override void OnResume()
    {
        base.OnResume();
        StartActivity(typeof(MainActivity));
    }
}