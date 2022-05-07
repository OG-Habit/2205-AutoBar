using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ABartenderPage : ContentPage
    {
        ABartenderViewModel _viewModel;

        public ABartenderPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ABartenderViewModel();
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