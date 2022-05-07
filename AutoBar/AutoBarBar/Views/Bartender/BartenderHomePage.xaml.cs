using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBarBar.Views.Bartender
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BartenderHomePage : ContentPage
    {
        public BartenderHomePage()
        {
            InitializeComponent();
        }

        private async void ShowScanUI(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ScanPage)}");
        }
    }
}