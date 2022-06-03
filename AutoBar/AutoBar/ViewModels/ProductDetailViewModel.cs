using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ProductDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string name;
        private decimal price;
        private string description;
        private string image;

        public int Id { get; set; }

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

        public string Image
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

        public async void LoadItemId(int itemId)
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
    }
}
