using AutoBarBar.ViewModels;
using AutoBarBar.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AutoBarBar
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(AOrderDetailPage), typeof(AOrderDetailPage));
            Routing.RegisterRoute(nameof(ABartenderDetailPage), typeof(ABartenderDetailPage));
            Routing.RegisterRoute(nameof(ABartenderEditPage), typeof(ABartenderEditPage));
            Routing.RegisterRoute(nameof(AMenuDetailPage), typeof(AMenuDetailPage));
            Routing.RegisterRoute(nameof(AMenuEditPage), typeof(AMenuEditPage));
            Routing.RegisterRoute(nameof(ARewardsDetailPage), typeof(ARewardsDetailPage));
            Routing.RegisterRoute(nameof(ARewardsEditPage), typeof(ARewardsEditPage));
        }

    }
}
