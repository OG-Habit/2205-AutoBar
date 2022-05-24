using AutoBar.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBar.ViewModels
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
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
        }
    }
}
