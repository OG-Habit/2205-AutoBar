using AutoBarBar.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class LoginViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one

            await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "1");
            await Shell.Current.GoToAsync($"//{nameof(AHomePage)}");

            // Uncomment Lines below and Comment Lines 22 and 23 to access the Bartender TabBar
            /*
            await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "2");
            await Shell.Current.GoToAsync($"//{nameof(BartenderHomePage)}");
            */
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
                await Shell.Current.GoToAsync($"//{nameof(AHomePage)}");
            }
            else if (isLoogged == "2")
            {
                await Shell.Current.GoToAsync($"//{nameof(ProfilePage)}");
            }
        }
    }
}
