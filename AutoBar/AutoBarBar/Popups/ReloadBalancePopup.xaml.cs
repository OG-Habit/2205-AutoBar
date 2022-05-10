using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoBarBar.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReloadBalancePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ReloadBalancePopup()
        {
            InitializeComponent();
        }

        private async void ClosePopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}