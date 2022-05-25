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
        readonly List<OrderLine> orderLines = new List<OrderLine>();
        public async Task<IEnumerable<OrderLine>> GetOrderLines(string IDs)
        {
            //if(IDs.Count == 0)
            //{
            //    return orderLines;
            //}

            //string str = "" + IDs[0];
            //for(var i = 1; i < IDs.Count; i++)
            //{
            //    str += ", " + IDs[i];
            //}

            string cmd = $"SELECT * FROM OrderLine WHERE OrderID IN ({IDs})";

            GetItems<OrderLine>(cmd, (dataRecord, ol) =>
            {
                ol.ID = dataRecord.GetInt32(0);
                ol.OrderID = dataRecord.GetInt32(1);
                ol.ProductID = dataRecord.GetInt32(2);
                ol.UnitPrice = dataRecord.GetDecimal(3);
                ol.Quantity = dataRecord.GetInt32(4);
                ol.CreatedOn = dataRecord.GetValue(6).ToString();
                orderLines.Add(ol);
            });

            return await Task.FromResult(orderLines);
        }

        public async Task<IEnumerable<OrderLine>> AddOrderLine()
        {
            string str = string.Empty;

            string cmd = $@"
                
            ";

            GetItems<OrderLine>(cmd, (dataRecord, ol) =>
            {
                ol.ID = dataRecord.GetInt32(0);
                ol.OrderID = dataRecord.GetInt32(1);
                ol.ProductID = dataRecord.GetInt32(2);
                ol.UnitPrice = dataRecord.GetDecimal(3);
                ol.Quantity = dataRecord.GetInt32(4);
                ol.CreatedOn = dataRecord.GetValue(6).ToString();
                orderLines.Add(ol);
            });

            return await Task.FromResult(orderLines);
        }
    }
}
