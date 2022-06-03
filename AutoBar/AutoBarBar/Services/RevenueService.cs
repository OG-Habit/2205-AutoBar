using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static AutoBarBar.Constants;

[assembly: Dependency(typeof(RevenueService))]
namespace AutoBarBar.Services
{
    public class RevenueService : BaseService, IRevenueService
    {
        Revenue r = new Revenue();
        public async Task<Revenue> GetRevenues()
        {
            string cmd1 = $@"
                    SELECT COUNT(ID) AS a FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_TODAY}
             ";
            GetItem<Revenue>(cmd1, ref r, (dataRecord, user) =>
            {
                r.TotalOrders = dataRecord.GetInt32(0);
            });

            string cmd2 = $@"
                    SELECT SUM(TotalPrice) AS b FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_TODAY}
             ";
            GetItem<Revenue>(cmd2, ref r, (dataRecord, user) =>
            {
                r.TotalRevenue = dataRecord.GetDouble(0);
            });

            string cmd3 = $@"
                    SELECT COUNT(ID) AS c FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem<Revenue>(cmd3, ref r, (dataRecord, user) =>
            {
                r.TotalWeekOrders = dataRecord.GetInt32(0);
            });

            string cmd4 = $@"
                    SELECT SUM(TotalPrice) AS d FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem<Revenue>(cmd4, ref r, (dataRecord, user) =>
            {
                r.TotalWeekRevenue = dataRecord.GetDouble(0);
            });

            return await Task.FromResult(r);
        }
    }
}
