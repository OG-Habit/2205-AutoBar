using AutoBar.Models;
using AutoBar.Services;
using AutoBar.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    public class LoginViewModel 
    {
        IAccountService accountService;

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            accountService = DependencyService.Get<IAccountService>();

            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            if (obj == null)
            {
                await App.Current.MainPage.DisplayAlert("Missing fields", "Please enter the required fields.", "Try again.");
                return;
            }

            var list = (List<string>)obj;
            Customer u = await accountService.LoginCustomer(list[0], list[1]);
            string userObj = JsonConvert.SerializeObject(u);

            if(u.UserDetails == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email and password combination is invalid.", "Try again.");
                return;
            }


            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            await Xamarin.Essentials.SecureStorage.SetAsync("qr", $"{u.QRKey}");
            await Xamarin.Essentials.SecureStorage.SetAsync("balance", $"{u.Balance}");
            await Xamarin.Essentials.SecureStorage.SetAsync("points", $"{u.Points}");
            await Xamarin.Essentials.SecureStorage.SetAsync("cardStatus", $"{u.CardStatus}");
            await Xamarin.Essentials.SecureStorage.SetAsync("user", $"{userObj}");
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }

        public void OnAppearing()
        {
            _ = CheckLogin();
        }

        private async Task CheckLogin()
        {
            var isLoogged = Xamarin.Essentials.SecureStorage.GetAsync("isLogged").Result;
            if (isLoogged == "1")
            {
                var userObj = Xamarin.Essentials.SecureStorage.GetAsync("user").Result;
                var qr = Xamarin.Essentials.SecureStorage.GetAsync("qr").Result;
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}?user={userObj}&qr={qr}");
            }
        }
    }
}
