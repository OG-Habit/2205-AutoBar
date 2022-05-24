using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PastOrderPage : ContentPage
    {
        PastOrderViewModel _viewModel;

        public PastOrderPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PastOrderViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}