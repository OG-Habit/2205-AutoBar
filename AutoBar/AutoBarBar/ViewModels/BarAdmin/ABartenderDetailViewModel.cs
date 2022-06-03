using AutoBarBar.Models;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ABartenderDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string firstName;
        private string lastName;
        private string name;
        private string email;
        private string contact;
        private DateTime birthday;
        private string sex;
        private ImageSource image;

        private decimal revenueGeneratedToday;
        private decimal revenueGeneratedPast7Days;
        private decimal revenueGeneratedOverall;

        public int Id { get; set; }

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command ImageCommand { get; }

        public ABartenderDetailViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            SaveCommand = new Command(OnSaveClicked);
            DeleteCommand = new Command(OnDeleteClicked);
            ImageCommand = new Command(OnImageClicked);
        }

        public string FirstName
        {
            get => firstName;
            set => SetProperty(ref firstName, value);
        }

        public string LastName
        {
            get => lastName;
            set => SetProperty(ref lastName, value);
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

        public ImageSource Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public int ItemId
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

        public decimal RevenueGeneratedToday
        {
            get => revenueGeneratedToday;
            set => SetProperty(ref revenueGeneratedToday, value);
        }
        public decimal RevenueGeneratedPast7Days
        {
            get => revenueGeneratedPast7Days;
            set => SetProperty(ref revenueGeneratedPast7Days, value);
        }
        public decimal RevenueGeneratedOverall
        {
            get => revenueGeneratedOverall;
            set => SetProperty(ref revenueGeneratedOverall, value);
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await BartenderDataStore.GetItemAsync(itemId);
                Id = item.Id;
                FirstName = item.FirstName;
                LastName = item.LastName;
                Name = FirstName + " " + LastName;
                Email = item.Email;
                Contact = item.Contact;
                Birthday = item.Birthday;
                Sex = item.Sex;
                // set location for file
                Image = item.ImageLink;

                RevenueGeneratedToday = item.RevenueGeneratedToday;
                RevenueGeneratedPast7Days = item.RevenueGeneratedPast7Days;
                RevenueGeneratedOverall = item.RevenueGeneratedOverall;

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
                if (FirstName != null && LastName != null && Email != null && Contact != null && Sex != null && Birthday != null)
                {
                    Bartender item = new Bartender
                    {
                        Id = ItemId,
                        FirstName = FirstName,
                        LastName = LastName,
                        Email = Email,
                        Contact = Contact,
                        Birthday = Birthday,
                        Sex = Sex,
                        ImageLink = (Image is FileImageSource source) ? source.File : "default_menu.png"
                    };
                    bool success = await BartenderDataStore.UpdateItemAsync(item);
                    if (success == true)
                    {
                        await App.Current.MainPage.DisplayAlert("Success", "Bartender updated!", "OK");
                        await Shell.Current.GoToAsync("..");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Failed", "This email has been used by another user.", "Retry");
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Field/s are empty", "Okay");
                }
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

        async void OnImageClicked()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please pick a photo"
            });
            if (result == null)
                return;

            var stream = await result.OpenReadAsync();
            image = ImageSource.FromStream(() => stream);
        }
    }
}
