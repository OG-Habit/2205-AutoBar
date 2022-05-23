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
    public class AMenuViewModel : BaseViewModel
    {
        private Product _selectedItem;
        public ObservableCollection<Product> Item { get; }
        public Command LoadItemCommand { get; }
        public Command ItemAdd { get; }
        public Command<Product> ItemTapped { get; }
        public Command<Product> ItemEdit { get; }
        public DateTime Today { get; set; }

        public AMenuViewModel()
        {
            Title = "Menu";
            Today = DateTime.Today;
            Item = new ObservableCollection<Product>();
            
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemAdd = new Command(OnAddSelected);

            ItemTapped = new Command<Product>(OnItemSelected);

            ItemEdit = new Command<Product>(OnItemEdit);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Item.Clear();
                var items = await ProductDataStore.GetItemsAsync(true);
                foreach (var item in items)
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

        async void OnAddSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(AMenuAddPage)}");
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

            await Shell.Current.GoToAsync($"{nameof(AMenuDetailPage)}?{nameof(AMenuDetailViewModel.ItemId)}={item.Id}");
        }

        public Product EditItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemEdit(value);
            }
        }

        async void OnItemEdit(Product item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(AMenuEditPage)}?{nameof(AMenuDetailViewModel.ItemId)}={item.Id}");
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
