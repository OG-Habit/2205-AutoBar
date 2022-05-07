using AutoBarBar.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class ABartenderViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Bartender { get; }
        public Command LoadBartenderCommand { get; }

        public ABartenderViewModel()
        {
            Title = "Bartender";
            Bartender = new ObservableCollection<Item>();
            LoadBartenderCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Bartender.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Bartender.Add(item);
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

            var items = Bartender.Where(x => x.B_Name.ToLowerInvariant().Contains(searchTerm)).ToList();

            foreach (var item in Bartender.ToList())
            {
                if (!items.Contains(item))
                {
                    Bartender.Remove(item);
                }
                else if (!Bartender.Contains(item))
                {
                    Bartender.Add(item);
                }
            }
        }
    }
}
