﻿using AutoBarBar.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class ARewardsAddViewModel : BaseViewModel
    {
        private string name;
        private decimal point;
        private string description;
        private ImageSource image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }
        public Command ImageCommand { get; }

        public ARewardsAddViewModel()
        {
            image = "default_reward.png";
            CancelCommand = new Command(OnCancelClicked);
            AddCommand = new Command(OnAddClicked);
            ImageCommand = new Command(OnImageClicked);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public decimal Point
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
            if (Name == null || Description == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Field/s are empty", "Okay");
                return;
            }
            if (Point <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a value greater than 0 for points", "Okay");
                return;
            }

            bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add to rewards?", "Yes", "No");
            if (retryBool)
            {
                Reward item = new Reward
                {
                    Name = Name,
                    Points = Point,
                    Description = Description,
                    ImageLink = "default_reward.png" // (Image is FileImageSource source) ? source.File :
                };
                bool success = await RewardDataStore.AddItemAsync(item);
                if (success == true)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Reward item added!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Reward name already exists.", "Retry");
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
