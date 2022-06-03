using AutoBar.Models;
using AutoBar.Services;
using AutoBar.Views;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public Command LogoutCommand { get; }
        public Command EwalletClicked { get; }
        public Command QRCodeClicked { get; }
        public Command ReportClicked { get; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageLink { get; }
        public Boolean isLostCard { get; set; }

        ILostCardService lostCardService;
        IToastService toastService;

        string _m1;
        string _m2;
        public string ThirdLinkMessage1
        {
            get => _m1;
            set => SetProperty(ref _m1, value);
        }

        public string ThirdLinkMessage2
        {
            get => _m2;
            set => SetProperty(ref _m2, value);
        }


        public ProfileViewModel()
        {
            lostCardService = DependencyService.Get<ILostCardService>();
            toastService = DependencyService.Get<IToastService>();
            LogoutCommand = new Command(OnLogoutClicked);
            EwalletClicked = new Command(OnEwalletClicked);
            QRCodeClicked = new Command(OnQRCodeClicked);
            ReportClicked = new Command(OnReportClicked);
            ImageLink = "default_pic";
            isLostCard = false;
            SetUserDetails();
            ToggleMessage();
        }

        private void ToggleMessage()
        {
            switch (isLostCard)
            {
                case false:
                    ThirdLinkMessage1 = "Report Lost Card";
                    ThirdLinkMessage2 = "Send a report of a lost card";
                    break;
                case true:
                    ThirdLinkMessage1 = "Request New Card";
                    ThirdLinkMessage2 = "Send a request for a new card";
                    break;
            }
        }
        async void SetUserDetails()
        {
            var UserString = await Xamarin.Essentials.SecureStorage.GetAsync("user");
            Customer u = JsonConvert.DeserializeObject<Customer>(UserString);
            Name = u.UserDetails.FullName;
            Email = u.UserDetails.Email;
        }
        private async void OnLogoutClicked(object obj)
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Log out", "Would you like to log out?", "Yes", "No");
            if (retryBool)
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Xamarin.Essentials.SecureStorage.SetAsync("isLogged", "0");
                await Shell.Current.GoToAsync($"//LoginPage");
            }
        }

        private async void OnEwalletClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(EwalletPage)}");
        }

        private async void OnQRCodeClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(QRCodePage)}");
        }

        private async void OnReportClicked(object obj)
        {
            if (isLostCard == false)
            {
                bool retryBool = await App.Current.MainPage.DisplayAlert("Lost Card", "Report and deactivate your lost card?", "Yes", "No");
                if (retryBool)
                {
                    await lostCardService.ReportLostCard();
                    isLostCard = true;
                    toastService.ShowLongMessage("Lost Card Reported and Deactivated!");
                    ToggleMessage();
                }
            }
            else
            {
                bool retriBool = await App.Current.MainPage.DisplayAlert("New Card", "Request for a new card?", "Yes", "No");
                if (retriBool)
                {
                    await lostCardService.RequestNewCard();
                    isLostCard = false;
                    toastService.ShowLongMessage("New Card Requested!");
                    ToggleMessage();
                }
            }
        }
    }
}
