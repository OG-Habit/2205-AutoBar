using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public interface IOrderLineService
    {
        Task<IEnumerable<OrderLine>> GetOrderLines(string IDs);
        Task AddOrderLines(string orderLines, int customerID, decimal newBalance);
    }
}
