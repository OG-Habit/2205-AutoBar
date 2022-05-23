using AutoBarBar.ViewModels;
using Xamarin.Forms;

namespace AutoBarBar.Views
{
    public partial class ARewardsAddPage : ContentPage
    {
        public ARewardsAddPage()
        {
            InitializeComponent();

            BindingContext = new ARewardsAddViewModel();
        }
    }
}