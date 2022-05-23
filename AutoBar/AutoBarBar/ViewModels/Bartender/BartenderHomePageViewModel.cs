using AutoBarBar.Models;
using AutoBarBar.Services;
using AutoBarBar.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class BartenderHomePageViewModel : BaseViewModel
    {
        public AsyncCommand GetReloadBalanceAmountCommand { get; }
        public AsyncCommand ShowScanCommand { get; }
        public AsyncCommand EndTransactionCommand { get; }
        public AsyncCommand AddOrderLineCommand { get; }
        public AsyncCommand<string> SearchProductCommand { get; }
        public AsyncCommand<string> SearchCustomerCommand { get; }

        public Command SwitchUserCommand { get; }
        public Command AddProductToOrderLineCommand { get; }
        public Command IncreaseQuantityCommand { get; }
        public Command DecreaseQuantityCommand { get; }

        public ICommand Test { get; }

        Reward DummyReward = new Reward()
        {
            Id = "dummy",
            Name = "-- None --"
        };
        public string[] times = { "7:30PM", "8:30PM", "10:30PM" };

        public BartenderHomePageViewModel()
        {
            Title = "Bartender Home Page";
            Customers = new ObservableRangeCollection<Customer>();
            Products = new ObservableRangeCollection<Product>();
            OrderLines = new ObservableRangeCollection<OrderLine>();
            Orders = new ObservableRangeCollection<Order>();
            Rewards = new ObservableRangeCollection<Reward>();

            NewOrderLines = new ObservableCollection<OrderLine>();
            
            PopulateData();
            SwitchUser(Customers[0]);

            ShowScanCommand = new AsyncCommand(ShowScan);
            GetReloadBalanceAmountCommand = new AsyncCommand(GetReloadBalanceAmount);
            EndTransactionCommand = new AsyncCommand(EndTransaction);
            AddOrderLineCommand = new AsyncCommand(AddOrderLine);
            SearchCustomerCommand = new AsyncCommand<string>(SearchCustomer);
            SearchProductCommand = new AsyncCommand<string>(SearchProduct);
            SwitchUserCommand = new Command<object>(SwitchUser);
            AddProductToOrderLineCommand = new Command<Product>(AddProductToOrderLine);
            IncreaseQuantityCommand = new Command<OrderLine>(IncreaseQuantity);
            DecreaseQuantityCommand = new Command<OrderLine>(DecreaseQuantity);

            SelectedProduct = null;
            TotalOrderLinesCost = 0;
            CanAddNewOrderLine = false;
            SelectedReward = DummyReward;

            Test = new Command(TestMe);
        }

        void TestMe()
        {
            var a = SelectedCustomer;
        }

        void PopulateData()
        {
            var getCustomersTask = GetItemsAsync(Customers, CustomerDataStore);
            var getProductsTask = GetItemsAsync(Products, ProductDataStore);
            var getOrderLinesTask = GetItemsAsync(OrderLines, OrderLineDataStore);
            var getOrdersTask = GetItemsAsync(Orders, OrderDataStore);
            var getRewardsTask = GetItemsAsync(Rewards, RewardDataStore);
            Task[] tasks = new Task[]
            {
                getCustomersTask, getProductsTask, getOrderLinesTask, getOrdersTask, getRewardsTask
            };
            //Task.WaitAll(tasks);
            Task.WhenAll(tasks).Await();
        }

        async Task GetItemsAsync<TModel>(ObservableRangeCollection<TModel> list, IDataStore<TModel> dataStore)
        {
            var listFromDb = await dataStore.GetItemsAsync();
            list.AddRange(listFromDb);
        }

        async Task ShowScan()
        {
            await Shell.Current.GoToAsync($"{nameof(ScanPage)}");
        }
        
        async Task GetReloadBalanceAmount()
        {
            string ans = await Application.Current.MainPage.DisplayPromptAsync("Balance", "Enter amount:", "Add", "Cancel", null, -1, Keyboard.Numeric, "");
            if(float.TryParse(ans, out float num))
            {
                foreach(var c in Customers)
                {
                    if(c.Name == SelectedCustomer.Name)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Balance has been added.", "Ok");
                        CurrentBalance = c.CurrentBalance += num;
                    }
                }
            }
        }

        async Task EndTransaction()
        {
            await App.Current.MainPage.DisplayAlert("Success", "Customer transaction has ended.", "Ok");

            CurrentOrder.OrderStatus = true;
            Orders.Add(CurrentOrder);
            Customers.Remove(SelectedCustomer);
        }

        async Task AddOrderLine()
        {
            await App.Current.MainPage.DisplayAlert("Success", "You have successfully ordered.", "Thanks");
            string time = "9:45PM";
            int pe;
            var group = from ol in NewOrderLines
                        group ol by time into newOl
                        select newOl;
            CurrentOrderLineGroup.Add(group.ElementAt(0));
            OrderLines.AddRange(NewOrderLines);
            NewOrderLines.Clear();

            CurrentBalance = SelectedCustomer.CurrentBalance -= TotalOrderLinesCost;
            TotalOrderPrice = CurrentOrder.TotalPrice += TotalOrderLinesCost;
            PointsEarned = CurrentOrder.PointsEarned += (pe = (int)CurrentOrder.TotalPrice / 1000) != 0 ? pe * 100 : 0;
            TotalOrderLinesCost = 0;
        }

        async Task SearchProduct(string arg)
        {
            await SearchItemsAsync<Product>(Products, ProductDataStore, arg.ToLowerInvariant());
        }

        async Task SearchCustomer(string arg)
        {
            await SearchItemsAsync<Customer>(Customers, CustomerDataStore, arg.ToLowerInvariant());
        }

        async Task SearchItemsAsync<TModel>(ObservableRangeCollection<TModel> List, IDataStore<TModel> dataStore, string arg)
        {
            var list = await dataStore.GetSearchResults(arg);
            List.ReplaceRange(list);
        }

        void SwitchUser(object c)
        {
            if (c == null)
                return;

            SelectedCustomer = c as Customer;
            CurrentBalance = selectedCustomer.CurrentBalance;
            CurrentOrderLines = new ObservableCollection<OrderLine>(OrderLines.Where(ol => ol.CustomerName == SelectedCustomer.Name));
            CurrentOrder = Orders.First(o => String.Equals(o.CustomerName, SelectedCustomer.Name) &&
                                            o.OrderStatus == false);
            PointsEarned = CurrentOrder.PointsEarned;
            TotalOrderPrice = CurrentOrder.TotalPrice;

            var group = from ol in CurrentOrderLines
                        group ol by ol.CreatedOn into newGroup
                        orderby newGroup.Key
                        select newGroup;
            CurrentOrderLineGroup = new ObservableCollection<IGrouping<string, OrderLine>>(group);
            NewOrderLines.Clear();
            foreach (var colg in CurrentOrderLineGroup)
            {
                double total = 0;
                foreach (var ol in colg)
                {
                    total += ol.SubTotal;
                }
            }
        }

        void AddProductToOrderLine (Product p)
        {
            SelectedProduct = null;

            if (CanAddNewOrderLine == false)
                CanAddNewOrderLine = true;

            var newTotalCost = p.Price + TotalOrderLinesCost;
            if(newTotalCost > CurrentBalance)
            {
                DependencyService.Get<IToastService>().ShowLongMessage("Insufficient balance.");
                return;
            }

            int x;
            for(x = 0; x < NewOrderLines.Count && NewOrderLines[x].ProductName != p.Name; x++) { }
            if(x == NewOrderLines.Count)
            {
                NewOrderLines.Add(new OrderLine
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerName = SelectedCustomer.Name,
                    ProductName = p.Name,
                    Price = p.Price,
                    Quantity = 1,
                    ProductImgUrl = p.ImageLink,
                    CreatedOn = "9:10PM",
                    SubTotal = p.Price
                });
                TotalOrderLinesCost = (float)newTotalCost;
            } 
            else
            {
                DependencyService.Get<IToastService>().ShowShortMessage($"{p.Name} is already in the order.");
            }
        }

        void IncreaseQuantity(OrderLine Ol)
        {
            var newTotal = TotalOrderLinesCost + Ol.Price;
            if (newTotal > SelectedCustomer.CurrentBalance)
            {
                DependencyService.Get<IToastService>().ShowLongMessage("Insufficient balance.");
                return;
            }

            foreach(var nol in NewOrderLines)
            {
                if (string.Equals(Ol.Id, nol.Id))
                {
                    nol.Quantity++;
                    nol.SubTotal += Ol.Price;
                    TotalOrderLinesCost = (float)newTotal;
                    break;
                }
            }
        }

        void DecreaseQuantity(OrderLine Ol)
        {
            foreach(var nol in NewOrderLines)
            {
                if(string.Equals(Ol.Id, nol.Id))
                {
                    if(--nol.Quantity == 0)
                    {   
                        NewOrderLines.Remove(nol);
                        if (NewOrderLines.Count == 0 && CanAddNewOrderLine == true)
                            CanAddNewOrderLine = false;
                    } else
                    {
                        nol.SubTotal -= nol.Price;
                    }

                    TotalOrderLinesCost -= (float)Ol.Price;
                    break;
                }
            }
        }

        #region Getters setters
        #region Customers
        ObservableRangeCollection<Customer> customers;
        public ObservableRangeCollection<Customer> Customers
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

        double currentBalance;
        public double CurrentBalance
        {
            get => currentBalance;
            set => SetProperty(ref currentBalance, value);
        }
        #endregion

        #region Products
        ObservableRangeCollection<Product> products;
        public ObservableRangeCollection<Product> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }

        Product selectedProduct;
        public Product SelectedProduct
        {
            get => selectedProduct;
            set => SetProperty(ref selectedProduct, value);
        }
        #endregion

        #region Orders
        ObservableRangeCollection<Order> orders;
        public ObservableRangeCollection<Order> Orders
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

        double totalOrderPrice;
        public double TotalOrderPrice
        {
            get => totalOrderPrice;
            set => SetProperty(ref totalOrderPrice, value);
        }

        int pointsEarned;
        public int PointsEarned
        {
            get => pointsEarned;
            set => SetProperty(ref pointsEarned, value);
        }
        #endregion

        #region OrderLines
        ObservableRangeCollection<OrderLine> orderLines;
        public ObservableRangeCollection<OrderLine> OrderLines
        {
            get { return orderLines; }
            set { SetProperty(ref orderLines, value); }
        }

        ObservableCollection<OrderLine> currentOrderLines;
        public ObservableCollection<OrderLine> CurrentOrderLines
        {
            get { return currentOrderLines; }
            set { SetProperty(ref currentOrderLines, value); }
        }

        ObservableCollection<OrderLine> newOrderLines;
        public ObservableCollection<OrderLine> NewOrderLines
        {
            get => newOrderLines;
            set => SetProperty(ref newOrderLines, value);
        }

        ObservableCollection<IGrouping<string, OrderLine>> currentOrderLineGroup;
        public ObservableCollection<IGrouping<string, OrderLine>> CurrentOrderLineGroup
        {
            get { return currentOrderLineGroup; }
            set { SetProperty(ref currentOrderLineGroup, value); }
        }

        bool canAddNewOrderLine;
        public bool CanAddNewOrderLine
        {
            get => canAddNewOrderLine;
            set => SetProperty(ref canAddNewOrderLine, value);
        }

        float totalOrderLinesCost;
        public float TotalOrderLinesCost
        {
            get => totalOrderLinesCost;
            set => SetProperty(ref totalOrderLinesCost, value);
        }
        #endregion

        #region Rewards
        ObservableRangeCollection<Reward> rewards;
        public ObservableRangeCollection<Reward> Rewards
        {
            get { return rewards; }
            set { SetProperty(ref rewards, value); }
        }

        Reward selectedReward;
        public Reward SelectedReward
        {
            get { return selectedReward; }
            set { SetProperty(ref selectedReward, value); }
        }
        #endregion
        #endregion

        #region Singleton
        static BartenderHomePageViewModel instance;
        public static BartenderHomePageViewModel Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = new BartenderHomePageViewModel();
                }
                return instance;
            }
        }
        #endregion
    }
}
