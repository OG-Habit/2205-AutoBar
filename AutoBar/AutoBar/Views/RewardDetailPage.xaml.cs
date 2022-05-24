using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RewardDetailPage : ContentPage
    {
        public RewardDetailPage()
        {
            InitializeComponent();
            BindingContext = new RewardDetailViewModel();
        }
    }
}