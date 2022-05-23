using AutoBarBar.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ABartenderDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private string email;
        private string contact;
        private DateTime birthday;
        private string sex;
        private string image;

        public string Id { get; set; }

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public ABartenderDetailViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            SaveCommand = new Command(OnSaveClicked);
            DeleteCommand = new Command(OnDeleteClicked);
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

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await BartenderDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Email = item.Email;
                Contact = item.Contact;
                Birthday = item.Birthday;
                Sex = item.Sex;
                Image = item.ImageLink;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        private async void OnCancelClicked()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSaveClicked()
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Save", "Would you like to save changes?", "Yes", "No");
            if (retryBool)
            {
                Bartender item = new Bartender();
                item.Id = ItemId;
                item.Name = Name;
                item.Email = Email;
                item.Contact = Contact;
                item.Birthday = Birthday;
                item.Sex = Sex;
                item.ImageLink = Image;
                await BartenderDataStore.UpdateItemAsync(item);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnDeleteClicked()
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Delete", "Would you like to delete bartender?", "Yes", "No");
            if (retryBool)
            {
                await BartenderDataStore.DeleteItemAsync(ItemId);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
