using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class RewardsPage : ContentPage
    {
        RewardsViewModel _viewModel;

        public RewardsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RewardsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}