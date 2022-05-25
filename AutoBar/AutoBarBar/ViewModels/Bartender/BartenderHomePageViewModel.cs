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
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using static AutoBarBar.Constants;
using static AutoBarBar.DateTimeHelper;

namespace AutoBarBar.ViewModels
{
    public class BartenderHomePageViewModel : BaseViewModel
    {
        readonly IProductService productService;
        readonly IActiveTabService activeTabService;
        readonly IOrderLineService orderLineService;

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
                List<int> orderIDs = new List<int>();
                var getRewardsTask = GetItemsAsync(Rewards, RewardDataStore);
                var productsTask = productService.GetProducts();
                var activeTabsTask = activeTabService.GetActiveTabs();

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
                    orderIDs.Add(at.ATOrder.ID);
                }

                var orderLinesTask = orderLineService.GetOrderLines(orderIDs);
                orderLinesTask.Wait();
                foreach(OrderLine ol in orderLinesTask.Result)
                {
                    ol.ProductName = Products.FirstOrDefault(p => p.ID == ol.ProductID).Name;
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
            ObservableCollection<User> user = new ObservableCollection<User>();
            var builder = new MySqlConnectionStringBuilder
            {
                Server = "sql6.freemysqlhosting.net", 
                UserID = "sql6494729",
                Password = "gEB2fyY5T4",
                Database = "sql6494729"
            };

            using (var conn = new MySqlConnection(builder.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string cmd = "SELECT * FROM Users";
                    using (var command = new MySqlCommand(cmd, conn))
                    using (var reader = command.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            var list = reader.GetEnumerator();
                            while (list.MoveNext())
                            {
                                User u = new User();
                                var a = list.Current;
                                DbDataRecord dataRecord = (DbDataRecord)a;
                                u.FirstName = dataRecord.GetString(2);
                                u.LastName = dataRecord.GetString(3);
                                int fc = dataRecord.FieldCount;
                                user.Add(u);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    var m = e.Message;
                }
            }
            {
                //try
                //{
                //    //Task contask = connection.OpenAsync();
                //    //contask.Await();
                //    await conn.OpenAsync();
                //    //using (var cmd = new MySqlCommand())
                //    //{
                //    //    cmd.Connection = conn;
                //    //    cmd.CommandText = "SELECT * FROM Test";
                //    //    try
                //    //    {
                //    //        using (var reader = await cmd.ExecuteReaderAsync())
                //    //        {
                //    //            while (await reader.ReadAsync())
                //    //            {
                //    //                var a = reader.GetString(0);
                //    //            }
                //    //        }
                //    //    }
                //    //    catch(Exception e)
                //    //    {
                //    //        var a = e.Message;
                //    //    }
                //    //}
                //} 
                //catch (MySqlException e)
                //{
                //    var a = e.Message;
                //    switch(e.Number)
                //    {
                //        case 4060:
                //            break;
                //        case 18456:
                //            break;
                //        default:
                //            break;
                //    }
                //}
                //catch (NullReferenceException e)
                //{
                //    var a = e.Message;
                //}
                //catch(Exception e)
                //{
                //    var a = e.Message;
                //    var source = e.Source;
                //}
            };
            //await Shell.Current.GoToAsync($"{nameof(ScanPage)}");
        }
        
        async Task GetReloadBalanceAmount()
        {
            string ans = await Application.Current.MainPage.DisplayPromptAsync("Balance", "Enter amount:", "Add", "Cancel", null, -1, Keyboard.Numeric, "");
            if(decimal.TryParse(ans, out decimal num))
            {
                foreach(var c in Customers)
                {
                    if(c.ID == SelectedCustomer.ID)
                    {
                        c.Balance += num; 
                        await activeTabService.AddBalance(c.ID, c.Balance, GetPHTime());
                        await Application.Current.MainPage.DisplayAlert("Success", "Balance has been added.", "Ok");
                        CurrentBalance = c.Balance;
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
            await App.Current.MainPage.DisplayAlert("Success", "You have successfully ordered.", "Thanks");
            string time = "9:45PM";
            int pe;
            var group = from ol in NewOrderLines
                        group ol by time into newOl
                        select newOl;
            CurrentOrderLineGroup.Add(group.ElementAt(0));
            OrderLines.AddRange(NewOrderLines);
            NewOrderLines.Clear();

            CurrentBalance = SelectedCustomer.Balance -= TotalOrderLinesCost;
            TotalOrderPrice = CurrentOrder.TotalPrice += Convert.ToDouble(TotalOrderLinesCost);
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

        void SwitchUser(object o)
        {
            if (o == null)
                return;

            User user = o as User;
            
            SelectedUser = user;
            SelectedCustomer = Customers.FirstOrDefault(c => c.UserID == user.ID);
            // Extend BaseModel
            CurrentBalance = SelectedCustomer.Balance;
            CurrentOrder = Orders.First(order => order.CustomerID == SelectedCustomer.ID);
            CurrentOrderLines = new ObservableCollection<OrderLine>(OrderLines.Where(ol => ol.OrderID == CurrentOrder.ID));
            PointsEarned = CurrentOrder.PointsEarned;
            TotalOrderPrice = CurrentOrder.TotalPrice;

            var group = from ol in CurrentOrderLines
                        group ol by ol.CreatedOn into newGroup
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

            if (CanAddNewOrderLine == false)
                CanAddNewOrderLine = true;

            var newTotalCost = p.UnitPrice + TotalOrderLinesCost;
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
                    CustomerName = SelectedUser.FirstName,
                    ProductName = p.Name,
                    UnitPrice = p.UnitPrice,
                    Quantity = 1,
                    ProductImgUrl = p.ImageLink,
                    CreatedOn = DateTime.Now.ToString(),
                    SubTotal = p.UnitPrice
                });
                TotalOrderLinesCost = newTotalCost;
            } 
            else
            {
                DependencyService.Get<IToastService>().ShowShortMessage($"{p.Name} is already in the order.");
            }
        }

        void IncreaseQuantity(OrderLine Ol)
        {
            var newTotal = TotalOrderLinesCost + Ol.UnitPrice;
            if (newTotal > SelectedCustomer.Balance)
            {
                DependencyService.Get<IToastService>().ShowLongMessage("Insufficient balance.");
                return;
            }

            foreach(var nol in NewOrderLines)
            {
                if (nol.ID == Ol.ID)
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
                if(nol.ID == Ol.ID)
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

        decimal currentBalance;
        public decimal CurrentBalance
        {
            get => currentBalance;
            set => SetProperty(ref currentBalance, value);
        }
        #endregion

        #region Users
        ObservableRangeCollection<User> users;
        public ObservableRangeCollection<User> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }

        User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set { SetProperty(ref selectedUser, value); }
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

        decimal pointsEarned;
        public decimal PointsEarned
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

        decimal totalOrderLinesCost;
        public decimal TotalOrderLinesCost
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

        #region ActiveTabs
        ObservableRangeCollection<ActiveTab> activeTabs;
        public ObservableRangeCollection<ActiveTab> ActiveTabs
        {
            get => activeTabs;
            set => SetProperty(ref activeTabs, value);
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
