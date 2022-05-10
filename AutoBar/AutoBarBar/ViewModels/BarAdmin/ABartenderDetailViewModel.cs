using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ABartenderDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string bName;
        private string email;
        private string image;

        public string Id { get; set; }

        public Command CancelCommand { get; }

        public ABartenderDetailViewModel()
        {
            CancelCommand = new Command(OnCancelClicked);
        }

        public string B_Name
        {
            get => bName;
            set => SetProperty(ref bName, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
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
                B_Name = item.B_Name;
                Email = item.Email;
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
