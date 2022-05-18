using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class AMenuAddPage : ContentPage
    {
        public AMenuAddPage()
        {
            InitializeComponent();

            BindingContext = new AMenuAddViewModel();
        }
    }
}