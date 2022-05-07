using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AMenuPage : ContentPage
    {
        AMenuViewModel _viewModel;

        public AMenuPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new AMenuViewModel();
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