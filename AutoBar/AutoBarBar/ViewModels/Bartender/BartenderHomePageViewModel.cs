using AutoBarBar.Models;
using AutoBarBar.Services;
using AutoBarBar.Views;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using static AutoBarBar.Constants;
using static AutoBarBar.DateTimeHelper;
using ZXing;
using System.Reflection;

namespace AutoBarBar.ViewModels
{
    public class BartenderHomePageViewModel : BaseViewModel, IQueryAttributable
    {
        readonly IProductService productService;
        readonly IActiveTabService activeTabService;
        readonly IOrderLineService orderLineService;
        readonly IToastService toastService;

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
            productService = DependencyService.Get<IProductService>();
            activeTabService = DependencyService.Get<IActiveTabService>();
            orderLineService = DependencyService.Get<IOrderLineService>();
            toastService = DependencyService.Get<IToastService>();

            Title = "Bartender Home Page";
            Customers = new ObservableRangeCollection<Customer>();
            Users = new ObservableRangeCollection<User>();
            Products = new ObservableRangeCollection<Product>();
            OrderLines = new ObservableRangeCollection<OrderLine>();
            Orders = new ObservableRangeCollection<Order>();
            Rewards = new ObservableRangeCollection<Reward>();
            ActiveTabs = new ObservableRangeCollection<ActiveTab>();

            NewOrderLines = new ObservableCollection<OrderLine>();
            
            PopulateData();

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
            var a = SelectedUser;
        }

        void PopulateData()
        {
            try
            {
                //List<int> orderIDs = new List<int>();
                var getRewardsTask = GetItemsAsync(Rewards, RewardDataStore);
                var productsTask = productService.GetProducts();
                var activeTabsTask = activeTabService.GetActiveTabs();
                _orderIDs = _customerIDs = string.Empty;

                Task[] tasks = new Task[]
                {
                getRewardsTask, productsTask, activeTabsTask
                };
                Task.WaitAll(tasks);

                ActiveTabs.AddRange(activeTabsTask.Result);
                Products.AddRange(productsTask.Result);

                foreach (var at in ActiveTabs)
                {
                    Customers.Add(at.ATCustomer);
                    Orders.Add(at.ATOrder);
                    Users.Add(at.ATUser);
                    _orderIDs += $",{at.ATOrder.ID}";
                    _customerIDs += $",{at.ATCustomer.ID}"; 
                }

                if(!string.IsNullOrEmpty(_orderIDs))
                {
                    _orderIDs = _orderIDs.Remove(0, 1);
                }
                if (!string.IsNullOrEmpty(_customerIDs))
                {
                    _customerIDs = _customerIDs.Remove(0, 1);
                }

                var orderLinesTask = orderLineService.GetOrderLines(_orderIDs);
                orderLinesTask.Wait();
                foreach(OrderLine ol in orderLinesTask.Result)
                {
                    ol.ProductName = _products.FirstOrDefault(p => p.ID == ol.ProductID).Name;
                    ol.SubTotal = ol.Quantity * ol.UnitPrice;
                    OrderLines.Add(ol);
                }
            }
            catch(Exception e)
            {
                var a = e.Message;
            }
        }

        async Task GetItemsAsync<TModel>(ObservableRangeCollection<TModel> list, IDataStore<TModel> dataStore)
        {
            var listFromDb = await dataStore.GetItemsAsync();
            list.AddRange(listFromDb);
        }

        async Task ShowScan()
        {
            await Shell.Current.GoToAsync($"//{nameof(BartenderHomePage)}/{nameof(ScanPage)}?{PARAM_CUSTOMER_IDS}={_customerIDs}");
        }
        
        async Task GetReloadBalanceAmount()
        {
            string ans = await Application.Current.MainPage.DisplayPromptAsync("Balance", "Enter amount:", "Add", "Cancel", null, -1, Keyboard.Numeric, "");
            if(decimal.TryParse(ans, out decimal num))
            {
                foreach(var c in _customers)
                {
                    if(c.ID == _selectedCustomer.ID)
                    {
                        await activeTabService.AddBalance(c.ID, c.Balance, GetPHTimeForDB());
                        await Application.Current.MainPage.DisplayAlert("Success", "Balance has been added.", "Ok");
                        c.Balance += num;
                    }
                }
            }
        }

        async Task EndTransaction()
        {
            await App.Current.MainPage.DisplayAlert("Success", "Customer transaction has ended.", "Ok");

            CurrentOrder.OrderStatus = 2;
            Orders.Add(CurrentOrder);
            Customers.Remove(SelectedCustomer);
        }

        async Task AddOrderLine()
        {
            Dictionary<string, string> phtime = GetPHTimeForBoth();
            string newOrderLinesStr = string.Empty;
            int tempPointsEarned;

            for (var i = _newOrderLines.Count - 1; i >= 0; i--)
            {
                _newOrderLines[i].CreatedOnForDB = phtime[KEY_DB];
                _newOrderLines[i].CreatedOnForUI = phtime[KEY_UI];
                OrderLines.Insert(0, _newOrderLines[i]);

                newOrderLinesStr += $@"({_newOrderLines[i].OrderID},{_newOrderLines[i].ProductID},{_newOrderLines[i].UnitPrice},{_newOrderLines[i].Quantity},{_newOrderLines[i].CreatedBy},""{_newOrderLines[i].CreatedOnForDB}"", 1),";                
            }
            newOrderLinesStr = newOrderLinesStr.Remove(newOrderLinesStr.Length-1);

            SelectedCustomer.Balance -= TotalOrderLinesCost;
            await orderLineService.AddOrderLines(newOrderLinesStr, _selectedCustomer.ID, _selectedCustomer.Balance);
            var group = from ol in _newOrderLines
                        group ol by ol.CreatedOnForUI into newOl
                        orderby newOl.Key descending
                        select newOl;
            CurrentOrderLineGroup.Insert(0, group.ElementAt(0));
            CanAddNewOrderLine = false;
            await App.Current.MainPage.DisplayAlert("Success", "You have successfully ordered.", "Thanks");

            NewOrderLines.Clear();

            CurrentOrder.TotalPrice += Convert.ToDouble(TotalOrderLinesCost);
            CurrentOrder.PointsEarned += (tempPointsEarned = (int)CurrentOrder.TotalPrice / 1000) != 0 ? tempPointsEarned * 10 : 0;
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

        void SwitchUser(object o)
        {
            if (o == null)
                return;

            User user = o as User;
            
            SelectedUser = user;
            SelectedCustomer = Customers.FirstOrDefault(c => c.UserID == user.ID);
            // Extend BaseModel
            CurrentOrder = Orders.First(order => order.CustomerID == SelectedCustomer.ID);
            CurrentOrderLines = new ObservableCollection<OrderLine>(OrderLines.Where(ol => ol.OrderID == CurrentOrder.ID));

            var group = from ol in CurrentOrderLines
                        group ol by ol.CreatedOnForUI into newGroup
                        orderby newGroup.Key descending
                        select newGroup;
            CurrentOrderLineGroup = new ObservableCollection<IGrouping<string, OrderLine>>(group);
            NewOrderLines.Clear();

            foreach (var colg in CurrentOrderLineGroup)
            {
                decimal total = 0;
                foreach (var ol in colg)
                {
                    total += ol.SubTotal;
                }
            }
        }

        void AddProductToOrderLine (Product p)
        {
            SelectedProduct = null;

            var newTotalCost = p.UnitPrice + TotalOrderLinesCost;
            if(newTotalCost > SelectedCustomer.Balance)
            {
                toastService.ShowLongMessage("Insufficient balance.");
                return;
            }

            int x;
            for(x = 0; x < NewOrderLines.Count && NewOrderLines[x].ProductID != p.ID; x++) { }
            if(x == NewOrderLines.Count)
            {
                CanAddNewOrderLine = true;
                NewOrderLines.Add(new OrderLine
                {
                    TempID = Guid.NewGuid().ToString(),
                    OrderID = CurrentOrder.ID,
                    ProductID = p.ID,
                    UnitPrice = p.UnitPrice,
                    Quantity = 1,
                    CreatedBy = StaffUser.StaffID,

                    CustomerName = SelectedUser.FirstName,
                    ProductName = p.Name,
                    ProductImgUrl = p.ImageLink,
                    SubTotal = p.UnitPrice
                }) ;
                TotalOrderLinesCost = newTotalCost;
            } 
            else
            {
                toastService.ShowShortMessage($"{p.Name} is already in the order.");
            }
        }

        void IncreaseQuantity(OrderLine Ol)
        {
            var newTotal = TotalOrderLinesCost + Ol.UnitPrice;
            if (newTotal > SelectedCustomer.Balance)
            {
                toastService.ShowLongMessage("Insufficient balance.");
                return;
            }

            foreach(var nol in NewOrderLines)
            {
                if (nol.TempID == Ol.TempID)
                {
                    nol.Quantity++;
                    nol.SubTotal += Ol.UnitPrice;
                    TotalOrderLinesCost = newTotal;
                    break;
                }
            }
        }

        void DecreaseQuantity(OrderLine Ol)
        {
            foreach(var nol in NewOrderLines)
            {
                if(nol.TempID == Ol.TempID)
                {
                    if(--nol.Quantity == 0)
                    {   
                        NewOrderLines.Remove(nol);
                        if (NewOrderLines.Count == 0 && CanAddNewOrderLine == true)
                            CanAddNewOrderLine = false;
                    } else
                    {
                        nol.SubTotal -= nol.UnitPrice;
                    }

                    TotalOrderLinesCost -= Ol.UnitPrice;
                    break;
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.Count == 0)
            {
                return;
            }

            if (query.ContainsKey($"{PARAM_USER}"))
            {
                string user = HttpUtility.UrlDecode(query[$"{PARAM_USER}"]);
                StaffUser = JsonConvert.DeserializeObject<User>(Uri.UnescapeDataString(user));
            }

            if(query.ContainsKey($"{PARAM_NEW_TAB}"))
            {
                string at = HttpUtility.UrlDecode(query[$"{PARAM_NEW_TAB}"]);
                ActiveTab activeTab = JsonConvert.DeserializeObject<ActiveTab>(Uri.UnescapeDataString(at));

                Customers.Add(activeTab.ATCustomer);
                Orders.Add(activeTab.ATOrder);
                Users.Add(activeTab.ATUser);
                _orderIDs += $",{activeTab.ATOrder.ID}";
                _customerIDs += $",{activeTab.ATCustomer.ID}";
            }
        }

        #region Getters setters
        #region Customers
        ObservableRangeCollection<Customer> _customers;
        public ObservableRangeCollection<Customer> Customers
        {
            get { return _customers; }
            set { SetProperty(ref _customers, value); }
        }

        Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { SetProperty(ref _selectedCustomer, value); }
        }

        string _customerIDs;
        public string CustomerIDs
        {
            get => _customerIDs;
            set => SetProperty(ref _customerIDs, value);
        }
        #endregion

        #region Users
        ObservableRangeCollection<User> _users;
        public ObservableRangeCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(ref _selectedUser, value); }
        }

        User _staffUser;
        public User StaffUser { 
            get => _staffUser;
            set => SetProperty(ref _staffUser, value);   
        }
        #endregion

        #region Products
        ObservableRangeCollection<Product> _products;
        public ObservableRangeCollection<Product> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }
        #endregion

        #region Orders
        ObservableRangeCollection<Order> _orders;
        public ObservableRangeCollection<Order> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }

        Order _currentOrder;
        public Order CurrentOrder
        {
            get { return _currentOrder; }
            set { SetProperty(ref _currentOrder, value); }
        }

        string _orderIDs;
        public string OrderIDs
        {
            get => _orderIDs;
            set => SetProperty(ref _orderIDs, value);
        }
        #endregion

        #region OrderLines
        ObservableRangeCollection<OrderLine> _orderLines;
        public ObservableRangeCollection<OrderLine> OrderLines
        {
            get { return _orderLines; }
            set { SetProperty(ref _orderLines, value); }
        }

        ObservableCollection<OrderLine> _currentOrderLines;
        public ObservableCollection<OrderLine> CurrentOrderLines
        {
            get { return _currentOrderLines; }
            set { SetProperty(ref _currentOrderLines, value); }
        }

        ObservableCollection<OrderLine> _newOrderLines;
        public ObservableCollection<OrderLine> NewOrderLines
        {
            get => _newOrderLines;
            set => SetProperty(ref _newOrderLines, value);
        }

        ObservableCollection<IGrouping<string, OrderLine>> _currentOrderLineGroup;
        public ObservableCollection<IGrouping<string, OrderLine>> CurrentOrderLineGroup
        {
            get { return _currentOrderLineGroup; }
            set { SetProperty(ref _currentOrderLineGroup, value); }
        }

        bool _canAddNewOrderLine;
        public bool CanAddNewOrderLine
        {
            get => _canAddNewOrderLine;
            set => SetProperty(ref _canAddNewOrderLine, value);
        }

        decimal _totalOrderLinesCost;
        public decimal TotalOrderLinesCost
        {
            get => _totalOrderLinesCost;
            set => SetProperty(ref _totalOrderLinesCost, value);
        }
        #endregion

        #region Rewards
        ObservableRangeCollection<Reward> _rewards;
        public ObservableRangeCollection<Reward> Rewards
        {
            get { return _rewards; }
            set { SetProperty(ref _rewards, value); }
        }

        Reward _selectedReward;
        public Reward SelectedReward
        {
            get { return _selectedReward; }
            set { SetProperty(ref _selectedReward, value); }
        }
        #endregion

        #region ActiveTabs
        ObservableRangeCollection<ActiveTab> _activeTabs;
        public ObservableRangeCollection<ActiveTab> ActiveTabs
        {
            get => _activeTabs;
            set => SetProperty(ref _activeTabs, value);
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
