using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ZXing;
using Newtonsoft.Json;
using AutoBarBar.Services;
using System.Threading.Tasks;

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
            NewTab = new ActiveTab();
            NewTab.ATOrder = new Order();
            NewTab.ATUser = new User();
            NewTab.ATCustomer = new Customer();
            NewTab.ATUser.FullName = "It workis.";

            ScanCommand = new Command<Result>(Scan);
        }

        void Scan(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                ScanResult = result.Text;

                if(activeTabService.CheckExistingTabs(customerIDs).Result)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Customer is already part of the tab system.", "Ok");
                    return;
                }

                ActiveTab at = await activeTabService.CreateActiveTab(result.Text);
                string obj = JsonConvert.SerializeObject(at);

                await App.Current.MainPage.DisplayAlert("Success", "User has been added.", "Ok");
                await Shell.Current.GoToAsync($"..?newTab={obj}");
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.Count == 0)
            {
                return;
            }

            customerIDs = query["ids"];
        }

        string scanResult;
        public string ScanResult
        {
            get => scanResult;
            set => SetProperty(ref scanResult, value);
        }

        string customerIDs;

        ActiveTab newTab;
        public ActiveTab NewTab
        {
            get => newTab;
            set => SetProperty(ref newTab, value);
        }
    }
}
