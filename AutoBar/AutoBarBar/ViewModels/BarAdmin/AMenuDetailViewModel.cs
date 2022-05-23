using AutoBarBar.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AMenuDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private double price;
        private string description;
        private string image;

        public string Id { get; set; }

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }

        public AMenuDetailViewModel()
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

        public double Price
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
                var item = await ProductDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Price = item.Price;
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
                Product item = new Product();
                item.Id = ItemId;
                item.Name = Name;
                item.Price = Price;
                item.Description = Description;
                item.ImageLink = Image;
                await ProductDataStore.UpdateItemAsync(item);
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnDeleteClicked()
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Delete", "Would you like to delete item?", "Yes", "No");
            if (retryBool)
            {
                await ProductDataStore.DeleteItemAsync(ItemId);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
