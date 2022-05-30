using AutoBar.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        OrderViewModel _viewModel;

        public OrderPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new OrderViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _viewModel.TimeXaml = DateTime.UtcNow.AddHours(8);
                });
                return true;
            });
            _viewModel.OnAppearing();
        }
    }
}