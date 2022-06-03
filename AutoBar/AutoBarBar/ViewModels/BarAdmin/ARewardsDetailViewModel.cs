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
        private int itemId;
        private string name;
        private decimal point;
        private string description;
        private ImageSource image;
        private int frequency;

        private int claimFrequencyToday;
        private int claimFrequencyPast7Days;
        private int claimFrequencyOverall;

        public int Id { get; set; }

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

        public int Frequency
        {
            get => frequency;
            set => SetProperty(ref frequency, value);
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

        public int ClaimFrequencyToday
        {
            get => claimFrequencyToday;
            set => SetProperty(ref claimFrequencyToday, value);
        }
        public int ClaimFrequencyPast7Days
        {
            get => claimFrequencyPast7Days;
            set => SetProperty(ref claimFrequencyPast7Days, value);
        }
        public int ClaimFrequencyOverall
        {
            get => claimFrequencyOverall;
            set => SetProperty(ref claimFrequencyOverall, value);
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await RewardDataStore.GetItemAsync(itemId);
                Id = item.ID;
                Name = item.Name;
                Point = item.Points;
                Description = item.Description;
                Image = item.ImageLink;

                ClaimFrequencyToday = item.ClaimFrequencyToday;
                ClaimFrequencyPast7Days = item.ClaimFrequencyPast7Days;
                ClaimFrequencyOverall = item.ClaimFrequencyOverall;
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
            bool retryBool = await App.Current.MainPage.DisplayAlert("Save", "Would you like to save changes?", "Yes", "No");
            if (retryBool)
            {
                Reward item = new Reward
                {
                    ID = ItemId,
                    Name = Name,
                    Points = Point,
                    Description = Description,
                    ImageLink = "default_reward.png" //(Image is FileImageSource source) ? source.File : 
                };
                bool success = await RewardDataStore.UpdateItemAsync(item);
                if (success == true)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Item updated!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Reward name already exists.", "Retry");
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
