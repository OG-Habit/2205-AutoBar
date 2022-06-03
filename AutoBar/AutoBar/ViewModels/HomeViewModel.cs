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
    public class HomeViewModel : BaseViewModel
    {

        private Product _selectedItem;
        public ObservableCollection<Product> Item { get; }
        public Command LoadItemCommand { get; }
        public Command<Product> ItemTapped { get; }
        public Command ItemSort { get; }
        public int state { get; set; }

        IToastService toastService;

        public decimal Balance { get; set; }

        public HomeViewModel()
        {
            SetBalance();
            state = 1;
            Item = new ObservableCollection<Product>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Product>(OnItemSelected);
            ItemSort = new Command(OnItemSort);
            toastService = DependencyService.Get<IToastService>();

        }

        async void SetBalance()
        {
            decimal bal = Convert.ToDecimal(await Xamarin.Essentials.SecureStorage.GetAsync("balance"));
            Balance = bal;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Item.Clear();
                var items = await ProductDataStore.GetItemsAsync(true);
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

        public Product SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private void OnItemSort()
        {
            ObservableCollection<Product> Item2 = new ObservableCollection<Product>();
            IsBusy = true;

            switch (state)
            {
                case 1:
                    state = 2;
                    Item2 = new ObservableCollection<Product>(Item.OrderByDescending(x => x.Name));
                    toastService.ShowShortMessage("Sorted by name (Descending)");
                    break;
                case 2:
                    state = 3;
                    Item2 = new ObservableCollection<Product>(Item.OrderBy(x => x.Price));
                    toastService.ShowShortMessage("Sorted by price (Ascending)");
                    break;
                case 3:
                    state = 4;
                    Item2 = new ObservableCollection<Product>(Item.OrderByDescending(x => x.Price));
                    toastService.ShowShortMessage("Sorted by price (Descending)");
                    break;
                case 4:
                    state = 1;
                    Item2 = new ObservableCollection<Product>(Item.OrderBy(x => x.Name));
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
        async void OnItemSelected(Product item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ProductDetailPage)}?{nameof(ProductDetailViewModel.ItemId)}={item.Id}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            try
            {
                Item.Clear();
                var items = await ProductDataStore.GetSearchResults(searchTerm);
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
