using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public interface IActiveTabService
    {
        Task<IEnumerable<ActiveTab>> GetActiveTabs();
        Task<bool> CheckExistingTabs(string customerIDs);
        Task<ActiveTab> CreateActiveTab(string qrKey, string customerIDs);
        Task AddBalance(int customerID, int bartenderID, decimal amount, string dateTime);
        Task CloseTab(int customerID, Order o, Reward r, int hasReward, string dateTime);
    }
}
