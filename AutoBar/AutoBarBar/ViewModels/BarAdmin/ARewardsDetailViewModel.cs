using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ARewardsDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string reward;
        private string point;
        private string description;
        private string image;

        public string Id { get; set; }

        public Command CancelCommand { get; }

        public ARewardsDetailViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
        }

        public string Reward
        {
            get => reward;
            set => SetProperty(ref reward, value);
        }

        public string Point
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
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Reward = item.Reward;
                Point = item.Points;
                Description = item.Description;
                Image = item.Image;
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
    }
}
