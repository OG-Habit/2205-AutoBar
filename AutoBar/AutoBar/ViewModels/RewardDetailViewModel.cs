using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class RewardDetailViewModel : BaseViewModel
    {
        private int itemId;
        private string name;
        private double points;
        private string description;
        private string image;

        public string Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public double Points
        {
            get => points;
            set => SetProperty(ref points, value);
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
                var item = await RewardDataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Points = item.Points;
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
