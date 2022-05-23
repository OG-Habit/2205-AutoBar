using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public class MockDataStore : IDataStore<Item>, IDataStore<Customer>, IDataStore<Product>, IDataStore<OrderLine>, IDataStore<Order>, IDataStore<Reward>
    {
        readonly List<Item> items;
        readonly List<Customer> customers;
        readonly List<Product> products;
        readonly List<OrderLine> orderLines;
        readonly List<Order> orders;
        readonly List<Reward> rewards;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description.",
                           C_Name="ABC DEF", B_Name="ZYX WVU", Drink="Champagne", Reward="First Reward", Image="default_pic",
                           Status="Member", Email="zxy@email.com", Price="100.00", Points="100", Time=DateTime.Today },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description = "This is an item description.",
                           C_Name="GHI JKL", B_Name="TSR QPO", Drink="Wine", Reward="Second Reward", Image="default_pic",
                           Status="Guest", Email="tsr@email.com", Price="100.00", Points="100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description = "This is an item description.",
                           C_Name="MNO PQR", B_Name="NML KJI", Drink="Tequila", Reward="Third Reward", Image="default_pic",
                           Status="Guest", Email="nml@email.com", Price="100.00", Points="100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description = "This is an item description.",
                           C_Name="STU VWX", B_Name="HGF EDC", Drink="Margarita", Reward="Fourth Reward", Image="default_pic",
                           Status="Member", Email="hgf@email.com", Price="100.00", Points="100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description = "This is an item description.",
                           C_Name="YZA BCD", B_Name="BAZ YXW", Drink="Mojito", Reward="Fifth Reward", Image="default_pic",
                           Status="Member", Email="baz@email.com", Price="100.00", Points="100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description = "This is an item description.",
                           C_Name="EFG HIJ", B_Name="VUT SRQ", Drink="Mimosa", Reward="Sixth Reward", Image="default_pic",
                           Status="Guest", Email="vut@email.com", Price="100.00", Points="100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Seventh item", Description = "This is an item description.",
                           C_Name="KLM NOP", B_Name="PON MLK", Drink="Pina Colada", Reward="Seventh Reward", Image="default_pic",
                           Status="Member", Email="pon@email.com", Price="100.00", Points="100", Time=DateTime.Today  }
            };

            customers = new List<Customer>()
            {
                new Customer { Id = Guid.NewGuid().ToString(), Name = "Adam Smith", Birthday = "Jan 1, 2001", CardIssued = "Jan 2, 2010", Contact = "09123294756", CurrentBalance = 1000, Email = "adamsmith@gmail.com", Sex="Male", TotalPoints="100"},
                new Customer { Id = Guid.NewGuid().ToString(), Name = "Bam Carousel", Birthday = "Feb 1, 2001", CardIssued = "Feb 2, 2010", Contact = "09123864756", CurrentBalance = 2000, Email = "bamcarousel@gmail.com", Sex="Male", TotalPoints="200"},
                new Customer { Id = Guid.NewGuid().ToString(), Name = "Caroline Smith", Birthday = "Mar 1, 2001", CardIssued = "Mar 2, 2010", Contact = "09123294756", CurrentBalance = 1500, Email = "caroline@gmail.com", Sex="Female", TotalPoints="300"},
                new Customer { Id = Guid.NewGuid().ToString(), Name = "Diana Wonderwoman", Birthday = "Apr 1, 2001", CardIssued = "Apr 2, 2010", Contact = "09123294756", CurrentBalance = 3000, Email = "diana@gmail.com", Sex="Female", TotalPoints="300"}
            };

            products = new List<Product>()
            {
                new Product { Id = Guid.NewGuid().ToString(), Name = "Apple", Description = "The apple is red, plump, and fresh", ImageLink = "default_pic.png", Price = 45.50 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Beans", Description = "The bean is green, long, and fresh", ImageLink = "default_pic.png", Price = 95.50 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Carrots", Description = "The carrot is orange, healthy, and fresh", ImageLink = "default_pic.png", Price = 60.75 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Duck", Description = "The duck is tasty, juicy, and free range", ImageLink = "default_pic.png", Price = 399.99 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
                new Product { Id = Guid.NewGuid().ToString(), Name = "Egg", Description = "The egg is big, dark orange, and fresh", ImageLink = "default_pic.png", Price = 10.00 },
            };

            orderLines = new List<OrderLine>()
            {
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=3, CreatedOn = "7:30PM"}, 
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Beans", Price=95.50, Quantity=2, CreatedOn = "7:30PM"},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Duck", Price=399.99, Quantity=1, CreatedOn = "8:30PM"},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Egg", Price=10.00, Quantity=10, CreatedOn = "10:30PM"},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Carrots", Price=60.75, Quantity=3, CreatedOn = "8:30PM"},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Adam Smith", ProductName="Apple", Price=45.50, Quantity=5, CreatedOn = "8:30PM"},
                new OrderLine { Id = Guid.NewGuid().ToString(), CustomerName = "Bam Carousel", ProductName="Apple", Price=45.50, Quantity=5, CreatedOn = "8:30PM"}
            };

            orders = new List<Order>()
            {
                new Order { Id = Guid.NewGuid().ToString(), OpenedOn = "7:25 PM", CustomerName="Adam Smith", TotalPrice=1236.25, PointsEarned = 100, OrderStatus=false},
                new Order { Id = Guid.NewGuid().ToString(), OpenedOn = "8:25 PM", CustomerName="Bam Carousel", TotalPrice=227.5, PointsEarned = 0, OrderStatus=false}
            };

            rewards = new List<Reward>()
            {
                new Reward { Id = Guid.NewGuid().ToString(), Name="Summer sale."},
                new Reward { Id = Guid.NewGuid().ToString(), Name="Winter sale."}
            };
        }
        #region Item
        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public Task<IEnumerable<Item>> GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Customer
        public async Task<bool> AddItemAsync(Customer item)
        {
            customers.Add(item);

            return await Task.FromResult(true);
        }

        public Task<bool> UpdateItemAsync(Customer item)
        {
            throw new NotImplementedException();
        }

        async Task<Customer> IDataStore<Customer>.GetItemAsync(string id)
        {
            return await Task.FromResult(customers.FirstOrDefault(s => s.Id == id));
        }

        async Task<IEnumerable<Customer>> IDataStore<Customer>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(customers);
        }

        async Task<IEnumerable<Customer>> IDataStore<Customer>.GetSearchResults(string query)
        {
            query = query.ToLowerInvariant();
            return await Task.FromResult(customers.Where(c => c.Name.ToLowerInvariant().Contains(query)));
        }
        #endregion

        #region Product
        public Task<bool> AddItemAsync(Product item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Product item)
        {
            throw new NotImplementedException();
        }

        Task<Product> IDataStore<Product>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
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

        Task<OrderLine> IDataStore<OrderLine>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orderLines);
        }

        Task<IEnumerable<OrderLine>> IDataStore<OrderLine>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
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

        Task<Order> IDataStore<Order>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<Order>> IDataStore<Order>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(orders);
        }

        Task<IEnumerable<Order>> IDataStore<Order>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Reward
        public Task<bool> AddItemAsync(Reward item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Reward item)
        {
            throw new NotImplementedException();
        }

        Task<Reward> IDataStore<Reward>.GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<Reward>> IDataStore<Reward>.GetItemsAsync(bool forceRefresh)
        {
            return await Task.FromResult(rewards);
        }

        Task<IEnumerable<Reward>> IDataStore<Reward>.GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}