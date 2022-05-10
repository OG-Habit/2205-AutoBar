using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ARewardsEditPage : ContentPage
    {
        public ARewardsEditPage()
        {
            InitializeComponent();

            BindingContext = new ARewardsDetailViewModel();
        }
    }
}