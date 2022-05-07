using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description.",
                           C_Name="ABC DEF", B_Name="ZYX WVU", Drink="Champagne", Reward="First Reward", Image="default_pic",
                           Status="Member", Email="zxy@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description = "This is an item description.",
                           C_Name="GHI JKL", B_Name="TSR QPO", Drink="Wine", Reward="Second Reward", Image="default_pic",
                           Status="Guest", Email="tsr@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description = "This is an item description.",
                           C_Name="MNO PQR", B_Name="NML KJI", Drink="Tequila", Reward="Third Reward", Image="default_pic",
                           Status="Guest", Email="nml@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description = "This is an item description.",
                           C_Name="STU VWX", B_Name="HGF EDC", Drink="Margarita", Reward="Fourth Reward", Image="default_pic",
                           Status="Member", Email="hgf@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description = "This is an item description.",
                           C_Name="YZA BCD", B_Name="BAZ YXW", Drink="Mojito", Reward="Fifth Reward", Image="default_pic",
                           Status="Member", Email="baz@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description = "This is an item description.",
                           C_Name="EFG HIJ", B_Name="VUT SRQ", Drink="Mimosa", Reward="Sixth Reward", Image="default_pic",
                           Status="Guest", Email="vut@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today  },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Seventh item", Description = "This is an item description.",
                           C_Name="KLM NOP", B_Name="PON MLK", Drink="Pina Colada", Reward="Seventh Reward", Image="default_pic",
                           Status="Member", Email="pon@email.com", Price="PHP 100.00", Points="Points: 100", Time=DateTime.Today  }
            };
        }

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
    }
}