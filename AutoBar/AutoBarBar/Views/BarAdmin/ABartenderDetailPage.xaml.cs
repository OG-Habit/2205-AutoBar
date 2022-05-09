using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ABartenderDetailPage : ContentPage
    {
        public ABartenderDetailPage()
        {
            InitializeComponent();

            BindingContext = new ABartenderDetailViewModel();
        }
    }
}