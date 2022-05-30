using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ProductDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private double price;
        private string description;
        private string image;

        public string Id { get; set; }

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
                Image = "default_menu.png"; //item.ImageLink;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
