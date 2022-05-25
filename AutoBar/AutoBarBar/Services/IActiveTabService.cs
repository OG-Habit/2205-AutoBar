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
        Task<ActiveTab> CreateActiveTab(string qrKey);
        Task AddBalance(int customerID, decimal newBalance, string dateTime);
    }
}
