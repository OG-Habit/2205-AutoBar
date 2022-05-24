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
    public class HomeViewModel : BaseViewModel
    {
        private Product _selectedItem;
        public ObservableCollection<Product> Item { get; }
        public Command LoadItemCommand { get; }
        public Command<Product> ItemTapped { get; }

        public double Balance { get; }

        public HomeViewModel()
        {
            Balance = 1200.00;
            Item = new ObservableCollection<Product>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Product>(OnItemSelected);
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
