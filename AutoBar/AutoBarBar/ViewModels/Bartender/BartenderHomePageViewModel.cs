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
using Xamarin.Forms;

namespace AutoBarBar.ViewModels
{
    public class BartenderHomePageViewModel : BaseViewModel
    {
        public ICommand SwitchUserCommand { get; }
        public ICommand ShowScanCommand { get; }
        public ICommand GetReloadBalanceAmountCommand { get; }
        public ICommand EndTransactionCommand { get; }
        public ICommand AddProductToOrderLineCommand { get; }
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand AddOrderLineCommand { get; }

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
            Customers = new ObservableCollection<Customer>();
            Products = new ObservableCollection<Product>();
            OrderLines = new ObservableCollection<OrderLine>();
            Orders = new ObservableCollection<Order>();
            Rewards = new ObservableCollection<Reward>();
            NewOrderLines = new ObservableCollection<OrderLine>();
            SelectedProduct = null;
            TotalOrderLinesCost = 0;
            CanAddNewOrderLine = false;

            PopulateData();
            SwitchUser(Customers[0]);

            SelectedReward = DummyReward;
            SwitchUserCommand = new Command<object>(SwitchUser);
            ShowScanCommand = new Command(ShowScan);
            GetReloadBalanceAmountCommand = new Command(GetReloadBalanceAmount);
            EndTransactionCommand = new Command(EndTransaction);
            AddProductToOrderLineCommand = new Command<Product>(AddProductToOrderLine);
            IncreaseQuantityCommand = new Command<OrderLine>(IncreaseQuantity);
            DecreaseQuantityCommand = new Command<OrderLine>(DecreaseQuantity);
            AddOrderLineCommand = new Command(AddOrderLine);

            Test = new Command(TestMe);
        }

        void TestMe()
        {
            var a = SelectedCustomer;
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
                a.SubTotal = a.Quantity * a.Price;
                OrderLines.Add(a);
            }

            Orders.Clear();
            var o = await OrderDataStore.GetItemsAsync();
            foreach (var a in o)
            {
                Orders.Add(a);
            }

            Rewards.Clear();
            Rewards.Add(DummyReward);
            var r = await RewardDataStore.GetItemsAsync();
            foreach (var a in r)
            {
                Rewards.Add(a);
            }
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
            foreach(var colg in CurrentOrderLineGroup)
            {
                double total = 0;
                foreach(var ol in colg)
                {
                    total += ol.SubTotal;
                }
            }
        }

        async void ShowScan()
        {
            await Shell.Current.GoToAsync($"{nameof(ScanPage)}");
        }
        
        async void GetReloadBalanceAmount()
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

        async void EndTransaction()
        {
            await App.Current.MainPage.DisplayAlert("Success", "Customer transaction has ended.", "Ok");

            CurrentOrder.OrderStatus = true;
            Orders.Add(CurrentOrder);
            Customers.Remove(SelectedCustomer);
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

        async void AddOrderLine()
        {
            await App.Current.MainPage.DisplayAlert("Success", "You have successfully ordered.", "Thanks");
            string time = "9:45PM"; 
            int pe;
            var group = from ol in NewOrderLines
                        group ol by time into newOl
                        select newOl;
            CurrentOrderLineGroup.Add(group.ElementAt(0));
            foreach (var ol in NewOrderLines)
            {
                OrderLines.Add(ol);

            }
            CurrentBalance = SelectedCustomer.CurrentBalance -= TotalOrderLinesCost;
            NewOrderLines.Clear();
            TotalOrderPrice =  CurrentOrder.TotalPrice += TotalOrderLinesCost;
            PointsEarned = CurrentOrder.PointsEarned += (pe = (int)CurrentOrder.TotalPrice / 1000) != 0 ? pe * 100 : 0;
            TotalOrderLinesCost = 0;
        }

        #region Getters setters
        #region Customers
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

        double currentBalance;
        public double CurrentBalance
        {
            get => currentBalance;
            set => SetProperty(ref currentBalance, value);
        }
        #endregion

        #region Products
        ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
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
        ObservableCollection<OrderLine> orderLines;
        public ObservableCollection<OrderLine> OrderLines
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
        ObservableCollection<Reward> rewards;
        public ObservableCollection<Reward> Rewards
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
