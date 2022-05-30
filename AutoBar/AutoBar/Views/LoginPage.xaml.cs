using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LoginViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}