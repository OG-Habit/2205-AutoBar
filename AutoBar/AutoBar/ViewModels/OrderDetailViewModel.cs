using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class OrderDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private double price;
        private string description;
        private string image;
        private int quantity;
        private DateTime created;
        private double total;
        private string bartender;

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

        public int Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        public DateTime Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }

        public double Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        public string Bartender
        {
            get => bartender;
            set => SetProperty(ref bartender, value);
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
                var item = await OrderLineDataStore.GetItemAsync(itemId);
                var product = await ProductDataStore.GetItemAsync(item.ProductId);
                var order = await OrderDataStore.GetItemAsync(item.OrderId);
                Id = item.Id;
                Name = product.Name;
                Price = product.Price;
                Description = product.Description;
                Image = product.ImageLink;
                Quantity = item.Quantity;
                Created = item.CreatedOn;
                Total = item.SubTotal;
                Bartender = order.BartenderName;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
