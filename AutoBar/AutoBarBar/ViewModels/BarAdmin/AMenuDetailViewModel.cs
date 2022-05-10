using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AMenuDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string drink;
        private string price;
        private string description;
        private string image;

        public string Id { get; set; }

        public Command CancelCommand { get; }

        public AMenuDetailViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
        }

        public string Drink
        {
            get => drink;
            set => SetProperty(ref drink, value);
        }

        public string Price
        {
            get => price;
            set => SetProperty(ref price, value);
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
                Drink = item.Drink;
                Price = item.Price;
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
