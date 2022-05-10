using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ABartenderEditPage : ContentPage
    {
        public ABartenderEditPage()
        {
            InitializeComponent();

            BindingContext = new ABartenderDetailViewModel();
        }
    }
}