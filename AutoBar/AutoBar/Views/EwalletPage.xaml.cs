using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EwalletPage : ContentPage
    {
        EwalletViewModel _viewModel;

        public EwalletPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new EwalletViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}