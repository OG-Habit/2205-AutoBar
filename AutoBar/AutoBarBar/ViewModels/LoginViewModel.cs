using AutoBarBar.Models;
using AutoBarBar.Services;
using AutoBarBar.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using static AutoBarBar.Constants;

namespace AutoBarBar.ViewModels
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
            if(obj == null)
            {
                await App.Current.MainPage.DisplayAlert("Missing fields", "Please enter the required fields.", "Try again.");
                return;
            }

            var list = (List<string>)obj;
            User u = await accountService.LoginUser(list[0], list[1]);
            string userObj = JsonConvert.SerializeObject(u);

            if(u.UserType == 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Email and password combination is invalid.", "Try again.");
            }
            else if(u.UserType == 1)
            {
                await Xamarin.Essentials.SecureStorage.SetAsync($"{KEY_ISLOGGED}", "1");
                await Xamarin.Essentials.SecureStorage.SetAsync($"{PARAM_USER}", $"{userObj}");
                await Shell.Current.GoToAsync($"//{nameof(AHomePage)}");
            }
            else // u.UserType == 2
            {
                await Xamarin.Essentials.SecureStorage.SetAsync($"{KEY_ISLOGGED}", "2");
                await Xamarin.Essentials.SecureStorage.SetAsync($"{PARAM_USER}", $"{userObj}");
                await Shell.Current.GoToAsync($"//{nameof(BartenderHomePage)}?{PARAM_USER}={userObj}");
            }

            //accountService.LoginUser();
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one

            //await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            //await Shell.Current.GoToAsync($"//{nameof(AHomePage)}");

            // Uncomment Lines below and Comment Lines 22 and 23 to access the Bartender TabBar

            //await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "2");
            //await Shell.Current.GoToAsync($"//{nameof(BartenderHomePage)}");
        }


        public void OnAppearing()
        {
            _ = CheckLogin();
        }

        private async Task CheckLogin()
        {
            var isLoogged = Xamarin.Essentials.SecureStorage.GetAsync($"{KEY_ISLOGGED}").Result;
            var user = Xamarin.Essentials.SecureStorage.GetAsync($"{PARAM_USER}").Result;
            if (isLoogged == "1")
            {
                await Shell.Current.GoToAsync($"//{nameof(AHomePage)}");
            }
            else if (isLoogged == "2")
            {
                await Shell.Current.GoToAsync($"//{nameof(BartenderHomePage)}?{PARAM_USER}={user}");
            }
        }
    }
}
