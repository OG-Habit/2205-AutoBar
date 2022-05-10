using AutoBarBar.Models;
using AutoBarBar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class BartenderHomePageViewModel : BaseViewModel
    {
        public ICommand SwitchUserCommand { get; }
        public ICommand ShowScanCommand { get; }
        public ICommand GetReloadBalanceAmountCommand { get; }

        public BartenderHomePageViewModel()
        {
            Title = "Bartender Home Page";
            Customers = new ObservableCollection<Customer>();
            Products = new ObservableCollection<Product>();
            OrderLines = new ObservableCollection<OrderLine>();
            Orders = new ObservableCollection<Order>();

            PopulateData();
            SwitchUser(Customers[1]);

            SwitchUserCommand = new Command<object>(SwitchUser);
            ShowScanCommand = new Command(ShowScan);
            GetReloadBalanceAmountCommand = new Command(GetReloadBalanceAmount);
        }

        async void PopulateData()
        {
            Customers.Clear();
            var customers = await CustomerDataStore.GetItemsAsync();
            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }

            Products.Clear();
            var products = await ProductDataStore.GetItemsAsync();
            foreach (var product in products)
            {
                Products.Add(product);
            }

            OrderLines.Clear();
            var orderlines = await OrderLineDataStore.GetItemsAsync();
            foreach (var a in orderlines)
            {
                OrderLines.Add(a);
            }

            Orders.Clear();
            var o = await OrderDataStore.GetItemsAsync();
            foreach (var a in o)
            {
                Orders.Add(a);
            }
        } 

        void SwitchUser(object c)
        {
            if (c == null)
                return;
                
            SelectedCustomer = c as Customer;
            CurrentOrderLine = new ObservableCollection<OrderLine>(OrderLines.Where(o => o.CustomerName == SelectedCustomer.Name));
            CurrentOrder = Orders.First(o => String.Equals(o.CustomerName, SelectedCustomer.Name));
        }

        async void ShowScan()
        {
            await Shell.Current.GoToAsync($"{nameof(ScanPage)}");
        }

        async void GetReloadBalanceAmount()
        {
            Customer temp;
            string ans = await Application.Current.MainPage.DisplayPromptAsync("Balance", "Enter amount:", "Add", "Cancel", null, -1, Keyboard.Numeric, "");
            if(float.TryParse(ans, out float num))
            {
                foreach(var c in Customers)
                {
                    if(c.Name == SelectedCustomer.Name)
                    {
                        c.CurrentBalance += num;
                        await Application.Current.MainPage.DisplayAlert("Success", "Balance has been added.", "Ok");
                        temp = SelectedCustomer;
                        selectedCustomer = null;
                        SelectedCustomer = temp;
                    }
                }
            }
        }

        #region Getters setters
        ObservableCollection<Customer> customers;
        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set { SetProperty(ref customers, value); }
        }

        Customer selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { SetProperty(ref selectedCustomer, value); }
        }

        ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }

        ObservableCollection<Order> orders;
        public ObservableCollection<Order> Orders
        {
            get { return orders; }
            set { SetProperty(ref orders, value); }
        }

        Order currentOrder;
        public Order CurrentOrder
        {
            get { return currentOrder; }
            set { SetProperty(ref currentOrder, value); }
        }

        ObservableCollection<OrderLine> orderLines;
        public ObservableCollection<OrderLine> OrderLines
        {
            get { return orderLines; }
            set { SetProperty(ref orderLines, value); }
        }

        ObservableCollection<OrderLine> currentOrderLine;
        public ObservableCollection<OrderLine> CurrentOrderLine
        {
            get { return currentOrderLine; }
            set { SetProperty(ref currentOrderLine, value); }
        }
        #endregion
    }
}
