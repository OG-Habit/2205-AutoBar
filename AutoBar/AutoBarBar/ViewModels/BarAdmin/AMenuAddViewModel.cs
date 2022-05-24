using AutoBarBar.Models;
using System;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class AMenuAddViewModel : BaseViewModel
    {
        private string name;
        private double price;
        private string description;
        private string image;

        public Command CancelCommand { get; }
        public Command AddCommand { get; }

        public AMenuAddViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
            AddCommand = new Command(OnAddClicked);
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

        private async void OnCancelClicked()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnAddClicked()
        {
            //bool retryBool = await App.Current.MainPage.DisplayAlert("Add", "Would you like to add to menu?", "Yes", "No");
            //if (retryBool)
            //{
            //    Product item = new Product();
            //    item.Id = Guid.NewGuid().ToString();
            //    item.Name = Name;
            //    item.UnitPrice = Price;
            //    item.Description = Description;
            //    item.ImageLink = "default_pic";
            //    await ProductDataStore.AddItemAsync(item);
            //    await Shell.Current.GoToAsync("..");
            //}
        }
    }
}
