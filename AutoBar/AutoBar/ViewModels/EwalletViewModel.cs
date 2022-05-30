using Xamarin.Forms;
using System;

namespace AutoBar.ViewModels
{
    public class EwalletViewModel
    {
        public double Balance { get; set; }
        public string ImageLink { get; }

        public EwalletViewModel()
        {
            SetBalance();
            ImageLink = "default_pic";
        }
        private async void SetBalance()
        {
            string BalString = await Xamarin.Essentials.SecureStorage.GetAsync("balance");
            double Bal = Convert.ToDouble(BalString);
            Balance = Bal;
        }
    }
}
