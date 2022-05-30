using Xamarin.Forms;
using Newtonsoft.Json;
using static AutoBarBar.Constants;
using AutoBarBar.Models;

namespace AutoBarBar.ViewModels
{
    public class ProfileViewModel
    {
        public Command LogoutCommand { get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageLink { get; }

        public ProfileViewModel()
        {
            LogoutCommand = new Command(OnLogoutClicked);
            SetProfile();
            ImageLink = "default_pic";
        }

        private async void OnLogoutClicked(object obj)
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Log out", "Would you like to log out?", "Yes", "No");
            if (retryBool)
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Xamarin.Essentials.SecureStorage.SetAsync($"{KEY_ISLOGGED}", "0");
                await Shell.Current.GoToAsync($"//LoginPage");
            }  
        }

        private async void SetProfile()
        {
            string UserString = await Xamarin.Essentials.SecureStorage.GetAsync($"{PARAM_USER}");
            User CurrentUser = JsonConvert.DeserializeObject<User>(UserString);
            Name = CurrentUser.FullName;
            Email = CurrentUser.Email;
        }
    }
}
