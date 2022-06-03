using AutoBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AutoBar.Services
{
    public class MockDataStore : BaseService, IDataStore<Product>, IDataStore<OrderLine>, IDataStore<Order>, IDataStore<Reward>, IDataStore<TransactionHistory>, IDataStore<PointsHistory>
    {
        readonly List<Product> products;
        readonly List<OrderLine> orderLines;
        readonly List<Order> orders;
        readonly List<Reward> rewards;
        readonly List<TransactionHistory> tHistory;
        readonly List<PointsHistory> pHistory;

        private int UserID;

        public MockDataStore()
        {
            SetUserID();

            products = new List<Product>();
            string cmd = $@"
                SELECT * FROM Products WHERE IsDeleted = 0
            ";

            GetItems<Product>(cmd, (dataRecord, product) =>
            {
                product.Id = dataRecord.GetInt32(0);
                product.Name = dataRecord.GetString(1);
                product.Description = dataRecord.GetString(2);
                product.Price = dataRecord.GetDecimal(3);
                product.ImageLink = dataRecord.GetString(4);
                products.Add(product);
            });

            orderLines = new List<OrderLine>();
            string cmd2 = $@"
                SELECT ol.ID, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", p.Name, ol.UnitPrice, ol.Quantity, 
                ol.CreatedOn, ol.OrderID, p.ImageLink, p.ID
                FROM OrderLine ol, Orders o, Customers c, Users u, Products p
                WHERE ol.OrderID = o.ID AND ol.ProductID = p.ID AND o.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID}
            ";

            GetItems<OrderLine>(cmd2, (dataRecord, orderLine) =>
            {
                orderLine.Id = dataRecord.GetInt32(0);
                orderLine.CustomerName = dataRecord.GetString(1);
                orderLine.ProductName = dataRecord.GetString(2);
                orderLine.Price = dataRecord.GetDecimal(3);
                orderLine.Quantity = dataRecord.GetInt32(4);
                orderLine.CreatedOn = dataRecord.GetDateTime(5);
                orderLine.OrderId = dataRecord.GetInt32(6);
                orderLine.ProductImgUrl = dataRecord.GetString(7);
                orderLine.ProductId = dataRecord.GetInt32(8);
                orderLine.SubTotal = orderLine.Price * orderLine.Quantity;
                orderLines.Add(orderLine);
            });

            orders = new List<Order>();
            string cmd3 = $@"
                SELECT o.ID, o.OpenedOn, o.ClosedOn, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", o.TotalPrice, o.PointsEarned,
                        o.OrderStatus, u.ID, 
                    CASE o.HasReward
                        WHEN 0 THEN ""No Reward""
                        WHEN 1 THEN r.Name
                    END AS ""Reward""
                FROM Orders o, Customers c, Users u, UsedRewards ur, Rewards r
                WHERE o.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID} OR (ur.OrderID = o.ID AND ur.RewardID = r.ID)
                GROUP BY o.ID;
            ";

            GetItems<Order>(cmd3, (dataRecord, order) =>
            {
                order.Id = dataRecord.GetInt32(0);
                order.OpenedOn = dataRecord.GetDateTime(1);
                order.ClosedOn = dataRecord.GetDateTime(2);
                order.CustomerName = dataRecord.GetString(3);
                order.TotalPrice = dataRecord.GetDouble(4);
                order.PointsEarned = dataRecord.GetDecimal(5);
                order.OrderStatus = dataRecord.GetInt32(6) == 1 ? true : false;
                order.CustomerId = dataRecord.GetInt32(7);
                order.BartenderName = "Ivan Woogue"; //temp
                order.Reward = dataRecord.GetString(8);
                orders.Add(order);
            });

            rewards = new List<Reward>();
            string cmd4 = $@"
                SELECT * FROM Rewards WHERE IsDeleted = 0
            ";

            GetItems<Reward>(cmd4, (dataRecord, reward) =>
            {
                reward.Id = dataRecord.GetInt32(0);
                reward.Name = dataRecord.GetString(1);
                reward.Description = dataRecord.GetString(2);
                reward.Points = Convert.ToInt32(dataRecord.GetDecimal(3));
                reward.ImageLink = dataRecord.GetString(4);
                rewards.Add(reward);
            });

            tHistory = new List<TransactionHistory>();
            string cmd5 = $@"
                SELECT x AS ""Timestamp"", y AS ""Type"", z AS ""Amount"" FROM 
                    (SELECT o.ClosedOn AS x, ""Order"" as y, o.TotalPrice AS z 
                    FROM Orders o, Customers c, Users u WHERE o.OrderStatus = 2 AND o.TotalPrice <> 0 AND o.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID} 
                    UNION 
                    SELECT r.CreatedOn AS x, ""Reload"" AS y, r.CreditedBalance AS z FROM Reloads r, Customers c, Users u 
                    WHERE r.CreditedBalance <> 0 AND r.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID}) 
                AS Results 
                ORDER BY x DESC LIMIT 0,20
            ";

            GetItems<TransactionHistory>(cmd5, (dataRecord, th) =>
            {
                th.TimeStamp = dataRecord.GetDateTime(0);
                th.Type = dataRecord.GetString(1);
                th.Amount = String.Equals(th.Type,"Order") ? Convert.ToDecimal((dataRecord.GetDouble(2))*(-1)) : Convert.ToDecimal(dataRecord.GetDouble(2));
                tHistory.Add(th);
            });

            pHistory = new List<PointsHistory>();
            string cmd6 = $@"
                SELECT x AS ""Timestamp"", y AS ""Type"", z AS ""Amount"" FROM 
                    (SELECT o.ClosedOn AS x, ""Earned"" as y, o.PointsEarned AS z
                    FROM Orders o, Customers c, Users u
                    WHERE o.OrderStatus = 2 AND o.PointsEarned <> 0 AND o.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID}
                    UNION
                    SELECT ur.CreatedOn AS x, ""Redeemed"" AS y, (ur.PointsDeducted * -1) AS z
                    FROM UsedRewards ur, Customers c, Users u
                    WHERE ur.PointsDeducted <> 0 AND ur.CustomerID = c.ID AND c.UserID = u.ID AND u.ID = {UserID})
                AS Results 
                ORDER BY x DESC LIMIT 0,20
            ";

            GetItems<PointsHistory>(cmd6, (dataRecord, ph) =>
            {
                ph.TimeStamp = dataRecord.GetDateTime(0);
                ph.Type = dataRecord.GetString(1);
                ph.Points = dataRecord.GetDecimal(2);
                pHistory.Add(ph);
            });
        }

        async void SetUserID()
        {
            UserID = Convert.ToInt32(await Xamarin.Essentials.SecureStorage.GetAsync("id"));
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
            int query2 = Convert.ToInt32(query);
            return await Task.FromResult(orderLines.Where(s => s.OrderId == query2));
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
            int query2 = Convert.ToInt32(query);
            return await Task.FromResult(orders.Where(c => c.CustomerId == query2));
        }

        async Task<Order> IDataStore<Order>.GetTodayResults(DateTime today)
        {
            return await Task.FromResult(orders.FirstOrDefault(s => s.OpenedOn.Date == today.Date));
        }
        #endregion

        #region Reward
        async Task<Reward> IDataStore<Reward>.GetItemAsync(int id)
        {
            return await Task.FromResult(rewards.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<Reward>> IDataStore<Reward>.GetItemsAsync(bool forceRefresh)
        {
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

        Task<TransactionHistory> IDataStore<TransactionHistory>.GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<TransactionHistory> IDataStore<TransactionHistory>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<TransactionHistory>> IDataStore<TransactionHistory>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(tHistory);
        }

        Task<IEnumerable<TransactionHistory>> IDataStore<TransactionHistory>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }

        Task<PointsHistory> IDataStore<PointsHistory>.GetItemAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<PointsHistory> IDataStore<PointsHistory>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<PointsHistory>> IDataStore<PointsHistory>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(pHistory);
        }

        Task<IEnumerable<PointsHistory>> IDataStore<PointsHistory>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}