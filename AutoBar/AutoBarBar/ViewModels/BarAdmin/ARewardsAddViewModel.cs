using AutoBarBar.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class ARewardsAddViewModel : BaseViewModel
    {
        private string name;
        private double point;
        private string description;
        private ImageSource image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }
        public Command ImageCommand { get; }

        public ARewardsAddViewModel()
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

        public double Point
        {
            get => point;
            set => SetProperty(ref point, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public ImageSource Image
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
            bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add to rewards?", "Yes", "No");
            if (retryBool)
            {
                if (Name != null && Description != null)
                {
                    Reward item = new Reward
                    {
                        ID = 1,
                        Name = Name,
                        //Points = Point,
                        Description = Description,
                        ImageLink = (Image is FileImageSource source) ? source.File : "default_pic"
                    };
                    await RewardDataStore.AddItemAsync(item);
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
