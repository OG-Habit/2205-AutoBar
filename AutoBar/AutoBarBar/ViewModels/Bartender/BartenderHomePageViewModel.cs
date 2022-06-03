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
        IProductService productService;
        IActiveTabService activeTabService;
        IOrderLineService orderLineService;
        IToastService toastService;
        IRewardService rewardService;

        public AsyncCommand GetReloadBalanceAmountCommand { get; }
        public AsyncCommand ShowScanCommand { get; }
        public AsyncCommand EndTransactionCommand { get; }
        public AsyncCommand AddOrderLineCommand { get; }
        public AsyncCommand RefreshTabCommand { get; }

        public Command<string> SearchProductCommand { get; }
        public Command<string> SearchCustomerCommand { get; }
        public Command SwitchUserCommand { get; }
        public Command AddProductToOrderLineCommand { get; }
        public Command IncreaseQuantityCommand { get; }
        public Command DecreaseQuantityCommand { get; }
        public Command CheckRewardCommand { get; }

        public ICommand Test { get; }

        Reward DummyReward = new Reward()
        {
            ID = -1,
            Name = "-- None --",
            Points = 0
        };
        Reward previousSelectedReward;
        string customerIDs, orderIDs;
        private BartenderHomePageViewModel()
        {
            productService = DependencyService.Get<IProductService>();
            activeTabService = DependencyService.Get<IActiveTabService>();
            orderLineService = DependencyService.Get<IOrderLineService>();
            toastService = DependencyService.Get<IToastService>();
            rewardService = DependencyService.Get<IRewardService>();

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
            RefreshTabCommand = new AsyncCommand(RefreshTab);

            SearchCustomerCommand = new Command<string>(SearchCustomer);
            SearchProductCommand = new Command<string>(SearchProduct);
            SwitchUserCommand = new Command<object>(SwitchUser);
            AddProductToOrderLineCommand = new Command<Product>(AddProductToOrderLine);
            IncreaseQuantityCommand = new Command<OrderLine>(IncreaseQuantity);
            DecreaseQuantityCommand = new Command<OrderLine>(DecreaseQuantity);
            CheckRewardCommand = new Command<Reward>(CheckReward);

            Rewards.Add(DummyReward);
            SelectedProduct = null;
            TotalOrderLinesCost = 0;
            CanAddNewOrderLine = IsEmpty = false;
            previousSelectedReward = SelectedReward = DummyReward;
            Date = DateTime.UtcNow.AddHours(8).ToString("MMM dd, yyyy");

            Test = new Command(TestMe);
        }

        void TestMe()
        {
            
        }

        void PopulateData()
        {
            try
            {
                var rewardsTask = rewardService.GetRewards();
                var productsTask = productService.GetProducts();
                var activeTabsTask = activeTabService.GetActiveTabs();

                Task[] tasks = new Task[]
                {
                rewardsTask, productsTask, activeTabsTask
                };
                Task.WaitAll(tasks);

                ActiveTabs.AddRange(activeTabsTask.Result);
                Products.AddRange(productsTask.Result.OrderBy(o => o.Name));
                Rewards.AddRange(rewardsTask.Result);

                PopulateModelsFromActiveTabs();

                AllUsers = Users;
                AllProducts = Products;
            }
            catch(Exception e)
            {
                var a = e.Message; Debug.WriteLine(a);
            }
        }

        async Task RefreshTab()
        {
            IsBusy = true;

            var activeTabs = await activeTabService.GetActiveTabs();
            ClearSelectedProperties();
            Users.Clear();
            Customers.Clear();
            Orders.Clear();
            OrderLines.Clear();
            ActiveTabs.ReplaceRange(activeTabs);
            PopulateModelsFromActiveTabs();
            AllUsers = Users;

            IsBusy = false;
        }

        async Task ShowScan()
        {
            await Shell.Current.GoToAsync($"//{nameof(BartenderHomePage)}/{nameof(ScanPage)}?{PARAM_CUSTOMER_IDS}={customerIDs}");
        }
        
        async Task GetReloadBalanceAmount()
        {
            string ans = await Application.Current.MainPage.DisplayPromptAsync("Balance", "Enter amount:", "Add", "Cancel", "Amount to be added...", -1, Keyboard.Numeric, "");
            if (string.IsNullOrEmpty(ans))
            {
                return;
            }

            if (!decimal.TryParse(ans, out decimal num) || num <= 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please input numbers that are greater than 0 only.", "Ok");
                return;
            }
            
            bool confirm = await Application.Current.MainPage.DisplayAlert("Warning", $"Are you sure you want to add PHP {num} to {_selectedUser.FullName}'s balance", "Yes", "No");
            if(!confirm)
            {
                return;
            }
            
            foreach(var c in _customers)
            {
                if(c.ID == _selectedCustomer.ID)
                {
                    await activeTabService.AddBalance(c.ID, StaffUser.StaffID, num, GetPHTimeForDB());
                    await Application.Current.MainPage.DisplayAlert("Success", "Balance has been added.", "Ok");
                    c.Balance += num;
                    break;
                }
            }
        }

        async Task EndTransaction()
        {
            bool ans = await Application.Current.MainPage.DisplayAlert("Warning", $"Are you sure you want to remove {_selectedUser.FullName} from the tab system?", "Yes", "No");
            if (ans == false)
            {
                return;
            }

            int hasReward = _selectedReward.ID == -1 ? 0 : 1;

            await activeTabService.CloseTab(_selectedCustomer.ID, _selectedOrder, _selectedReward, hasReward, GetPHTimeForDB());
            RemoveIDFromStr(ref customerIDs, _selectedCustomer.ID.ToString());
            RemoveIDFromStr(ref orderIDs, _selectedOrder.ID.ToString());
            Users.Remove(_selectedUser);
            Customers.Remove(_selectedCustomer);
            Orders.Remove(_selectedOrder);
            OrderLines.RemoveRange(_currentOrderLines);
            ClearSelectedProperties();
            await App.Current.MainPage.DisplayAlert("Success", "Customer transaction has ended.", "Ok");
        }

        async Task AddOrderLine()
        {
            bool ans = await Application.Current.MainPage.DisplayAlert("Warning", "Are you sure you want to add these orders?", "Yes", "No");
            if(ans == false)
            {
                return;
            }

            Dictionary<string, string> phtime = GetPHTimeForBoth();
            string newOrderLinesStr = string.Empty;
            decimal tempVal;
            int tempPointsEarned;

            _selectedOrder.CostTracker += TotalOrderLinesCost;
            if ((tempVal = Math.Truncate(_selectedOrder.CostTracker / 1000)) > 0)
            {
                tempPointsEarned = (int)tempVal * 10;
                _selectedOrder.CostTracker %= 1000;
            } else
            {
                tempPointsEarned = 0;
            }

            for (var i = _newOrderLines.Count - 1; i >= 0; i--)
            {
                _newOrderLines[i].CreatedOnForDB = phtime[KEY_DB];
                _newOrderLines[i].CreatedOnForUI = phtime[KEY_UI];
                OrderLines.Insert(0, _newOrderLines[i]);

                newOrderLinesStr += $@"({_newOrderLines[i].OrderID},{_newOrderLines[i].ProductID},{_newOrderLines[i].UnitPrice},{_newOrderLines[i].Quantity},{_newOrderLines[i].CreatedBy},""{_newOrderLines[i].CreatedOnForDB}"", 1),";                
            }
            newOrderLinesStr = newOrderLinesStr.Remove(newOrderLinesStr.Length-1);

            await orderLineService.AddOrderLines(newOrderLinesStr, _selectedCustomer.ID, _selectedOrder.ID,  _totalOrderLinesCost, tempPointsEarned, GetPHTimeForDB());

            SelectedCustomer.Balance -= _totalOrderLinesCost;
            SelectedCustomer.Points += tempPointsEarned;
            var group = from ol in _newOrderLines
                        group ol by ol.CreatedOnForUI into newOl
                        orderby newOl.Key descending
                        select newOl;
            CurrentOrderLineGroup.Insert(0, group.ElementAt(0));
            CanAddNewOrderLine = false;

            await App.Current.MainPage.DisplayAlert("Success", "You have successfully ordered.", "Thanks");

            NewOrderLines.Clear();

            SelectedOrder.TotalPrice += Convert.ToDouble(_totalOrderLinesCost);
            SelectedOrder.PointsEarned += tempPointsEarned;
            TotalOrderLinesCost = 0;
        }

        void PopulateModelsFromActiveTabs()
        {
            orderIDs = customerIDs = string.Empty;
            foreach (var at in ActiveTabs)
            {
                string atFName = at.ATUser.FullName;
                int usersCount = Users.Count, x;
                at.ATOrder.CostTracker = (decimal)at.ATOrder.TotalPrice % 1000;
                Customers.Add(at.ATCustomer);
                Orders.Add(at.ATOrder);
                for (x = 0; x < usersCount; x++)
                {
                    if (string.Compare(atFName, _users[x].FullName) < 0)
                    {
                        Users.Insert(x, at.ATUser);
                        break;
                    }
                }
                if (x == usersCount)
                {
                    Users.Add(at.ATUser);
                }
                orderIDs += $",{at.ATOrder.ID}";
                customerIDs += $",{at.ATCustomer.ID}";
            }

            if (!string.IsNullOrEmpty(orderIDs))
            {
                orderIDs = orderIDs.Remove(0, 1);
            }
            if (!string.IsNullOrEmpty(customerIDs))
            {
                customerIDs = customerIDs.Remove(0, 1);
            }

            GetOrderLines();
        }

        void GetOrderLines()
        {
            var orderLinesTask = orderLineService.GetOrderLines(orderIDs);
            foreach (OrderLine ol in orderLinesTask.Result)
            {
                ol.ProductName = _products.FirstOrDefault(p => p.ID == ol.ProductID).Name;
                ol.SubTotal = ol.Quantity * ol.UnitPrice;
                OrderLines.Add(ol);
            }
        }

        void SearchProduct(string arg)
        {
            Products = new ObservableRangeCollection<Product>(AllProducts.Where(p => p.Name.ToLowerInvariant().Contains(arg.ToLowerInvariant())));
        }

        void SearchCustomer(string arg)
        {
            ClearSelectedProperties();
            Users = new ObservableRangeCollection<User>(AllUsers.Where(u => u.FullName.ToLowerInvariant().Contains(arg.ToLowerInvariant())));
        }

        void SwitchUser(object o)
        {
            if (o == null)
                return;

            User user = o as User; 

            SelectedUser = user;
            SelectedCustomer = Customers.First(c => c.UserID == user.ID);
            SelectedOrder = Orders.FirstOrDefault(or => or.CustomerID == _selectedCustomer.ID);
            SelectedReward = previousSelectedReward = DummyReward;

            CurrentOrderLines = new ObservableCollection<OrderLine>(OrderLines.Where(ol => ol.OrderID == SelectedOrder.ID));

            var group = from ol in CurrentOrderLines
                        group ol by ol.CreatedOnForUI into newGroup
                        orderby newGroup.Key descending
                        select newGroup;
            CurrentOrderLineGroup = new ObservableCollection<IGrouping<string, OrderLine>>(group);
            NewOrderLines.Clear();
            TotalOrderLinesCost = 0;
            CanAddNewOrderLine = false;

            foreach (var colg in CurrentOrderLineGroup)
            {
                decimal total = 0;
                foreach (var ol in colg)
                {
                    total += ol.SubTotal;
                }
            }

            if (IsEmpty == false)
                IsEmpty = true;
        }

        void AddProductToOrderLine (Product p)
        {
            if(_isEmpty == false)
            {
                toastService.ShowLongMessage("Choose a tab first.");
                return;
            }

            SelectedProduct = null;

            var newTotalCost = p.UnitPrice + TotalOrderLinesCost;
            if(newTotalCost > _selectedCustomer.Balance)
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
                    OrderID = _selectedOrder.ID,
                    ProductID = p.ID,
                    UnitPrice = p.UnitPrice,
                    Quantity = 1,
                    CreatedBy = StaffUser.StaffID,

                    CustomerName = _selectedUser.FullName,
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
            if (newTotal > _selectedCustomer.Balance)
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

        void CheckReward(Reward r)
        {
            if(r == null)
            {
                SelectedReward = DummyReward;
            }
            else if(r.Points > _selectedCustomer.Points)
            {
                SelectedReward = previousSelectedReward;
                toastService.ShowLongMessage("Points not enough.");
            } 
            else
            {
                previousSelectedReward = SelectedReward;
            }
        }

        void ClearSelectedProperties()
        {
            SelectedUser = null;
            SelectedCustomer = null;
            SelectedOrder = null;
            CurrentOrderLines = null;
            TotalOrderLinesCost = 0;
            CurrentOrderLineGroup = null;
            SelectedReward = null;
            NewOrderLines.Clear();
            IsEmpty = false;
            TotalOrderLinesCost = 0;
        }
        void RemoveIDFromStr(ref string IDs, string IDToDel)
        {
            string temp = string.Empty;
            int x;
            for (x = IDs.Length-1; x >= 0 && IDs[x] != ','; x--)
            {
                temp = IDs[x] + temp;
            }
            if(x < 0)
            {
                IDs = IDs.Remove(0);
            }
            else if ((string.Compare(IDToDel, temp)) == 0)
            {
                IDs = IDs.Remove(x);
            }
            else 
            {
                IDs = IDs.Replace(IDToDel + ",", "");
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
                string comma = string.IsNullOrEmpty(orderIDs) ? "" : ",";
                int usersCount = Users.Count, x;
                ActiveTab activeTab = JsonConvert.DeserializeObject<ActiveTab>(Uri.UnescapeDataString(at));
                string atFName = activeTab.ATUser.FullName;

                Customers.Add(activeTab.ATCustomer);
                Orders.Add(activeTab.ATOrder);
                for (x = 0; x < usersCount; x++)
                {
                    if (string.Compare(atFName, _users[x].FullName) < 0)
                    {
                        Users.Insert(x, activeTab.ATUser);
                        break;
                    }
                }
                if (x == usersCount)
                {
                    Users.Add(activeTab.ATUser);
                }
                orderIDs += $"{comma}{activeTab.ATOrder.ID}";
                customerIDs += $"{comma}{activeTab.ATCustomer.ID}";
            }
        }

        #region Getters setters
        string _time;
        public string Time
        {
            get => _time;
            set => SetProperty(ref _time, value);
        }

        string _date;
        public string Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }
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
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
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
            get=> _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        ObservableRangeCollection<User> _allUsers;
        public ObservableRangeCollection<User> AllUsers
        {
            get => _allUsers;
            set => SetProperty(ref _allUsers, value);
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

        ObservableRangeCollection<Product> _allProducts;
        public ObservableRangeCollection<Product> AllProducts
        {
            get => _allProducts;
            set => SetProperty(ref _allProducts, value);
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

        Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
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
