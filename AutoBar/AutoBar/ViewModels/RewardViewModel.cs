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
    public class RewardViewModel : BaseViewModel
    {
        private Reward _selectedItem;
        public ObservableCollection<Reward> Item { get; }
        public Command LoadItemCommand { get; }
        public Command<Reward> ItemTapped { get; }

        public double Balance { get; }

        public RewardViewModel()
        {
            Balance = 500.00;
            Item = new ObservableCollection<Reward>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Reward>(OnItemSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Item.Clear();
                var items = await RewardDataStore.GetItemsAsync(true);
                foreach (var item in items.OrderBy(x => x.Name))
                {
                    Item.Add(item);
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

        public Reward SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(Reward item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(RewardDetailPage)}?{nameof(RewardDetailViewModel.ItemId)}={item.Id}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            try
            {
                Item.Clear();
                var items = await RewardDataStore.GetSearchResults(searchTerm);
                foreach (var item in items)
                {
                    Item.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
