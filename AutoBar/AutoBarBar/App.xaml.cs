using AutoBarBar.Services;
using AutoBarBar.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBarBar
{
    public partial class App : Application
    {
        public static float ScreenWidth { get; set; }
        public static float ScreenHeight { get; set; }
        public static float AppScale { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
