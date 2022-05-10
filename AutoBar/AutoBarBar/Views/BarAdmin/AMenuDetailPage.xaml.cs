using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AMenuDetailPage : ContentPage
    {
        public AMenuDetailPage()
        {
            InitializeComponent();

            BindingContext = new AMenuDetailViewModel();
        }
    }
}