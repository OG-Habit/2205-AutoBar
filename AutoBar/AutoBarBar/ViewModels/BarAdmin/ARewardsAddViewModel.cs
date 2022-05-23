using AutoBarBar.Models;
using System;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class ARewardsAddViewModel : BaseViewModel
    {
        private string name;
        private double point;
        private string description;
        private string image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }

        public ARewardsAddViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            AddCommand = new Command(OnAddClicked);
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
            bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add to rewards?", "Yes", "No");
            if (retryBool)
            {
                Reward item = new Reward();
                item.Id = Guid.NewGuid().ToString();
                item.Name = Name;
                item.Points = Point;
                item.Description = Description;
                item.ImageLink = "default_pic";
                await RewardDataStore.AddItemAsync(item);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
