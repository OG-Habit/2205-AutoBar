using AutoBarBar.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        private ImageSource image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }
        public Command ImageCommand { get; }

        public ABartenderAddViewModel()
        {
            image = "default_pic.png";
            CancelCommand = new Command(OnCancelClicked);
            AddCommand = new Command(OnAddClicked);
            ImageCommand = new Command(OnImageClicked);
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
            set
            {
                if (image == value)
                    return;
                image = value;
                OnPropertyChanged();
            }
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
                if (Name != null && Email != null && Contact != null && Sex != null && Birthday != null)
                {
                    Bartender item = new Bartender
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = Name,
                        Email = Email,
                        Contact = Contact,
                        Birthday = Birthday,
                        Sex = Sex,
                        ImageLink = (Image is FileImageSource source) ? source.File : "default_pic"
                    };
                    await BartenderDataStore.AddItemAsync(item);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Field/s are empty", "Okay");
                }                
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
