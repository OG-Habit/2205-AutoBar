using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ABartenderAddPage : ContentPage
    {
        public ABartenderAddPage()
        {
            InitializeComponent();

            BindingContext = new ABartenderAddViewModel();
        }
    }
}