﻿using AutoBar.Models;
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
        public Command SwitchTapped { get; }

        public decimal Balance { get; set; }

        private int itemId;
        private decimal points;
        private string reward;
        private DateTime time;
        private double total;

        public PastOrderDetailViewModel()
        {
            SetBalance();
            Order = new ObservableCollection<OrderLine>();
            LoadOrderCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<OrderLine>(OnItemSelected);
            SwitchTapped = new Command(OnSwitchSelected);
        }
        async void SetBalance()
        {
            decimal bal = Convert.ToDecimal(await Xamarin.Essentials.SecureStorage.GetAsync("balance"));
            Balance = bal;
        }
        public double Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        public decimal Points
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

        public int ItemId
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
                var items = await OrderLineDataStore.GetSearchResults(ItemId.ToString());
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
            await Shell.Current.GoToAsync($"..");
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
