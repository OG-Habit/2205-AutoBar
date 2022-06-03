using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static AutoBarBar.Constants;
using System.Diagnostics;

[assembly: Dependency(typeof(RevenueService))]
namespace AutoBarBar.Services
{
    public class RevenueService : BaseService, IRevenueService
    {
        Revenue r = new Revenue();
        public async Task<Revenue> GetRevenues()
        {
            string cmd1 = $@"
                    SELECT IFNULL(COUNT(ID),0) FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_TODAY}
             ";
            GetItem<Revenue>(cmd1, ref r, (dataRecord, user) =>
            {
                r.TotalOrders = Convert.ToInt32(dataRecord.GetValue(0));
                Debug.WriteLine($"Number of orders today: {r.TotalOrders}");
            });

            string cmd2 = $@"
                    SELECT IFNULL(SUM(TotalPrice),0) FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_TODAY}
             ";
            GetItem<Revenue>(cmd2, ref r, (dataRecord, user) =>
            {
                r.TotalRevenue = Convert.ToDouble(dataRecord.GetValue(0));

                Debug.WriteLine($"Revenue today: {r.TotalRevenue}");
            });

            string cmd3 = $@"
                    SELECT IFNULL(COUNT(ID),0) FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem<Revenue>(cmd3, ref r, (dataRecord, user) =>
            {
                r.TotalWeekOrders = Convert.ToInt32(dataRecord.GetValue(0));
                Debug.WriteLine($"Number of orders past 7 days: {r.TotalWeekOrders}");
            });

            string cmd4 = $@"
                    SELECT IFNULL(SUM(TotalPrice),0) FROM Orders 
                    WHERE OrderStatus = 2 AND ClosedOn IS NOT NULL AND DATE(ClosedOn) {FROM_PAST_7_DAYS}
             ";
            GetItem<Revenue>(cmd4, ref r, (dataRecord, user) =>
            {
                r.TotalWeekRevenue = Convert.ToDouble(dataRecord.GetValue(0));
                Debug.WriteLine($"Revenue past 7 days: {r.TotalWeekRevenue}");
            });

            return await Task.FromResult(r);
        }
    }
}
