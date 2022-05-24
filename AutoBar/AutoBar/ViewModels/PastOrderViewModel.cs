using AutoBar.Models;
using AutoBar.Views;
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

        public double Balance { get; }
        public DateTime Today { get; }
        public string UserID { get; }

        public PastOrderViewModel()
        {
            Balance = 1200.00;
            Today = DateTime.Now;
            UserID = "1";
            Order = new ObservableCollection<Order>();
            LoadOrderCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Order>(OnItemSelected);
            SwitchTapped = new Command(OnSwitchSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Order.Clear();
                var items = await OrderDataStore.GetSearchResults(UserID);
                foreach (var item in items.OrderByDescending(x => x.ClosedOn.Date))
                {
                    if (item.OpenedOn.Date != Today.Date)
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
