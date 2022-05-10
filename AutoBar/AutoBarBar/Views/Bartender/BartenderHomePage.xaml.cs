using AutoBarBar.Popups;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBarBar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BartenderHomePage : ContentPage
    {
        public BartenderHomePage()
        {
            InitializeComponent();
        }

        private void ShowReloadBalancePopup(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new ReloadBalancePopup());
        }
    }
}