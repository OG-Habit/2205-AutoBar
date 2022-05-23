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
        private Reward _selectedReward;
        public ObservableCollection<Reward> Rewards { get; }
        public Command LoadRewardsCommand { get; }
        public Command RewardAdd { get; }
        public Command<Reward> RewardTapped { get; }
        public Command<Reward> RewardEdit { get; }
        public DateTime Today { get; set; }

        public ARewardsViewModel()
        {
            Title = "Rewards";
            Today = DateTime.Today;
            Rewards = new ObservableCollection<Reward>();
            
            LoadRewardsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            RewardAdd = new Command(OnAddSelected);

            RewardTapped = new Command<Reward>(OnRewardSelected);

            RewardEdit = new Command<Reward>(OnRewardEdit);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Rewards.Clear();
                var items = await RewardDataStore.GetItemsAsync(true);
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

        async void OnAddSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(ARewardsAddPage)}");
        }

        public Reward SelectedReward
        {
            get => _selectedReward;
            set
            {
                SetProperty(ref _selectedReward, value);
                OnRewardSelected(value);
            }
        }

        async void OnRewardSelected(Reward item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ARewardsDetailPage)}?{nameof(ARewardsDetailViewModel.ItemId)}={item.Id}");
        }

        public Reward EditReward
        {
            get => _selectedReward;
            set
            {
                SetProperty(ref _selectedReward, value);
                OnRewardEdit(value);
            }
        }

        async void OnRewardEdit(Reward item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ARewardsEditPage)}?{nameof(ARewardsDetailViewModel.ItemId)}={item.Id}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;
            searchTerm = searchTerm.ToLowerInvariant();

            await ExecuteLoadItemsCommand();

            var items = Rewards.Where(x => x.Name.ToLowerInvariant().Contains(searchTerm)).ToList();

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
