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
        private Item _selectedItem;
        public ObservableCollection<Item> Item { get; }
        public Command LoadItemCommand { get; }
        public Command<Item> ItemTapped { get; }
        public Command<Item> ItemEdit { get; }
        public DateTime Today { get; set; }

        public AMenuViewModel()
        {
            Title = "Menu";
            Today = DateTime.Today;
            Item = new ObservableCollection<Item>();
            
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            ItemEdit = new Command<Item>(OnItemEdit);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Item.Clear();
                var items = await DataStore.GetItemsAsync(true);
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

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(AMenuDetailPage)}?{nameof(AMenuDetailViewModel.ItemId)}={item.Id}");
        }

        public Item EditItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemEdit(value);
            }
        }

        async void OnItemEdit(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(AMenuEditPage)}?{nameof(AMenuDetailViewModel.ItemId)}={item.Id}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;
            searchTerm = searchTerm.ToLowerInvariant();

            await ExecuteLoadItemsCommand();

            var items = Item.Where(x => x.Drink.ToLowerInvariant().Contains(searchTerm)).ToList();

            foreach (var item in Item.ToList())
            {  
                if (!items.Contains(item))
                {
                    Item.Remove(item);
                }
                else if (!Item.Contains(item))
                {
                    Item.Add(item);
                }    
            }
        }
    }
}
