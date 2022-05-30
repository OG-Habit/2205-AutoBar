using AutoBarBar.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public class MockDataStore : BaseService, IDataStore<Customer>, IDataStore<Product>, IDataStore<OrderLine>, IDataStore<Order>, IDataStore<Reward>, IDataStore<Bartender>
    {
        readonly List<Customer> customers;
        readonly List<Product> products;
        readonly List<OrderLine> orderLines;
        readonly List<Order> orders;
        readonly List<Reward> rewards;
        readonly List<Bartender> bartenders;

        public MockDataStore()
        {
            customers = new List<Customer>();
            products = new List<Product>();
            orderLines = new List<OrderLine>();
            orders = new List<Order>();
            rewards = new List<Reward>();
            bartenders = new List<Bartender>();

            string cmd1 = $@"
                SELECT c.ID, CONCAT(u.FirstName,"" "",u.LastName) AS ""Name"", u.Birthday, u.CreatedOn, u.MobileNumber, c.Balance, u.Email, u.Sex, c.Points 
                FROM Customers c, Users u
                WHERE u.ID=c.UserID
            "; //the customers u.created on is just temporary

            GetItems<Customer>(cmd1, (dataRecord, c) =>
            {
                c.ID = dataRecord.GetInt32(0);
                c.Name = dataRecord.GetString(1);
                c.Birthday = dataRecord.GetDateTime(2);
                c.CardIssued = dataRecord.GetDateTime(3);
                c.Contact = dataRecord.GetString(4);
                c.CurrentBalance = dataRecord.GetDouble(5);
                c.Email = dataRecord.GetString(6);
                c.Sex = dataRecord.GetString(7);
                c.Points = 100;
                c.ImageLink = "default_pic.png";
                customers.Add(c);
            });
            //{
            //    new Customer { Id = "1", Name = "Adam Smith",  = Convert.ToDateTime("Jan 1, 2001"), CardIssued = Convert.ToDateTime("Jan 2, 2010"), Contact = "09123294756", CurrentBalance = 1000, Email = "adamsmith@gmail.com", Sex="Male", TotalPoints="100", ImageLink = "default_pic.png", Status="Member"},
            //    new Customer { Id = "2", Name = "Bam Carousel", Birthday = Convert.ToDateTime("Feb 1, 2001"), CardIssued = Convert.ToDateTime("Feb 2, 2010"), Contact = "09123864756", CurrentBalance = 2000, Email = "bamcarousel@gmail.com", Sex="Male", TotalPoints="200", ImageLink = "default_pic.png", Status="Member"},
            //    new Customer { Id = "3", Name = "Caroline Smith", Birthday = Convert.ToDateTime("Mar 1, 2001"), CardIssued = Convert.ToDateTime("Mar 2, 2010"), Contact = "09123294756", CurrentBalance = 1500, Email = "caroline@gmail.com", Sex="Female", TotalPoints="300", ImageLink = "default_pic.png", Status="Member"},
            //    new Customer { Id = "4", Name = "Diana Wonderwoman", Birthday = Convert.ToDateTime("Apr 1, 2001"), CardIssued = Convert.ToDateTime("Apr 2, 2010"), Contact = "09123294756", CurrentBalance = 3000, Email = "diana@gmail.com", Sex="Female", TotalPoints="300", ImageLink = "default_pic.png", Status="Member"}
            //};
            

            //{
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Apple", Description = "The apple is red, plump, and fresh", ImageLink = "default_pic.png", Price = 45.50 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Beans", Description = "The bean is green, long, and fresh", ImageLink = "default_pic.png", Price = 95.50 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Carrots", Description = "The carrot is orange, healthy, and fresh", ImageLink = "default_pic.png", Price = 60.75 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Duck", Description = "The duck is tasty, juicy, and free range", ImageLink = "default_pic.png", Price = 399.99 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            //    new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            //};

            //
            //{
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=3, CreatedOn = "7:30PM", OrderId="10", ProductImgUrl="default_pic.png"}, 
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Beans", Price=95.50, Quantity=2, CreatedOn = "7:30PM", OrderId="10", ProductImgUrl="default_pic.png"},
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Duck", Price=399.99, Quantity=1, CreatedOn = "8:30PM", OrderId="10", ProductImgUrl="default_pic.png"},
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Egg", Price=10.00, Quantity=10, CreatedOn = "10:30PM", OrderId="10", ProductImgUrl="default_pic.png"},
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Carrots", Price=60.75, Quantity=3, CreatedOn = "8:30PM", OrderId="10", ProductImgUrl="default_pic.png"},
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=5, CreatedOn = "8:30PM", OrderId="10", ProductImgUrl="default_pic.png"},
            //    new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Bam Carousel", ProductName="Apple", Price=45.50, Quantity=5, CreatedOn = "8:30PM", OrderId="11", ProductImgUrl="default_pic.png"}
            //};

            //orders = new List<Order>()
            //{
            //    new Order { Id = "10", OpenedOn = DateTime.Today, ClosedOn = DateTime.Today, CustomerName="Adam Smith", TotalPrice=1236.25, PointsEarned = 100, OrderStatus=false, CustomerId="1", BartenderName="Bartender One", Reward="No Reward"},
            //    new Order { Id = "11", OpenedOn = DateTime.Today, ClosedOn = DateTime.Today, CustomerName="Bam Carousel", TotalPrice=227.5, PointsEarned = 0, OrderStatus=false, CustomerId="2", BartenderName="Bartender Three", Reward="No Reward"}
            //};

            //rewards = new List<Reward>()
            //{
            //    new Reward { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Points = 100.00 },
            //    new Reward { Id = Guid.NewGuid().ToString(), Name = "Apple", Description = "The apple is red, plump, and fresh", ImageLink = "default_pic.png", Points = 400.00 }
            //};

            //bartenders = new List<Bartender>()
            //{
            //    new Bartender { Id = Guid.NewGuid().ToString(), Name = "Bartender One", Birthday = Convert.ToDateTime("Jan 1, 2001"), Contact = "09123294756", Email = "one@gmail.com", Sex="Male", ImageLink = "default_pic.png"},
            //    new Bartender { Id = Guid.NewGuid().ToString(), Name = "Bartender Two", Birthday = Convert.ToDateTime("Feb 1, 2001"), Contact = "09123864756", Email = "two@gmail.com", Sex="Male", ImageLink = "default_pic.png"},
            //    new Bartender { Id = Guid.NewGuid().ToString(), Name = "Bartender Three", Birthday = Convert.ToDateTime("Mar 1, 2001"), Contact = "09123294756", Email = "three@gmail.com", Sex="Female", ImageLink = "default_pic.png"},
            //    new Bartender { Id = Guid.NewGuid().ToString(), Name = "Bartender Four", Birthday = Convert.ToDateTime("Apr 1, 2001"), Contact = "09123294756", Email = "four@gmail.com", Sex="Female", ImageLink = "default_pic.png"}
            //};
        }

        #region Customer
        public async Task<bool> AddItemAsync(Customer item)
        {
            customers.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Customer item)
        {
            //var oldItem = customers.Where((Customer arg) => arg.Id == item.Id).FirstOrDefault();
            //customers.Remove(oldItem);
            //customers.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //var oldItem = customers.Where((Customer arg) => arg.Id == id).FirstOrDefault();
            //customers.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Customer> IDataStore<Customer>.GetItemAsync(string id)
        {
            //return await Task.FromResult(customers.FirstOrDefault(s => s.Id == id));
            return await Task.FromResult(customers.First());
        }

        async Task<IEnumerable<Customer>> IDataStore<Customer>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(customers);
        }

        async Task<IEnumerable<Customer>> IDataStore<Customer>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(customers.Where(c => c.LastTransactionAt.ToLowerInvariant().Contains(query)));
        }
        #endregion

        #region Product
        public async Task<bool> AddItemAsync(Product item)
        {
            products.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Product item)
        {
            var oldItem = products.Where((Product arg) => arg.ID == item.ID).FirstOrDefault();
            products.Remove(oldItem);
            products.Add(item);

            return await Task.FromResult(true);
        }

        async Task<bool> IDataStore<Product>.DeleteItemAsync(string id)
        {
            //var oldItem = products.Where((Product arg) => arg.Id == id).FirstOrDefault();
            //products.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Product> IDataStore<Product>.GetItemAsync(string id)
        {
            //return await Task.FromResult(products.FirstOrDefault(s => s.Id == id));
            
            // feel free to remove, just added this para i can run the app
            return await Task.FromResult(products[0]);
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

        async Task<OrderLine> IDataStore<OrderLine>.GetItemAsync(string id)
        {
            //return await Task.FromResult(orderLines.FirstOrDefault(s => s.Id == id));
            return await Task.FromResult(orderLines.FirstOrDefault());
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orderLines);
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            //return await Task.FromResult(orderLines.Where(c => c.OrderId.Contains(query)));
            return await Task.FromResult(orderLines);
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

        async Task<Order> IDataStore<Order>.GetItemAsync(string id)
        {
            //return await Task.FromResult(orders.FirstOrDefault(s => s.Id == id));
            return await Task.FromResult(orders.FirstOrDefault());
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
            rewards.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Reward item)
        {
            //var oldItem = rewards.Where((Reward arg) => arg.Id == item.Id).FirstOrDefault();
            //rewards.Remove(oldItem);
            rewards.Add(item);

            return await Task.FromResult(true);
        }

        async Task<bool> IDataStore<Reward>.DeleteItemAsync(string id)
        {
            //var oldItem = rewards.Where((Reward arg) => arg.Id == id).FirstOrDefault();
            //rewards.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Reward> IDataStore<Reward>.GetItemAsync(string id)
        {
            //return await Task.FromResult(rewards.FirstOrDefault(s => s.Id == id));
            return rewards[0];
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
            bartenders.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Bartender item)
        {
            var oldItem = bartenders.Where((Bartender arg) => arg.Id == item.Id).FirstOrDefault();
            bartenders.Remove(oldItem);
            bartenders.Add(item);

            return await Task.FromResult(true);
        }

        async Task<bool> IDataStore<Bartender>.DeleteItemAsync(string id)
        {
            var oldItem = bartenders.Where((Bartender arg) => arg.Id == id).FirstOrDefault();
            bartenders.Remove(oldItem);

            return await Task.FromResult(true);
        }

        async Task<Bartender> IDataStore<Bartender>.GetItemAsync(string id)
        {
            return await Task.FromResult(bartenders.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<Bartender>> IDataStore<Bartender>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(bartenders);
        }

        async Task<IEnumerable<Bartender>> IDataStore<Bartender>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(bartenders.Where(c => c.Name.ToLowerInvariant().Contains(query)));
        }
        #endregion
    }
}