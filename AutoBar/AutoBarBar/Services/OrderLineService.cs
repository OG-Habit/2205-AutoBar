using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public class OrderLineService : BaseService, IOrderLineService
    {
        readonly List<OrderLine> orderLines;
        public async Task<IEnumerable<OrderLine>> GetOrderLines(List<int> IDs)
        {
            string str = string.Empty;

            for(var i = 0; i < IDs.Count; i++)
            {
                str += IDs[i];
                
                if(i > 0)
                {
                    str += ", " + IDs[i];
                }
            }

            string cmd = $"SELECT * FROM OrderLine WHERE OrderID IN ({str})";

            return await Task.FromResult(orderLines);
        }
    }
}
