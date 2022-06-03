using AutoBarBar.Models;
using AutoBarBar.Views;
using AutoBarBar.Services;

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
        private Order _selectedCustomer;
        public ObservableCollection<Order> Customer { get; }
        public Command LoadCustomerCommand { get; }
        public Command<Order> CustomerTapped { get; }
        public DateTime Today { get; set; }
        IRevenueService revenueService;

        int orderToday;
        int orderWeek;
        double revenueToday;
        double revenueWeek;

        public int OrderToday
        {
            get => orderToday;
            set => SetProperty(ref orderToday, value);
        }

        public int OrderWeek
        {
            get => orderWeek;
            set => SetProperty(ref orderWeek, value);
        }
        
        public double RevenueToday
        {
            get => revenueToday;
            set => SetProperty(ref revenueToday, value);
        }

        public double RevenueWeek
        {
            get => revenueWeek;
            set => SetProperty(ref revenueWeek, value);
        }

        public AHomeViewModel()
        {
            Title = "Home";
            Today = DateTime.Today;
            Customer = new ObservableCollection<Order>();
            LoadCustomerCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CustomerTapped = new Command<Order>(OnCustomerSelected);
            revenueService = DependencyService.Get<IRevenueService>();

            SetStats();
        }

        private async void SetStats()
        {
            Revenue order = await revenueService.GetRevenues();
            OrderToday = order.TotalOrders;
            OrderWeek = order.TotalWeekOrders;
            RevenueToday = order.TotalRevenue;
            RevenueWeek = order.TotalWeekRevenue;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Customer.Clear();
                var items = await OrderDataStore.GetItemsAsync(true);
                foreach (var item in items.OrderByDescending(x => x.ClosedOn))
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

        public Order SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
                OnCustomerSelected(value);
            }
        }

        async void OnCustomerSelected(Order item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(AOrderDetailPage)}?{nameof(AOrderDetailViewModel.ItemId)}={item.ID}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;
            
            try
            {
                Customer.Clear();
                var items = await OrderDataStore.GetSearchResults(searchTerm);
                foreach (var item in items)
                {
                    Customer.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
