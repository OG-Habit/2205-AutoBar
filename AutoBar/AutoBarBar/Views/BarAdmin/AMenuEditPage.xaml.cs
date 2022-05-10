using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AMenuEditPage : ContentPage
    {
        public AMenuEditPage()
        {
            InitializeComponent();

            BindingContext = new AMenuDetailViewModel();
        }
    }
}