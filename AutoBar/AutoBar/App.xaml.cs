using AutoBar.Helpers;
using AutoBar.Services;
using AutoBar.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar
{
    public partial class App : Application
    {
        public static float ScreenWidth { get; set; }
        public static float ScreenHeight { get; set; }
        public static float AppScale { get; set; }

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
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
