using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
        }
    }
}