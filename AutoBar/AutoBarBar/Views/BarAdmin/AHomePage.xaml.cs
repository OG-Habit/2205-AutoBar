using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AHomePage : ContentPage
    {
        AHomeViewModel _viewModel;

        public AHomePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new AHomeViewModel();
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