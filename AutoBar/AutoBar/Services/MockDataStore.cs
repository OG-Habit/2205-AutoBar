using AutoBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;
using static AutoBar.Constants;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AutoBar.Services
{
    public class MockDataStore : BaseService, IDataStore<Product>, IDataStore<OrderLine>, IDataStore<Order>, IDataStore<Reward>, IDataStore<TransactionHistory>, IDataStore<PointsHistory>
    {
        readonly List<Product> products = new List<Product>();
        readonly List<OrderLine> orderLines = new List<OrderLine>();
        readonly List<Order> orders= new List<Order>();
        readonly List<Reward> rewards= new List<Reward>();
        readonly List<TransactionHistory> transactionHistories= new List<TransactionHistory>();
        readonly List<PointsHistory> pointsHistories = new List<PointsHistory>();
        public int UserID { get; set; }

        public MockDataStore()
        {
            SetUserId();

            string cmd1 = @"
                SELECT * FROM Products
            ";

            GetItems<Product>(cmd1, (dataRecord, product) =>
            {
                product.Id = dataRecord.GetInt32(0);
                product.Name = dataRecord.GetString(1);
                product.Description = dataRecord.GetString(2);
                product.Price = Convert.ToDouble(dataRecord.GetDecimal(3));
                product.ImageLink = "default_menu.png";
                products.Add(product);
            });

            string cmd2 = $@" 
                SELECT ol.ID, ol.OrderID, ol.ProductID, ol.UnitPrice, ol.Quantity, ol.CreatedOn, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", p.Name
                FROM OrderLine ol, Orders o, Customers c, Users u, Products p
                WHERE ol.OrderID=o.ID AND o.CustomerID=c.ID AND c.UserID=u.ID AND p.ID=ol.ProductID
                AND u.ID={UserID}";

            GetItems<OrderLine>(cmd2, (dataRecord, ol) =>
            {
                ol.Id = dataRecord.GetInt32(0);
                ol.OrderId = dataRecord.GetInt32(1);
                ol.ProductId = dataRecord.GetInt32(2);
                ol.Price = Convert.ToDouble(dataRecord.GetDecimal(3));
                ol.Quantity = dataRecord.GetInt32(4);
                ol.CreatedOn = dataRecord.GetDateTime(5);
                ol.CustomerName = dataRecord.GetString(6);
                ol.ProductName = dataRecord.GetString(7);
                ol.ProductImgUrl = "default_item.png";
                ol.SubTotal = ol.Price * ol.Quantity;
                orderLines.Add(ol);
            });

            string cmd3 = $@" 
                SELECT o.ID, o.OpenedOn, o.ClosedOn, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", o.TotalPrice, o.PointsEarned, ""None"" AS ""Reward"", o.OrderStatus, c.ID
                FROM Orders o, Bartenders b, Customers c, Users u
                WHERE(o.CustomerID = c.ID AND c.UserID = u.ID
                AND u.ID = {UserID} AND o.HasReward = 0 AND o.TotalPrice <> 0)
                UNION
                SELECT o.ID, o.OpenedOn, o.ClosedOn, CONCAT(u.FirstName, "" "", u.LastName) AS ""Name"", o.TotalPrice, o.PointsEarned, r.Name AS ""Reward"", o.OrderStatus, c.ID
                FROM Orders o, Bartenders b, Customers c, Users u, UsedRewards ur, Rewards r
                WHERE(o.CustomerID = c.ID AND c.UserID = u.ID
                AND ur.OrderID = o.ID AND ur.RewardID = r.ID
                AND u.ID = {UserID} AND o.HasReward = 1 AND o.TotalPrice <> 0)
                GROUP BY o.ID";

            GetItems<Order>(cmd3, (dataRecord, o) =>
            {
                o.Id = dataRecord.GetInt32(0);
                o.OpenedOn = dataRecord.GetDateTime(1);
                o.ClosedOn = dataRecord.GetDateTime(2);
                o.CustomerName = dataRecord.GetString(3);
                o.TotalPrice = dataRecord.GetDouble(4);
                o.PointsEarned = Convert.ToDouble(dataRecord.GetDecimal(5));
                o.BartenderName = "Ivan Woogue"; //temporary workaround since there's a field on it, need to change
                o.Reward = dataRecord.GetString(6);
                o.OrderStatus = dataRecord.GetInt32(7);
                o.CustomerId = dataRecord.GetInt32(8);
                orders.Add(o);
            });

            

            string cmd5 = $@"
                SELECT o.ClosedOn AS ""Timestamp"", ""Order"" as ""Type"", o.TotalPrice AS ""Amount"" 
                FROM Orders o, Customers c, Users u
                WHERE o.OrderStatus = 2 AND o.TotalPrice <> 0 AND o.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID}
                UNION
                SELECT r.CreatedOn AS ""Timestamp"", ""Reload"" AS ""Type"", r.CreditedBalance AS ""Amount""
                FROM Reloads r, Customers c, Users u
                WHERE r.CreditedBalance <> 0 AND r.CustomerID = c.ID AND c.UserID = u.ID AND u.ID={UserID}
                ORDER BY ""Timestamp"" DESC
            ";

            GetItems<TransactionHistory>(cmd5, (dataRecord, th) =>
            {
                th.TimeStamp = dataRecord.GetString(0);
                th.Type = dataRecord.GetString(1);
                th.Amount = dataRecord.GetDecimal(2);
                transactionHistories.Add(th);
            });

            string cmd6 = $@"
                SELECT o.ClosedOn AS ""Timestamp"", ""Earned"" as ""Type"", o.PointsEarned AS ""Points""
                FROM Orders o, Customers c, Users u
                WHERE o.OrderStatus = 2 AND o.PointsEarned <> 0 AND o.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID}
                UNION
                SELECT ur.CreatedOn AS ""Timestamp"", ""Redeemed"" AS ""Type"", ur.PointsDeducted AS ""Points""
                FROM UsedRewards ur, Customers c, Users u
                WHERE ur.PointsDeducted <> 0 AND ur.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID}
                ORDER BY ""Timestamp"" DESC
            ";

            GetItems<PointsHistory>(cmd6, (dataRecord, ph) =>
            {
                ph.TimeStamp = dataRecord.GetString(0);
                ph.Type = dataRecord.GetString(1);
                ph.Points = dataRecord.GetDecimal(2);
                pointsHistories.Add(ph);
            });

            /*{
                new Product { Id = "1", Name = "Apple", Description = "The apple is red, plump, and fresh", ImageLink = "default_pic.png", Price = 45.50 },
                new Product { Id = "2", Name = "Beans", Description = "The bean is green, long, and fresh", ImageLink = "default_pic.png", Price = 95.50 },
                new Product { Id = "3", Name = "Carrots", Description = "The carrot is orange, healthy, and fresh", ImageLink = "default_pic.png", Price = 60.75 },
                new Product { Id = "4", Name = "Duck", Description = "The duck is tasty, juicy, and free range", ImageLink = "default_pic.png", Price = 399.99 },
                new Product { Id = "5", Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = "6", Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = "7", Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = "8", Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = "9", Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            };

            
            {
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=1, CreatedOn = DateTime.Parse("May 21, 2022 7:30PM"), OrderId="8", ProductImgUrl="default_pic.png", ProductId="1", SubTotal=45.50*1},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Beans", Price=95.50, Quantity=1, CreatedOn = DateTime.Parse("May 22, 2022 7:30PM"), OrderId="9", ProductImgUrl="default_pic.png", ProductId="2", SubTotal=95.50*1},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=3, CreatedOn = DateTime.Parse("May 24, 2022 7:30PM"), OrderId="10", ProductImgUrl="default_pic.png", ProductId="1", SubTotal=45.50*3},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Beans", Price=95.50, Quantity=2, CreatedOn = DateTime.Parse("May 24, 2022 7:30PM"), OrderId="10", ProductImgUrl="default_pic.png", ProductId="2", SubTotal=95.50*2},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Duck", Price=399.99, Quantity=1, CreatedOn = DateTime.Parse("May 24, 2022 8:30PM"), OrderId="10", ProductImgUrl="default_pic.png", ProductId="4", SubTotal=399.99*1},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Egg", Price=10.00, Quantity=10, CreatedOn = DateTime.Parse("May 24, 2022 10:30PM"), OrderId="10", ProductImgUrl="default_pic.png", ProductId="5", SubTotal=10.00*10},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Carrots", Price=60.75, Quantity=3, CreatedOn = DateTime.Parse("May 24, 2022 8:30PM"), OrderId="10", ProductImgUrl="default_pic.png", ProductId="3", SubTotal=60.75*3},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=5, CreatedOn = DateTime.Parse("May 24, 2022 8:30PM"), OrderId="10", ProductImgUrl="default_pic.png", ProductId="1", SubTotal=45.50*5},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Bam Carousel", ProductName="Apple", Price=45.50, Quantity=5, CreatedOn = DateTime.Parse("May 24, 2022 8:30PM"), OrderId="11", ProductImgUrl="default_pic.png", ProductId="1", SubTotal=45.50*5}
            };

            {
                new Order { Id = "8", OpenedOn = DateTime.Parse("5/21/2022 19:30:00"), ClosedOn = DateTime.Parse("5/21/2022 10:30:00PM"), CustomerName="Adam Smith", TotalPrice=45.50, PointsEarned = 0, OrderStatus=false, CustomerId="1", BartenderName="Bartender One", Reward="No Reward"},
                new Order { Id = "9", OpenedOn = DateTime.Parse("5/22/2022 19:30:00"), ClosedOn = DateTime.Parse("5/22/2022 9:30:00PM"), CustomerName="Adam Smith", TotalPrice=95.50, PointsEarned = 0, OrderStatus=false, CustomerId="1", BartenderName="Bartender Four", Reward="No Reward"},
                new Order { Id = "10", OpenedOn = DateTime.Today, ClosedOn = DateTime.Today, CustomerName="Adam Smith", TotalPrice=1236.25, PointsEarned = 100, OrderStatus=false, CustomerId="1", BartenderName="Bartender One", Reward="No Reward"},
                new Order { Id = "11", OpenedOn = DateTime.Today, ClosedOn = DateTime.Today, CustomerName="Bam Carousel", TotalPrice=227.5, PointsEarned = 0, OrderStatus=false, CustomerId="2", BartenderName="Bartender Three", Reward="No Reward"}
            };

            
            {
                new Reward { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Points = 100.00 },
                new Reward { Id = Guid.NewGuid().ToString(), Name = "Apple", Description = "The apple is red, plump, and fresh", ImageLink = "default_pic.png", Points = 400.00 }
            };
            */
        }

        private async void SetUserId()
        {
            string UserString = await Xamarin.Essentials.SecureStorage.GetAsync("user");
            Customer CurrentUser = JsonConvert.DeserializeObject<Customer>(UserString);
            UserID = CurrentUser.UserDetails.ID;
        }

        #region Product
        public async Task<Product> GetItemAsync(int id)
        {
            return await Task.FromResult(products.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Product>> GetItemsAsync(bool forceRefresh)
        { 
            return await Task.FromResult(products);
        }

        public async Task<IEnumerable<Product>> GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(products.Where(p => p.Name.ToLowerInvariant().Contains(query)));
        }

        public Task<Product> GetTodayResults(string query)
        {
            throw new NotImplementedException();
        }
        Task<Product> IDataStore<Product>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region OrderLine
        async Task<OrderLine> IDataStore<OrderLine>.GetItemAsync(int id)
        {
            return await Task.FromResult(orderLines.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetItemsAsync(bool forceRefresh)
        { 
            return await Task.FromResult(orderLines);
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetSearchResults(string query)
        {
            return await Task.FromResult(orderLines.Where(s => s.OrderId.ToString() == query));
        }

        Task<OrderLine> IDataStore<OrderLine>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Order
        async Task<Order> IDataStore<Order>.GetItemAsync(int id)
        {
            return await Task.FromResult(orders.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<Order>> IDataStore<Order>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orders);
        }

        async Task<IEnumerable<Order>> IDataStore<Order>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(orders.Where(c => c.BartenderName.ToLowerInvariant().Contains(query)));
        }

        async Task<IEnumerable<Order>> IDataStore<Order>.GetSearchResults(int id)
        {
            var list = orders;
            return await Task.FromResult(orders.Where(o => o.CustomerId == id));
        }


        async Task<Order> IDataStore<Order>.GetTodayResults(DateTime today)
        {
            return await Task.FromResult(orders.FirstOrDefault(s => s.OpenedOn.Date == today.Date));
        }
        #endregion

        #region Reward
        async Task<Reward> IDataStore<Reward>.GetItemAsync(int id)
        {
            return await Task.FromResult(rewards.FirstOrDefault(s => s.Id == id.ToString()));
        }

        async Task<IEnumerable<Reward>> IDataStore<Reward>.GetItemsAsync(bool forceRefresh)
        {
            string cmd4 = @"
                SELECT * FROM Rewards
            ";
            Debug.WriteLine("Selecting from rewards");
            GetItems<Reward>(cmd4, (dataRecord, reward) =>
            {
                reward.Id = dataRecord.GetInt32(0).ToString();
                reward.Name = dataRecord.GetString(1);
                reward.Description = dataRecord.GetString(2);
                reward.Points = Convert.ToDouble(dataRecord.GetDecimal(3));
                reward.ImageLink = "default_reward.png";
                rewards.Add(reward);
                Debug.WriteLine(reward.Id);
            });
            Debug.WriteLine("Selected from rewards");
            return await Task.FromResult(rewards);
        }

        async Task<IEnumerable<Reward>> IDataStore<Reward>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(rewards.Where(c => c.Name.ToLowerInvariant().Contains(query)));
        }

        Task<Reward> IDataStore<Reward>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region TransactionHistory
        Task<TransactionHistory> IDataStore<TransactionHistory>.GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<TransactionHistory>> IDataStore<TransactionHistory>.GetItemsAsync(bool forceRefresh)
        {
            
            return await Task.FromResult(transactionHistories);
        }

        Task<IEnumerable<TransactionHistory>> IDataStore<TransactionHistory>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }

        Task<TransactionHistory> IDataStore<TransactionHistory>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }

        Task<PointsHistory> IDataStore<PointsHistory>.GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PointsHistory> GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<PointsHistory>> IDataStore<PointsHistory>.GetItemsAsync(bool forceRefresh)
        {

            return await Task.FromResult(pointsHistories);
        }

        Task<IEnumerable<PointsHistory>> IDataStore<PointsHistory>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetSearchResults(int id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetSearchResults(int id)
        {
            return await Task.FromResult(orderLines.Where(ol => ol.OrderId == id));
        }

        Task<IEnumerable<Reward>> IDataStore<Reward>.GetSearchResults(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TransactionHistory>> IDataStore<TransactionHistory>.GetSearchResults(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<PointsHistory>> IDataStore<PointsHistory>.GetSearchResults(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}