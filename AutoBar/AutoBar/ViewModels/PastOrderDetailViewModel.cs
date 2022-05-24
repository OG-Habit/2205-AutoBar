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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class PastOrderDetailViewModel : BaseViewModel
    {
        private OrderLine _selectedItem;
        public ObservableCollection<OrderLine> Order { get; }
        public Command LoadOrderCommand { get; }
        public Command<OrderLine> ItemTapped { get; }

        public double Balance { get; }

        private string itemId;
        private int points;
        private string reward;
        private DateTime time;
        private double total;

        public PastOrderDetailViewModel()
        {
            Balance = 1200.00;
            Order = new ObservableCollection<OrderLine>();
            LoadOrderCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<OrderLine>(OnItemSelected);
        }

        public double Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        public int Points
        {
            get => points;
            set => SetProperty(ref points, value);
        }

        public string Reward
        {
            get => reward;
            set => SetProperty(ref reward, value);
        }

        public DateTime Time
        {
            get => time;
            set => SetProperty(ref time, value);
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
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Order.Clear();
                var past = await OrderDataStore.GetItemAsync(ItemId);
                var items = await OrderLineDataStore.GetSearchResults(ItemId);
                foreach (var item in items.OrderByDescending(x => x.CreatedOn))
                {
                    Order.Add(item);
                }
                Time = past.OpenedOn;
                Total = past.TotalPrice;
                Points = past.PointsEarned;
                Reward = past.Reward;
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
            await Shell.Current.GoToAsync($"{nameof(PastOrderPage)}");
        }

        public OrderLine SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(OrderLine item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(OrderDetailPage)}?{nameof(OrderDetailViewModel.ItemId)}={item.Id}");
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

    }
}
