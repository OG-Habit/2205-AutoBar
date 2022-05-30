using AutoBar.Models;
using AutoBar.Views;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBar.ViewModels
{
    public class PastOrderViewModel : BaseViewModel
    {
        private Order _selectedItem;
        public ObservableCollection<Order> Order { get; }
        public Command LoadOrderCommand { get; }
        public Command<Order> ItemTapped { get; }
        public Command SwitchTapped { get; }

        public double Balance { get; set; }
        public DateTime Today { get; }
        public int UserID { get; }
        public Customer currentCustoemr;

        public PastOrderViewModel()
        {
            SetBalance();
            Today = DateTime.Now;
            Order = new ObservableCollection<Order>();
            LoadOrderCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Order>(OnItemSelected);
            SwitchTapped = new Command(OnSwitchSelected);

            string UserString = Xamarin.Essentials.SecureStorage.GetAsync("user").Result;
            currentCustoemr = JsonConvert.DeserializeObject<Customer>(UserString);
            UserID = currentCustoemr.UserDetails.ID;
        }
        private async void SetBalance()
        {
            string BalString = await Xamarin.Essentials.SecureStorage.GetAsync("balance");
            double Bal = Convert.ToDouble(BalString);
            Balance = Bal;
        }
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Order.Clear();
                var items = await OrderDataStore.GetSearchResults(currentCustoemr.ID);
                foreach (var item in items.OrderByDescending(x => x.ClosedOn.Date))
                {
                    if (item.OrderStatus != 1)
                    {
                        Order.Add(item);
                    }
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

        async void OnSwitchSelected()
        {
            await Shell.Current.GoToAsync("..");
        }

        public Order SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(Order item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(PastOrderDetailPage)}?{nameof(PastOrderDetailViewModel.ItemId)}={item.Id}");
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            try
            {
                Order.Clear();
                var items = await OrderDataStore.GetSearchResults(searchTerm);
                foreach (var item in items)
                {
                    Order.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
