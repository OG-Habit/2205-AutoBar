using AutoBar.Models;
using AutoBar.Views;
using AutoBar.Services;
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

        public decimal Points { get; set; }
        public Command ItemSort { get; }
        public int state { get; set; }

        IToastService toastService;

        public RewardViewModel()
        {
            SetPoints();
            state = 1;
            Item = new ObservableCollection<Reward>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Reward>(OnItemSelected);
            ItemSort = new Command(OnItemSort);
            toastService = DependencyService.Get<IToastService>();
        }

        async void SetPoints()
        {
            decimal p = Convert.ToDecimal(await Xamarin.Essentials.SecureStorage.GetAsync("points"));
            Points = p;
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

        private void OnItemSort()
        {
            ObservableCollection<Reward> Item2 = new ObservableCollection<Reward>();
            IsBusy = true;

            switch (state)
            {
                case 1:
                    state = 2;
                    Item2 = new ObservableCollection<Reward>(Item.OrderByDescending(x => x.Name));
                    toastService.ShowShortMessage("Sorted by name (Descending)");
                    break;
                case 2:
                    state = 3;
                    Item2 = new ObservableCollection<Reward>(Item.OrderBy(x => x.Points));
                    toastService.ShowShortMessage("Sorted by points (Ascending)");
                    break;
                case 3:
                    state = 4;
                    Item2 = new ObservableCollection<Reward>(Item.OrderByDescending(x => x.Points));
                    toastService.ShowShortMessage("Sorted by points (Descending)");
                    break;
                case 4:
                    state = 1;
                    Item2 = new ObservableCollection<Reward>(Item.OrderBy(x => x.Name));
                    toastService.ShowShortMessage("Sorted by name (Ascending)");
                    break;
            }
            Item.Clear();
            foreach (var item in Item2)
            {
                Item.Add(item);
            }
            IsBusy = false;
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
