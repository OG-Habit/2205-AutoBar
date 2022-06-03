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
            image = "default_menu.png";
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

            bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add to menu?", "Yes", "No");
            if (retryBool)
            {

                Product item = new Product
                {
                    Name = Name,
                    UnitPrice = Price,
                    Description = Description,
                    ImageLink = "default_menu.png" //(Image is FileImageSource source) ? source.File :
                };
                bool success = await ProductDataStore.AddItemAsync(item);
                if (success == true)
                {
                    await App.Current.MainPage.DisplayAlert("Success", "Product item added!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Failed", "Product name already exists.", "Retry");
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
