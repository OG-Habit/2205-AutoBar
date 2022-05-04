using AutoBarBar.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class BartenderViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Bartender { get; }
        public Command LoadBartenderCommand { get; }

        public BartenderViewModel()
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
    }
}
