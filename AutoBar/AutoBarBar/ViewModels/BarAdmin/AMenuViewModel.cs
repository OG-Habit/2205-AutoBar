using AutoBarBar.Models;
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
        public ObservableCollection<Item> Item { get; }
        public Command LoadItemCommand { get; }

        public AMenuViewModel()
        {
            Title = "Menu";
            Item = new ObservableCollection<Item>();
            LoadItemCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
