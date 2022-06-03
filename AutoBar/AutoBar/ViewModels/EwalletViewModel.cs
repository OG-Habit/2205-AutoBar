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
    public class EwalletViewModel : BaseViewModel
    {
        public decimal Balance { get; set; }
        public decimal Points { get; set; }
        public string ImageLink { get; }
        public ObservableCollection<TransactionHistory> Item1 { get; }
        public ObservableCollection<PointsHistory> Item2 { get; }
        public Command LoadItemCommand { get; }


        public EwalletViewModel()
        {
            SetBalancePoints();
            ImageLink = "default_pic";
            Item1 = new ObservableCollection<TransactionHistory>();
            Item2 = new ObservableCollection<PointsHistory>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async void SetBalancePoints()
        {
            decimal bal = Convert.ToDecimal(await Xamarin.Essentials.SecureStorage.GetAsync("balance"));
            decimal points = Convert.ToDecimal(await Xamarin.Essentials.SecureStorage.GetAsync("points"));
            Balance = bal;
            Points = points;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Item1.Clear();
                Item2.Clear();
                var items1 = await TransactionHistoryDataStore.GetItemsAsync(true);
                foreach (var item in items1.OrderBy(x => x.TimeStamp))
                {
                    Item1.Add(item);
                }
                var items2 = await PointsHistoryDataStore.GetItemsAsync(true);
                foreach (var item in items2.OrderBy(x => x.TimeStamp))
                {
                    Item2.Add(item);
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
