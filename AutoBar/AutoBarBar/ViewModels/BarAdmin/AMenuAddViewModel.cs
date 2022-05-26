using AutoBarBar.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class AMenuAddViewModel : BaseViewModel
    {
        private string name;
        private decimal price;
        private string description;
        private ImageSource image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }
        public Command ImageCommand { get; }

        public AMenuAddViewModel()
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

        private async void OnCancelClicked()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnAddClicked()
        {
            /*
            bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add to menu?", "Yes", "No");
            if (retryBool)
            {
                if (Name != null && Description != null)
                {
                    Product item = new Product
                    {
                        ID = 1,
                        Name = Name,
                        UnitPrice = Price,
                        Description = Description,
                        ImageLink = (Image is FileImageSource source) ? source.File : "default_pic"
                    };
                    await ProductDataStore.AddItemAsync(item);
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Field/s are empty", "Okay");
                }
            }
            */
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
