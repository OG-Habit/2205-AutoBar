using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public class ActiveTabService : BaseService, IActiveTabService
    {
        List<ActiveTab> activeTabs = new List<ActiveTab>();

        public async Task<IEnumerable<ActiveTab>> GetActiveTabs()
        {
            string cmd = @"
                SELECT 
                Orders.ID, Orders.CustomerID, Orders.TotalPrice, Orders.PointsEarned, Orders.HasReward, Orders.OpenedOn,
                Users.ID, Users.FirstName, Users.LastName, Users.Sex, Users.Birthday, Users.MobileNumber, Users.Email, Users.ImageLink,
                Customers.ID, Customers.UserID, Customers.Balance, Customers.Points
                FROM Orders
                INNER JOIN Customers
                INNER JOIN Users
                ON Orders.CustomerID = Customers.ID AND Customers.UserID = Users.ID
                WHERE 
                Orders.OrderStatus = 1 AND 
                Users.IsDeleted = 0;
            ";

            GetItems<ActiveTab>(cmd, (dataRecord, activeTab) =>
            {
                activeTab.ATOrder.ID = dataRecord.GetInt32(0);
                activeTab.ATOrder.CustomerID = dataRecord.GetInt32(1);
                activeTab.ATOrder.TotalPrice = dataRecord.GetDouble(2);
                activeTab.ATOrder.PointsEarned = dataRecord.GetDecimal(3);
                activeTab.ATOrder.HasReward = dataRecord.GetInt32(4);
                activeTab.ATOrder.OpenedOn = dataRecord.GetString(5);

                activeTab.ATUser.ID = dataRecord.GetInt32(6);
                activeTab.ATUser.FirstName = dataRecord.GetString(7);
                activeTab.ATUser.LastName = dataRecord.GetString(8);
                activeTab.ATUser.Sex = dataRecord.GetInt32(9);
                activeTab.ATUser.Birthday = dataRecord.GetString(10);
                activeTab.ATUser.MobileNumber = dataRecord.GetString(11);
                activeTab.ATUser.Email = dataRecord.GetString(12);
                activeTab.ATUser.ImageLink = dataRecord.GetString(13);

                activeTab.ATCustomer.ID = dataRecord.GetInt32(14);
                activeTab.ATCustomer.UserID = dataRecord.GetInt32(15);
                activeTab.ATCustomer.Balance = dataRecord.GetDecimal(14);
                activeTab.ATCustomer.Points = dataRecord.GetDecimal(14);

                activeTabs.Add(activeTab);
            });

            return await Task.FromResult(activeTabs);
        }
    }
}
