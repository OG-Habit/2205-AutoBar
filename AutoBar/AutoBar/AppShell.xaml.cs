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
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
