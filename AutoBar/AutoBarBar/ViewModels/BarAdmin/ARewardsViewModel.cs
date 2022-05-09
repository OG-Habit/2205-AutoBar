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
    public class ARewardsViewModel : BaseViewModel
    {
        private Item _selectedReward;
        public ObservableCollection<Item> Rewards { get; }
        public Command LoadRewardsCommand { get; }
        public Command<Item> RewardTapped { get; }
        public Command<Item> RewardEdit { get; }
        public DateTime Today { get; set; }

        public ARewardsViewModel()
        {
            Title = "Rewards";
            Today = DateTime.Today;
            Rewards = new ObservableCollection<Item>();
            
            LoadRewardsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            RewardTapped = new Command<Item>(OnRewardSelected);

            RewardEdit = new Command<Item>(OnRewardEdit);
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

        public Item SelectedReward
        {
            get => _selectedReward;
            set
            {
                SetProperty(ref _selectedReward, value);
                OnRewardSelected(value);
            }
        }

        async void OnRewardSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ARewardsDetailPage)}?{nameof(ARewardsDetailViewModel.ItemId)}={item.Id}");
        }

        public Item EditReward
        {
            get => _selectedReward;
            set
            {
                SetProperty(ref _selectedReward, value);
                OnRewardEdit(value);
            }
        }

        async void OnRewardEdit(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ARewardsEditPage)}?{nameof(ARewardsDetailViewModel.ItemId)}={item.Id}");
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
