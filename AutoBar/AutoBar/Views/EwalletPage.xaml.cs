using AutoBar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EwalletPage : ContentPage
    {
        public EwalletPage()
        {
            InitializeComponent();
            BindingContext = new EwalletViewModel();
        }
    }
}