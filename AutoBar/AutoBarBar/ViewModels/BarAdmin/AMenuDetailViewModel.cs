﻿using AutoBarBar.Models;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AMenuDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string name;
        private decimal price;
        private string description;
        private ImageSource image;
        private int orderFrequencyToday;
        private int orderFrequencyPast7Days;
        private int orderFrequencyOverall;

        public int Id { get; set; }

        public Command CancelCommand { get; }
        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command ImageCommand { get; }

        public AMenuDetailViewModel()
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

        public decimal Price
        {
            get => price;
            set => SetProperty(ref price, value);
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


        public int OrderFrequencyToday
        {
            get => orderFrequencyToday;
            set => SetProperty(ref orderFrequencyToday, value);
        }
        public int OrderFrequencyPast7Days
        {
            get => orderFrequencyPast7Days;
            set => SetProperty(ref orderFrequencyPast7Days, value);
        }
        public int OrderFrequencyOverall
        {
            get => orderFrequencyOverall;
            set => SetProperty(ref orderFrequencyOverall, value);
        }

        public async void LoadItemId(int itemId)
        {
            try
            {
                var item = await ProductDataStore.GetItemAsync(itemId);
                Id = item.ID;
                Name = item.Name;
                Price = item.UnitPrice;
                Description = item.Description;
                Image = item.ImageLink;

                OrderFrequencyToday = item.OrderFrequencyToday;
                OrderFrequencyPast7Days = item.OrderFrequencyPast7Days;
                OrderFrequencyOverall = item.OrderFrequencyOverall;
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
            if (price <= 0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a value greater than 0 for price", "Okay");
                return;
            }
            bool retryBool = await App.Current.MainPage.DisplayAlert("Save", "Would you like to save changes?", "Yes", "No");
            if (retryBool)
            {
                Product item = new Product
                {
                    ID = ItemId,
                    Name = Name,
                    UnitPrice = Price,
                    Description = Description,
                    ImageLink = "default_menu.png" //(Image is FileImageSource source) ? source.File : 
                };
                bool success = await ProductDataStore.UpdateItemAsync(item);
                if (success == true)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Product item updated!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Product name already exists.", "Retry");
                }

            }
        }

        private async void OnDeleteClicked()
        {
            bool retryBool = await App.Current.MainPage.DisplayAlert("Delete", "Would you like to delete item?", "Yes", "No");
            if (retryBool)
            {
                //await ProductDataStore.DeleteItemAsync(ItemId);
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
