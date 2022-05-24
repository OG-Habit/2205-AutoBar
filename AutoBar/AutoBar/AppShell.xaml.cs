using AutoBar.ViewModels;
using AutoBar.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AutoBar
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(QRCodePage), typeof(QRCodePage));
            Routing.RegisterRoute(nameof(EwalletPage), typeof(EwalletPage));
            Routing.RegisterRoute(nameof(ProductDetailPage), typeof(ProductDetailPage));
            Routing.RegisterRoute(nameof(RewardDetailPage), typeof(RewardDetailPage));
            Routing.RegisterRoute(nameof(OrderDetailPage), typeof(OrderDetailPage));
            Routing.RegisterRoute(nameof(PastOrderPage), typeof(PastOrderPage));
            Routing.RegisterRoute(nameof(PastOrderDetailPage), typeof(PastOrderDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
