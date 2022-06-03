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
        private Bartender _selectedBartender;
        public ObservableCollection<Bartender> Bartender { get; }
        public Command LoadBartenderCommand { get; }
        public Command BartenderAdd { get; }
        public Command<Bartender> BartenderTapped { get; }
        public Command<Bartender> BartenderEdit { get; }
        public DateTime Today { get; set; }

        public ABartenderViewModel()
        {
            Title = "Bartender";
            Today = DateTime.Today;
            Bartender = new ObservableCollection<Bartender>();
            
            LoadBartenderCommand = new Command(async () => await ExecuteLoadItemsCommand());

            BartenderAdd = new Command(OnAddSelected);

            BartenderTapped = new Command<Bartender>(OnBartenderSelected);

            BartenderEdit = new Command<Bartender>(OnEditSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Bartender.Clear();
                var items = await BartenderDataStore.GetItemsAsync(true);
                foreach (var item in items.OrderBy(x => x.LastName))
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

        async void OnAddSelected()
        {
            await Shell.Current.GoToAsync($"{nameof(ABartenderAddPage)}");
        }

        public Bartender SelectedBartender
        {
            get => _selectedBartender;
            set
            {
                SetProperty(ref _selectedBartender, value);
                OnBartenderSelected(value);
            }
        }

        async void OnBartenderSelected(Bartender item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ABartenderDetailPage)}?{nameof(ABartenderDetailViewModel.ItemId)}={item.Id}");
        }

        public Bartender EditBartender
        {
            get => _selectedBartender;
            set
            {
                SetProperty(ref _selectedBartender, value);
                OnEditSelected(value);
            }
        }

        async void OnEditSelected(Bartender item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ABartenderEditPage)}?{nameof(ABartenderDetailViewModel.ItemId)}={item.Id}");
        }

        public async void SearchBar_Change(object sender, TextChangedEventArgs e)
        {
            var searchTerm = e.NewTextValue;

            try
            {
                Bartender.Clear();
                var items = await BartenderDataStore.GetSearchResults(searchTerm);
                foreach (var item in items)
                {
                    Bartender.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
