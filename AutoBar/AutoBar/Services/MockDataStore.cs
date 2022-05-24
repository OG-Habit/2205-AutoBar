using AutoBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBar.Services
{
    public class MockDataStore : IDataStore<Product>, IDataStore<OrderLine>, IDataStore<Order>, IDataStore<Reward>
    {
        readonly List<Product> products;
        readonly List<OrderLine> orderLines;
        readonly List<Order> orders;
        readonly List<Reward> rewards;

        public MockDataStore()
        {
            products = new List<Product>()
            {
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

            orderLines = new List<OrderLine>()
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

            orders = new List<Order>()
            {
                new Order { Id = "8", OpenedOn = DateTime.Parse("5/21/2022 19:30:00"), ClosedOn = DateTime.Parse("5/21/2022 10:30:00PM"), CustomerName="Adam Smith", TotalPrice=45.50, PointsEarned = 0, OrderStatus=false, CustomerId="1", BartenderName="Bartender One", Reward="No Reward"},
                new Order { Id = "9", OpenedOn = DateTime.Parse("5/22/2022 19:30:00"), ClosedOn = DateTime.Parse("5/22/2022 9:30:00PM"), CustomerName="Adam Smith", TotalPrice=95.50, PointsEarned = 0, OrderStatus=false, CustomerId="1", BartenderName="Bartender Four", Reward="No Reward"},
                new Order { Id = "10", OpenedOn = DateTime.Today, ClosedOn = DateTime.Today, CustomerName="Adam Smith", TotalPrice=1236.25, PointsEarned = 100, OrderStatus=false, CustomerId="1", BartenderName="Bartender One", Reward="No Reward"},
                new Order { Id = "11", OpenedOn = DateTime.Today, ClosedOn = DateTime.Today, CustomerName="Bam Carousel", TotalPrice=227.5, PointsEarned = 0, OrderStatus=false, CustomerId="2", BartenderName="Bartender Three", Reward="No Reward"}
            };

            rewards = new List<Reward>()
            {
                new Reward { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Points = 100.00 },
                new Reward { Id = Guid.NewGuid().ToString(), Name = "Apple", Description = "The apple is red, plump, and fresh", ImageLink = "default_pic.png", Points = 400.00 }
            };
        }

        #region Product
        public async Task<Product> GetItemAsync(string id)
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
        async Task<OrderLine> IDataStore<OrderLine>.GetItemAsync(string id)
        {
            return await Task.FromResult(orderLines.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orderLines);
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetSearchResults(string query)
        {
            return await Task.FromResult(orderLines.Where(s => s.OrderId == query));
        }

        Task<OrderLine> IDataStore<OrderLine>.GetTodayResults(DateTime today)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Order
        async Task<Order> IDataStore<Order>.GetItemAsync(string id)
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
            return await Task.FromResult(orders.Where(c => c.CustomerId.ToLowerInvariant().Contains(query)));
        }

        async Task<Order> IDataStore<Order>.GetTodayResults(DateTime today)
        {
            return await Task.FromResult(orders.FirstOrDefault(s => s.OpenedOn.Date == today.Date));
        }
        #endregion

        #region Reward
        async Task<Reward> IDataStore<Reward>.GetItemAsync(string id)
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
        #endregion

    }
}