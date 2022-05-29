using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ZXing;
using Newtonsoft.Json;
using AutoBarBar.Services;
using static AutoBarBar.Constants;
using System.Threading.Tasks;
using static AutoBarBar.DateTimeHelper;

namespace AutoBarBar.ViewModels
{
    public class ScanPageViewModel : BaseViewModel, IQueryAttributable
    {
        IActiveTabService activeTabService;

        public Command ScanCommand { get; }

        public ScanPageViewModel()
        {
            activeTabService = DependencyService.Get<IActiveTabService>();

            ScanResult = "Result...";

            ScanCommand = new Command<Result>(Scan);
        }

        void Scan(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                ScanResult = result.Text;

                ActiveTab at = await activeTabService.CreateActiveTab(result.Text, customerIDs, GetPHTimeForDB());
                if(at == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Customer is already part of the tab system.", "Ok");
                    IsBusy = false;
                    return;
                }

                string obj = JsonConvert.SerializeObject(at);

                await App.Current.MainPage.DisplayAlert("Success", $"{at.ATUser.FullName} has been added.", "Ok");
                IsBusy = false;
                await Shell.Current.GoToAsync($"..?{PARAM_NEW_TAB}={obj}");
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.Count == 0)
            {
                return;
            }

            customerIDs = query[$"{PARAM_CUSTOMER_IDS}"];
        }

        string customerIDs;

        string scanResult;
        public string ScanResult
        {
            get => scanResult;
            set => SetProperty(ref scanResult, value);
        }
    }
}
