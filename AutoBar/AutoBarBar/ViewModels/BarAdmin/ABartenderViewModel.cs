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
    public class ABartenderViewModel : BaseViewModel
    {
        private Item _selectedBartender;
        public ObservableCollection<Item> Bartender { get; }
        public Command LoadBartenderCommand { get; }
        public Command<Item> BartenderTapped { get; }
        public Command<Item> BartenderEdit { get; }
        public DateTime Today { get; set; }

        public ABartenderViewModel()
        {
            Title = "Bartender";
            Today = DateTime.Today;
            Bartender = new ObservableCollection<Item>();
            
            LoadBartenderCommand = new Command(async () => await ExecuteLoadItemsCommand());

            BartenderTapped = new Command<Item>(OnBartenderSelected);

            BartenderEdit = new Command<Item>(OnEditSelected);
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

        public Item SelectedBartender
        {
            get => _selectedBartender;
            set
            {
                SetProperty(ref _selectedBartender, value);
                OnBartenderSelected(value);
            }
        }

        async void OnBartenderSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ABartenderDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ABartenderDetailPage)}?{nameof(ABartenderDetailViewModel.ItemId)}={item.Id}");
        }

        public Item EditBartender
        {
            get => _selectedBartender;
            set
            {
                SetProperty(ref _selectedBartender, value);
                OnEditSelected(value);
            }
        }

        async void OnEditSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ABartenderEditPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ABartenderEditPage)}?{nameof(ABartenderDetailViewModel.ItemId)}={item.Id}");
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
