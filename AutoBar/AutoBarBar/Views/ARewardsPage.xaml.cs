using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ARewardsPage : ContentPage
    {
        ARewardsViewModel _viewModel;

        public ARewardsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ARewardsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        void SearchBarChange(object sender, TextChangedEventArgs e)
        {
            _viewModel.SearchBar_Change(sender, e);
        }
    }
}