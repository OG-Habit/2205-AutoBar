using AutoBarBar.Models;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ARewardsDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private double point;
        private string description;
        private ImageSource image;

        public string Id { get; set; }

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command ImageCommand { get; }

        public ARewardsDetailViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            SaveCommand = new Command(OnSaveClicked);
            DeleteCommand = new Command(OnDeleteClicked);
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
                var item = await RewardDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Point = item.Points;
                Description = item.Description;
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
                if (Name != null && Description != null) 
                { 
                    Reward item = new Reward
                    {
                        Id = ItemId,
                        Name = Name,
                        Points = Point,
                        Description = Description,
                        ImageLink = (Image is FileImageSource source) ? source.File : "default_pic"
                    };
                    await RewardDataStore.UpdateItemAsync(item);
                    await Shell.Current.GoToAsync("..");
                    }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Field/s are empty", "Okay");
                }
            }
        }

        private async void OnDeleteClicked()
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Delete", "Would you like to delete reward?", "Yes", "No");
            if (retryBool)
            {
                await RewardDataStore.DeleteItemAsync(ItemId);
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
