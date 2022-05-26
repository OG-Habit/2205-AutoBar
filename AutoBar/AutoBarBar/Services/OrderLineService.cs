using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrderLineService))]
namespace AutoBarBar.Services
{
    public class OrderLineService : BaseService, IOrderLineService
    {
        public async Task<IEnumerable<OrderLine>> GetOrderLines(string IDs)
        {
            List<OrderLine> orderLines = new List<OrderLine>();
            string cmd = $"SELECT * FROM OrderLine WHERE OrderID IN ({IDs})";

            GetItems<OrderLine>(cmd, (dataRecord, ol) =>
            {
                ol.ID = dataRecord.GetInt32(0);
                ol.OrderID = dataRecord.GetInt32(1);
                ol.ProductID = dataRecord.GetInt32(2);
                ol.UnitPrice = dataRecord.GetDecimal(3);
                ol.Quantity = dataRecord.GetInt32(4);
                ol.CreatedOnForUI = dataRecord.GetValue(6).ToString();
                orderLines.Add(ol);
            });

            return await Task.FromResult(orderLines);
        }

        public Task AddOrderLines(string orderLines, int customerID, decimal newBalance)
        {
            string cmd = $@"
                INSERT INTO OrderLine(`OrderID`, `ProductID`, `UnitPrice`, `Quantity`, `CreatedBy`, `CreatedOn`, `IsCompleted`)
                VALUES {orderLines};
                UPDATE Customers
                SET Balance = {newBalance}
                WHERE ID = {customerID};
            ";
            AddItem(cmd);
            return Task.CompletedTask;
        }
    }
}
