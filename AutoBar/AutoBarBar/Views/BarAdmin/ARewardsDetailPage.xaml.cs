using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ARewardsDetailPage : ContentPage
    {
        public ARewardsDetailPage()
        {
            InitializeComponent();

            BindingContext = new ARewardsDetailViewModel();
        }
    }
}