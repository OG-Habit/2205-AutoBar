using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PastOrderDetailPage : ContentPage
    {
        PastOrderDetailViewModel _viewModel;
        public PastOrderDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new PastOrderDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}