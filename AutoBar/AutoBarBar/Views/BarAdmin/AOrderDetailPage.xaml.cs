using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AOrderDetailPage : ContentPage
    {
        public AOrderDetailPage()
        {
            InitializeComponent();
            BindingContext = new AOrderDetailViewModel();
        }
    }
}