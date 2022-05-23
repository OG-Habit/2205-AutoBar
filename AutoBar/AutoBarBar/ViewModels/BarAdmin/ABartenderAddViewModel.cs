using AutoBarBar.Models;
using System;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class ABartenderAddViewModel : BaseViewModel
    {
        private string name;
        private string email;
        private string contact;
        private DateTime birthday;
        private string sex;
        private string image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }

        public ABartenderAddViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            AddCommand = new Command(OnAddClicked);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);

        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Contact
        {
            get => contact;
            set => SetProperty(ref contact, value);
        }

        public DateTime Birthday
        {
            get => birthday;
            set => SetProperty(ref birthday, value);
        }

        public string Sex
        {
            get => sex;
            set => SetProperty(ref sex, value);
        }

        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        private async void OnCancelClicked()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnAddClicked()
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add as bartender?", "Yes", "No");
            if (retryBool)
            {
                Bartender item = new Bartender();
                item.Id = Guid.NewGuid().ToString();
                item.Name = Name;
                item.Email = Email;
                item.Contact = Contact;
                item.Birthday = Birthday;
                item.Sex = Sex;
                item.ImageLink = "default_pic";
                await BartenderDataStore.AddItemAsync(item);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
