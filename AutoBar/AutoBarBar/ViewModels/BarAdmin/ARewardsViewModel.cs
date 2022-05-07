using AutoBarBar.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class ARewardsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Rewards { get; }
        public Command LoadRewardsCommand { get; }

        public ARewardsViewModel()
        {
            Title = "Rewards";
            Rewards = new ObservableCollection<Item>();
            LoadRewardsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Rewards.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Rewards.Add(item);
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

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;
            searchTerm = searchTerm.ToLowerInvariant();

            await ExecuteLoadItemsCommand();

            var items = Rewards.Where(x => x.Reward.ToLowerInvariant().Contains(searchTerm)).ToList();

            foreach (var item in Rewards.ToList())
            {
                if (!items.Contains(item))
                {
                    Rewards.Remove(item);
                }
                else if (!Rewards.Contains(item))
                {
                    Rewards.Add(item);
                }
            }
        }
    }
}
