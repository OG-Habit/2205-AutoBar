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
        public double Balance { get; }
        public string ImageLink { get; }
        public ObservableCollection<TransactionHistory> Item1 { get; }
        public ObservableCollection<PointsHistory> Item2 { get; }
        public Command LoadItemCommand { get; }

        public EwalletViewModel()
        {
            Balance = 1200.00;
            ImageLink = "default_pic";
            Item1 = new ObservableCollection<TransactionHistory>();
            Item2 = new ObservableCollection<PointsHistory>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
