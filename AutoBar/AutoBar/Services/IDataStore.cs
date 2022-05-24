using AutoBar.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoBar.Services
{
    public interface IDataStore<T>
    {
        Task<T> GetItemAsync(string id);
        Task<T> GetTodayResults(DateTime today);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetSearchResults(string query);
    }
}
