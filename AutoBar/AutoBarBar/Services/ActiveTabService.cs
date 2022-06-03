using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

[assembly: Dependency(typeof(ActiveTabService))]
namespace AutoBarBar.Services
{
    public class ActiveTabService : BaseService, IActiveTabService
    {
        public async Task<IEnumerable<ActiveTab>> GetActiveTabs()
        {
            List<ActiveTab> activeTabs = new List<ActiveTab>();
            string cmd = @"
                SELECT 
                Orders.ID, Orders.CustomerID, Orders.TotalPrice, Orders.PointsEarned, Orders.HasReward, Orders.OpenedOn,
                Users.ID, Users.FirstName, Users.LastName, Users.Sex, Users.Birthday, Users.MobileNumber, Users.Email, Users.ImageLink,
                Customers.ID, Customers.UserID, Customers.Balance, Customers.Points, Customers.CardStatus
                FROM Orders
                INNER JOIN Customers
                INNER JOIN Users
                ON Orders.CustomerID = Customers.ID AND Customers.UserID = Users.ID
                WHERE 
                (Orders.OrderStatus = 1 AND 
                Users.IsDeleted = 0);
            ";

            GetItems<ActiveTab>(cmd, (dataRecord, activeTab) =>
            {
                activeTab.ATOrder = new Order();
                activeTab.ATUser = new User();
                activeTab.ATCustomer = new Customer();

                activeTab.ATOrder.ID = dataRecord.GetInt32(0);
                activeTab.ATOrder.CustomerID = dataRecord.GetInt32(1);
                activeTab.ATOrder.TotalPrice = dataRecord.GetDouble(2);
                activeTab.ATOrder.PointsEarned = dataRecord.GetDecimal(3);
                activeTab.ATOrder.HasReward = dataRecord.GetInt32(4);
                activeTab.ATOrder.OpenedOn = dataRecord.GetDateTime(5).ToString();

                activeTab.ATUser.ID = dataRecord.GetInt32(6);
                activeTab.ATUser.FirstName = dataRecord.GetString(7);
                activeTab.ATUser.LastName = dataRecord.GetString(8);
                activeTab.ATUser.Sex = dataRecord.GetString(9);
                var bday = dataRecord.GetValue(10).ToString();
                activeTab.ATUser.Birthday = bday.Substring(0, bday.IndexOf(" "));
                activeTab.ATUser.MobileNumber = dataRecord.GetString(11);
                activeTab.ATUser.Email = dataRecord.GetString(12);
                activeTab.ATUser.ImageLink = dataRecord.GetValue(13).ToString();

                activeTab.ATCustomer.ID = dataRecord.GetInt32(14);
                activeTab.ATCustomer.UserID = dataRecord.GetInt32(15);
                activeTab.ATCustomer.Balance = dataRecord.GetDecimal(16);
                activeTab.ATCustomer.Points = dataRecord.GetDecimal(17);
                activeTab.ATCustomer.CardStatus = dataRecord.GetInt32(18);

                activeTabs.Add(activeTab);
            });

            return await Task.FromResult(activeTabs);
        }

        public async Task<ActiveTab> CreateActiveTab(string qrKey, string customerIDs, string dateTime)
        {
            ActiveTab activeTab = null;
            Order o = null;
            string condition = string.IsNullOrEmpty(customerIDs) ? "" : $"AND Customers.ID NOT IN({customerIDs})";
            string cmd = $@"
                SELECT 
                Users.ID, Users.FirstName, Users.LastName, Users.Sex, Users.Birthday, Users.MobileNumber, Users.Email, Users.ImageLink,
                Customers.ID, Customers.UserID, Customers.Balance, Customers.Points, Customers.CardStatus
                FROM Customers
                INNER JOIN Users
                ON Users.ID = Customers.UserID
                WHERE QRKey = ""{qrKey}"" 
                {condition}
                LIMIT 1;
            ";

            GetItem<ActiveTab>(cmd, ref activeTab, (dataRecord, at) =>
            {
                activeTab = new ActiveTab
                {
                    ATOrder = new Order(),
                    ATUser = new User(),
                    ATCustomer = new Customer()
                };

                activeTab.ATUser.ID = dataRecord.GetInt32(0);
                activeTab.ATUser.FirstName = dataRecord.GetString(1);
                activeTab.ATUser.LastName = dataRecord.GetString(2);
                activeTab.ATUser.Sex = dataRecord.GetString(3);
                var bday = dataRecord.GetValue(4).ToString();
                activeTab.ATUser.Birthday = bday.Substring(0, bday.IndexOf(" "));
                activeTab.ATUser.MobileNumber = dataRecord.GetString(5);
                activeTab.ATUser.Email = dataRecord.GetString(6);
                activeTab.ATUser.ImageLink = dataRecord.GetValue(7).ToString();

                activeTab.ATCustomer.ID = dataRecord.GetInt32(8);
                activeTab.ATCustomer.UserID = dataRecord.GetInt32(9);
                activeTab.ATCustomer.Balance = dataRecord.GetDecimal(10);
                activeTab.ATCustomer.Points = dataRecord.GetDecimal(11);
                activeTab.ATCustomer.CardStatus = dataRecord.GetInt32(12);
            });

            if(activeTab != null)
            {
                string cmd2 = $@"
                    INSERT INTO Orders(CustomerID, OpenedOn)
                    VALUES({activeTab.ATCustomer.ID},""{dateTime}"");
                ";
                AddItem(cmd2);

                cmd2 = $@"
                    SELECT Orders.ID, Orders.CustomerID, Orders.TotalPrice, Orders.PointsEarned, Orders.HasReward, Orders.OpenedOn
                    FROM Orders
                    WHERE (Orders.OrderStatus = 1 AND {activeTab.ATCustomer.ID} = Orders.CustomerID)
                    LIMIT 1;
                ";
                GetItem<Order>(cmd2, ref o, (dataRecord, order) =>
                {
                    o = new Order();
                    o.ID = dataRecord.GetInt32(0);
                    o.CustomerID = dataRecord.GetInt32(1);
                    o.TotalPrice = dataRecord.GetDouble(2);
                    o.PointsEarned = dataRecord.GetDecimal(3);
                    o.HasReward = dataRecord.GetInt32(4);
                    o.OpenedOn = dataRecord.GetDateTime(5).ToString();
                });

                activeTab.ATOrder = o;
            }

            return await Task.FromResult(activeTab);
        }

        public async Task<bool> CheckExistingTabs(string customerIDs)
        {
            string cmd = $@"
                SELECT Orders.CustomerID FROM Orders
                WHERE 
                Orders.OrderStatus = 1 AND
                Orders.CustomerID IN ({customerIDs}) 
                LIMIT 1;
            ";
            bool ans = false;

            GetItem<bool>(cmd, ref ans, (dataRecord, data) =>
            {
                ans = true;
            });

            return await Task.FromResult(ans);
        }

        public Task AddBalance(int customerID, int bartenderID, decimal amount, string dateTime)
        {
            string cmd = $@"
                UPDATE Customers
                SET Balance=Balance+{amount}, LastTransactionAt=""{dateTime}"" 
                WHERE ID={customerID};

                INSERT INTO Reloads (CustomerID, BartenderID, CreditedBalance, CreatedOn) 
                VALUES ({customerID},{bartenderID},{amount},""{dateTime}"");
            ";
            UpdateItem(cmd);
            return Task.CompletedTask;
        }

        public Task CloseTab(int customerID, Order o, Reward r, int hasReward, string dateTime)
        {
            string insertUsedReward = hasReward == 1 ? $@"
                INSERT INTO UsedRewards(CustomerID, RewardID, OrderID, PointsDeducted, CreatedOn)
                VALUES({customerID},{r.ID},{o.ID},{r.Points},""{dateTime}"");
            " : "";
            string cmd = $@"
                UPDATE Orders
                SET ClosedOn=""{dateTime}"",
                HasReward = {hasReward}, OrderStatus = 2
                WHERE ID={o.ID};

                {insertUsedReward}

                UPDATE Customers
                SET Points=Points-{r.Points},
                LastTransactionAt=""{dateTime}""
                WHERE ID={customerID};
            ";
            UpdateItem(cmd);
            return Task.CompletedTask;
        }
    }
}
