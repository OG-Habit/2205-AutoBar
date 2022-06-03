using AutoBarBar.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AutoBarBar.Constants;

namespace AutoBarBar.Services
{
    public class MockDataStore : BaseService, IDataStore<CustomerForAdmin>, IDataStore<Product>, IDataStore<OrderLine>, IDataStore<Order>, IDataStore<Reward>, IDataStore<Bartender>
    {
        readonly List<CustomerForAdmin> customers;
        readonly List<Product> products;
        readonly List<OrderLine> orderLines;
        readonly List<Order> orders;
        readonly List<Reward> rewards;
        readonly List<Bartender> bartenders;

        //to fix: cannot update immediately after insert because we didnt obtain last insert id yet from db

        public MockDataStore()
        {
            customers = new List<CustomerForAdmin>();
            string cmd1 = @"
                SELECT c.ID, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", u.Email, u.MobileNumber, u.Birthday, u.Sex, u.ImageLink,
                c.Balance, c.Points
                FROM Customers c, Users u
                WHERE c.UserID = u.ID
            ";
            GetItems<CustomerForAdmin>(cmd1, (dataRecord, c) =>
            {
                c.Id = dataRecord.GetInt32(0);
                c.Name = dataRecord.GetString(1);
                c.Email = dataRecord.GetString(2);
                c.Contact = dataRecord.GetString(3);
                c.Birthday = dataRecord.GetDateTime(4);
                c.Sex = dataRecord.GetString(5);
                c.ImageLink = dataRecord.GetString(6);
                c.CurrentBalance = dataRecord.GetDecimal(7);
                c.TotalPoints = dataRecord.GetDecimal(8);
                c.Status = "Member"; //temp
                c.CardIssued = Convert.ToDateTime("May 25, 2022"); //temp

                customers.Add(c);
            });

            products = new List<Product>();
            string cmd2 = @"
                SELECT * FROM Products WHERE IsDeleted = 0;
            ";

            GetItems<Product>(cmd2, (dataRecord, product) =>
            {
                product.ID = dataRecord.GetInt32(0);
                product.Name = dataRecord.GetString(1);
                product.Description = dataRecord.GetString(2);
                product.UnitPrice = dataRecord.GetDecimal(3);
                product.ImageLink = "default_menu.png";
                products.Add(product);
            });

            orderLines = new List<OrderLine>();
            string cmd3 = @"
                SELECT ol.ID, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", p.Name, ol.UnitPrice, ol.Quantity, 
                ol.CreatedOn, ol.OrderID, p.ImageLink
                FROM OrderLine ol, Orders o, Customers c, Users u, Products p
                WHERE ol.OrderID = o.ID AND ol.ProductID = p.ID AND o.CustomerID = c.ID AND c.UserID = u.ID
            ";

            GetItems<OrderLine>(cmd3, (dataRecord, ol) =>
            {
                ol.ID = dataRecord.GetInt32(0);
                ol.CustomerName = dataRecord.GetString(1);
                ol.ProductName = dataRecord.GetString(2);
                ol.UnitPrice = dataRecord.GetDecimal(3);
                ol.Quantity = dataRecord.GetInt32(4);
                ol.CreatedOnForUI = dataRecord.GetDateTime(5).ToString();
                ol.OrderID = dataRecord.GetInt32(6);
                ol.ProductImgUrl = dataRecord.GetString(7);
                orderLines.Add(ol);
            });

            orders = new List<Order>();
            string cmd4 = $@"
                SELECT o.ID, o.OpenedOn, o.ClosedOn, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", o.TotalPrice, o.PointsEarned,
                        o.OrderStatus, u.ID, 
                    CASE o.HasReward
                        WHEN 0 THEN ""No Reward""
                        WHEN 1 THEN r.Name
                    END AS ""Reward""
                FROM Orders o, Customers c, Users u, UsedRewards ur, Rewards r
                WHERE o.CustomerID = c.ID AND c.UserID = u.ID OR (ur.OrderID = o.ID AND ur.RewardID = r.ID)
                GROUP BY o.ID;
            ";

            GetItems<Order>(cmd4, (dataRecord, order) =>
            {
                order.ID = dataRecord.GetInt32(0);
                order.OpenedOn = dataRecord.GetDateTime(1).ToString();
                order.ClosedOn = dataRecord.GetDateTime(2).ToString();
                order.CustomerName = dataRecord.GetString(3);
                order.TotalPrice = dataRecord.GetDouble(4);
                order.PointsEarned = dataRecord.GetDecimal(5);
                order.OrderStatus = dataRecord.GetInt32(6);
                order.CustomerID = dataRecord.GetInt32(7);
                order.BartenderName = "Ivan Woogue"; //temp
                order.Reward = dataRecord.GetString(8);
                orders.Add(order);
            });


            rewards = new List<Reward>();
            string cmd5 = @"
                SELECT * FROM Rewards WHERE IsDeleted = 0;
            ";

            GetItems<Reward>(cmd5, (dataRecord, reward) =>
            {
                reward.ID = dataRecord.GetInt32(0);
                reward.Name = dataRecord.GetString(1);
                reward.Description = dataRecord.GetString(2);
                reward.Points = dataRecord.GetDecimal(3);
                reward.ImageLink = "default_reward.png";
                rewards.Add(reward);
            });

            bartenders = new List<Bartender>();
            string cmd6 = @"
                SELECT b.ID, u.FirstName, u.LastName, u.Email, u.MobileNumber, u.Birthday, u.Sex, u.ImageLink
                FROM Bartenders b, Users u
                WHERE b.UserID = u.ID AND b.IsRemoved=0;
            ";
            GetItems<Bartender>(cmd6, (dataRecord, bartender) =>
            {
                bartender.Id = dataRecord.GetInt32(0);
                bartender.FirstName = dataRecord.GetString(1);
                bartender.LastName = dataRecord.GetString(2);
                bartender.Name = bartender.FirstName + " " + bartender.LastName;
                bartender.Email = dataRecord.GetString(3);
                bartender.Contact = dataRecord.GetString(4);
                bartender.Birthday = dataRecord.GetDateTime(5);
                bartender.Sex = dataRecord.GetString(6);
                bartender.ImageLink = "default_pic.png";
                bartenders.Add(bartender);
            });

        }

        #region Customer
        public async Task<bool> AddItemAsync(CustomerForAdmin item)
        {
            customers.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(CustomerForAdmin item)
        {
            var oldItem = customers.Where((CustomerForAdmin arg) => arg.Id == item.Id).FirstOrDefault();
            customers.Remove(oldItem);
            customers.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = customers.Where((CustomerForAdmin arg) => arg.Id == id).FirstOrDefault();
            customers.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<CustomerForAdmin> IDataStore<CustomerForAdmin>.GetItemAsync(int id)
        {
            return await Task.FromResult(customers.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<CustomerForAdmin>> IDataStore<CustomerForAdmin>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(customers);
        }

        async Task<IEnumerable<CustomerForAdmin>> IDataStore<CustomerForAdmin>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(customers.Where(c => c.Name.ToLowerInvariant().Contains(query)));
        }
        #endregion

        #region Product
        public async Task<bool> AddItemAsync(Product item)
        {
            
            string cmd = $@"
                INSERT INTO Products(`Name`,`UnitPrice`, `Description`, `CreatedBy`)
                VALUES (""{item.Name}"",{item.UnitPrice},""{item.Description}"",1);
            ";
            AddItem(cmd);

            item.ID = products.Count + 1;
            products.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Product item)
        {
            string cmd = $@"
                UPDATE Products 
                SET `Name`=""{item.Name}"",`UnitPrice`={item.UnitPrice}, `Description`=""{item.Description}""
                WHERE ID = {item.ID};
            ";
            AddItem(cmd);

            var oldItem = products.Where((Product arg) => arg.ID == item.ID).FirstOrDefault();
            products.Remove(oldItem);
            products.Add(item);

            return await Task.FromResult(true);
        }

        async Task<bool> IDataStore<Product>.DeleteItemAsync(int id)
        {
            string cmd = $@"
                UPDATE Products 
                SET IsDeleted = 1
                WHERE ID = {id};
            ";
            AddItem(cmd);
            var oldItem = products.Where((Product arg) => arg.ID == id).FirstOrDefault();
            products.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Product> IDataStore<Product>.GetItemAsync(int id)
        {
            Product p = products.FirstOrDefault(s => s.ID == id);
            string cmd2 = $@"
                    SELECT SUM(Quantity) FROM OrderLine 
                    WHERE ProductID = {id} AND DATE(CreatedOn) {FROM_TODAY}
             ";
            GetItem(cmd2, ref p, (dataRecord, user) =>
            {
                p.OrderFrequencyToday = dataRecord.GetInt32(0);
            });

            string cmd3 = $@"
                    SELECT SUM(Quantity) FROM OrderLine  
                    WHERE ProductID = {id} AND DATE(CreatedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem(cmd3, ref p, (dataRecord, user) =>
            {
                p.OrderFrequencyPast7Days = dataRecord.GetInt32(0);
            });

            string cmd4 = $@"
                    SELECT SUM(Quantity) FROM OrderLine  
                    WHERE ProductID = {id}
             ";
            GetItem(cmd4, ref p, (dataRecord, user) =>
            {
                p.OrderFrequencyOverall = dataRecord.GetInt32(0);
            });

            return await Task.FromResult(p);
        }

        async Task<IEnumerable<Product>> IDataStore<Product>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(products);
        }

        async Task<IEnumerable<Product>> IDataStore<Product>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(products.Where(p => p.Name.ToLowerInvariant().Contains(query)));
        }
        #endregion

        #region OrderLine
        public Task<bool> AddItemAsync(OrderLine item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(OrderLine item)
        {
            throw new NotImplementedException();
        }

        async Task<OrderLine> IDataStore<OrderLine>.GetItemAsync(int id)
        {
            return await Task.FromResult(orderLines.FirstOrDefault(s => s.ID == id));
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orderLines);
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(orderLines.Where(c => c.OrderID==Convert.ToInt32(query)));
        }
        #endregion

        #region Order
        public Task<bool> AddItemAsync(Order item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Order item)
        {
            throw new NotImplementedException();
        }

        async Task<Order> IDataStore<Order>.GetItemAsync(int id)
        {
            return await Task.FromResult(orders.FirstOrDefault(s => s.ID == id));
        }

        async Task<IEnumerable<Order>> IDataStore<Order>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orders);
        }

        async Task<IEnumerable<Order>> IDataStore<Order>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(orders.Where(c => c.CustomerName.ToLowerInvariant().Contains(query)));
        }
        #endregion

        #region Reward
        public async Task<bool> AddItemAsync(Reward item)
        {
            string cmd = $@"
                INSERT INTO Rewards(`Name`,`Points`, `Description`, `CreatedBy`)
                VALUES (""{item.Name}"",{item.Points},""{item.Description}"",1);
            ";
            AddItem(cmd);

            item.ID = rewards.Count + 1;
            rewards.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Reward item)
        {
            string cmd = $@"
                UPDATE Rewards 
                SET `Name`=""{item.Name}"",`Points`={item.Points}, `Description`=""{item.Description}""
                WHERE ID = {item.ID};
            ";
            AddItem(cmd);
            var oldItem = rewards.Where((Reward arg) => arg.ID == item.ID).FirstOrDefault();
            rewards.Remove(oldItem);
            rewards.Add(item);

            return await Task.FromResult(true);
        }

        async Task<bool> IDataStore<Reward>.DeleteItemAsync(int id)
        {
            string cmd = $@"
                UPDATE Rewards 
                SET IsDeleted = 1
                WHERE ID = {id};
            ";
            var oldItem = rewards.Where((Reward arg) => arg.ID == id).FirstOrDefault();
            rewards.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Reward> IDataStore<Reward>.GetItemAsync(int id)
        {
            Reward p = rewards.FirstOrDefault(s => s.ID == id);
            string cmd2 = $@"
                    SELECT COUNT(ID) FROM UsedRewards 
                    WHERE RewardID = {id} AND DATE(CreatedOn) {FROM_TODAY}
             ";
            GetItem(cmd2, ref p, (dataRecord, user) =>
            {
                p.ClaimFrequencyToday = dataRecord.GetInt32(0);
            });

            string cmd3 = $@"
                    SELECT COUNT(ID) FROM UsedRewards   
                    WHERE ProductID = {id} AND DATE(CreatedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem(cmd3, ref p, (dataRecord, user) =>
            {
                p.ClaimFrequencyPast7Days = dataRecord.GetInt32(0);
            });

            string cmd4 = $@"
                    SELECT COUNT(ID) FROM UsedRewards   
                    WHERE ProductID = {id}
             ";
            GetItem(cmd4, ref p, (dataRecord, user) =>
            {
                p.ClaimFrequencyOverall = dataRecord.GetInt32(0);
            });

            return await Task.FromResult(p);
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
        #endregion

        #region Bartender
        public async Task<bool> AddItemAsync(Bartender item)
        {
            //temporary code: to be fix with propper encryption
            string temp = item.FirstName.ToLowerInvariant();
            string password = temp + temp; //fnamefname
            string cmd0 = $@"
                INSERT INTO Users(`LastName`,`FirstName`, `Sex`, `Birthday`,`MobileNumber`,`Email`,`Password`,`ImageLink`)
                VALUES (""{item.LastName}"",""{item.FirstName}"",""{item.Sex}"",""{item.Birthday}"",""{item.Contact}"",
                ""{item.Email}"",""{password}"",""{item.ImageLink}"");
                SET @last_id_in_table1 = LAST_INSERT_ID();
                INSERT INTO Bartenders (UserID) VALUES (@last_id_in_table1); 
            ";
            AddItem(cmd0);

            item.Id = bartenders.Count + 1;
            bartenders.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Bartender item)
        {
            string temp = item.FirstName.ToLowerInvariant();
            string password = temp + temp; //fnamefname
            string cmd0 = $@"
                UPDATE Users 
                SET `LastName`=""{item.LastName}"", `FirstName`=""{item.FirstName}"", `Sex`=""{item.Sex}"", `Birthday`=""{item.Birthday}"",
                    `MobileNumber`=""{item.Contact}"",`Email`=""{item.Email}"",`Password`=""{password}"",`ImageLink`=""{item.ImageLink}"")
                WHERE Bartenders.ID = {item.Id} AND Bartenders.UserID = Users.ID;
            ";
            AddItem(cmd0);
            var oldItem = bartenders.Where((Bartender arg) => arg.Id == item.Id).FirstOrDefault();
            bartenders.Remove(oldItem);
            bartenders.Add(item);

            return await Task.FromResult(true);
        }

        async Task<bool> IDataStore<Bartender>.DeleteItemAsync(int id)
        {
            string cmd = $@"
                UPDATE Bartenders 
                SET IsRemoved = 1
                WHERE ID = {id};
            ";
            var oldItem = bartenders.Where((Bartender arg) => arg.Id == id).FirstOrDefault();
            bartenders.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Bartender> IDataStore<Bartender>.GetItemAsync(int id)
        {
            Bartender p = bartenders.FirstOrDefault(s => s.Id == id);
            string cmd2 = $@"
                    SELECT SUM(Quantity*UnitPrice) FROM OrderLine 
                    WHERE CreatedBy = {id} AND DATE(CreatedOn) {FROM_TODAY}
             ";
            GetItem(cmd2, ref p, (dataRecord, user) =>
            {
                p.RevenueGeneratedToday = dataRecord.GetDecimal(0);
            });

            string cmd3 = $@"
                    SELECT SUM(Quantity*UnitPrice) FROM OrderLine   
                    WHERE CreatedBy = {id} AND DATE(CreatedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem(cmd3, ref p, (dataRecord, user) =>
            {
                p.RevenueGeneratedPast7Days = dataRecord.GetDecimal(0);
            });

            string cmd4 = $@"
                    SELECT SUM(Quantity*UnitPrice) FROM OrderLine  
                    WHERE CreatedBy = {id}
             ";
            GetItem(cmd4, ref p, (dataRecord, user) =>
            {
                p.RevenueGeneratedOverall = dataRecord.GetDecimal(0);
            });

            return await Task.FromResult(p);
        }

        async Task<IEnumerable<Bartender>> IDataStore<Bartender>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(bartenders);
        }

        async Task<IEnumerable<Bartender>> IDataStore<Bartender>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(bartenders.Where(c => (c.LastName.ToLowerInvariant().Contains(query)) || (c.FirstName.ToLowerInvariant().Contains(query))));
        }
        #endregion

    }
}