using AutoBarBar.Models;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AutoBarBar.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class AOrderDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string name;
        private DateTime time;
        private double payment;
        private string status;
        private string image;
        private DateTime birthday;
        private DateTime cardIssued;
        private string sex;
        private string contact;
        private string email;
        private double points;
        private string bartender;
        private string reward;

        public ObservableCollection<OrderLine> Items{ get; }
        public Command LoadItemsCommand { get; }

        public string Id { get; set; }

        public AOrderDetailViewModel()
        {
            Items = new ObservableCollection<OrderLine>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public DateTime Time
        {
            get => time;
            set => SetProperty(ref time, value);
        }

        public double Price
        {
            get => payment;
            set => SetProperty(ref payment, value);
        }

        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public DateTime Birthday
        {
            get => birthday;
            set => SetProperty(ref birthday, value);
        }

        public DateTime CardIssued
        {
            get => cardIssued;
            set => SetProperty(ref cardIssued, value);
        }

        public string Sex
        {
            get => sex;
            set => SetProperty(ref sex, value);
        }

        public string Contact
        {
            get => contact;
            set => SetProperty(ref contact, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public double Points
        {
            get => points;
            set => SetProperty(ref points, value);
        }

        public string Bartender
        {
            get => bartender;
            set => SetProperty(ref bartender, value);
        }

        public string Reward
        {
            get => reward;
            set => SetProperty(ref reward, value);
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
                var item = await OrderDataStore.GetItemAsync(itemId);
                var customer = await CustomerDataStore.GetItemAsync(item.CustomerID.ToString());
                Id = customer.Id;
                Name = customer.Name;
                Status = customer.Status;
                Image = customer.ImageLink;
                Birthday = customer.Birthday;
                CardIssued = customer.CardIssued;
                Sex = customer.Sex;
                Contact = customer.Contact;
                Email = customer.Email;
                Time = item.ClosedOn;
                Price = item.TotalPrice;
                Points = item.PointsEarned;
                Bartender = item.BartenderName;
                Reward = item.Reward;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await OrderLineDataStore.GetSearchResults(itemId);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
