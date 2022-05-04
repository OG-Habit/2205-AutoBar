using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class BartenderPage : ContentPage
    {
        BartenderViewModel _viewModel;

        public BartenderPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new BartenderViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}