using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AOrderDetailPage : ContentPage
    {
        AOrderDetailViewModel _viewModel;

        public AOrderDetailPage()
        {
            InitializeComponent();
            
            BindingContext = _viewModel = new AOrderDetailViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}