using AutoBarBar.Models;
using AutoBarBar.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class AHomeViewModel: BaseViewModel
    {
        private Item _selectedCustomer;
        public ObservableCollection<Item> Customer { get; }
        public Command LoadCustomerCommand { get; }
        public Command<Item> CustomerTapped { get; }
        public DateTime Today { get; set; }

        public AHomeViewModel()
        {
            Title = "Home";
            Today = DateTime.Today;
            Customer = new ObservableCollection<Item>();
            
            LoadCustomerCommand = new Command(async () => await ExecuteLoadItemsCommand());
            
            CustomerTapped = new Command<Item>(OnCustomerSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Customer.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Customer.Add(item);
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

        public Item SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                OnCustomerSelected(value);
            }
        }

        async void OnCustomerSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(AOrderDetailPage)}?{nameof(AOrderDetailViewModel.ItemId)}={item.Id}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;
            searchTerm = searchTerm.ToLowerInvariant();

            await ExecuteLoadItemsCommand();

            var items = Customer.Where(x => x.C_Name.ToLowerInvariant().Contains(searchTerm)).ToList();

            foreach (var item in Customer.ToList())
            {
                if (!items.Contains(item))
                {
                    Customer.Remove(item);
                }
                else if (!Customer.Contains(item))
                {
                    Customer.Add(item);
                }
            }
        }
    }
}
