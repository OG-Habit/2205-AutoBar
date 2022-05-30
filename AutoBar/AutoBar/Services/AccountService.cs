using AutoBar.Models;
using AutoBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountService))]
namespace AutoBar.Services
{
    public class AccountService : BaseService, IAccountService
    {
        Customer c = new Customer();
        public async Task<Customer> LoginCustomer(string email, string password)
        {
            string cmd = $@"
                SELECT Users.ID, Users.FirstName, Users.LastName, Users.Email, Users.ImageLink, 
                Customers.QRKey, Customers.ID, Customers.Balance, Customers.Points, Customers.CardStatus
                FROM Users
                INNER JOIN Customers
                ON Users.ID = Customers.UserID
                WHERE
                (Users.Email = ""{email}"" AND Users.Password = ""{password}"" AND Users.UserType=3)
                LIMIT 1
            ";

            GetItem<Customer>(cmd, ref c, (dataRecord, user) =>
            {
                c.UserDetails = new User();
                c.UserDetails.ID = dataRecord.GetInt32(0);
                c.UserDetails.FirstName = dataRecord.GetString(1);
                c.UserDetails.LastName = dataRecord.GetString(2);
                c.UserDetails.FullName = string.Concat(c.UserDetails.FirstName, c.UserDetails.LastName);
                c.UserDetails.Email = dataRecord.GetString(3);
                c.UserDetails.ImageLink = dataRecord.GetValue(4).ToString();
                c.QRKey = dataRecord.GetString(5);
                c.ID = dataRecord.GetInt32(6);
                c.Balance = dataRecord.GetDecimal(7);
                c.Points = dataRecord.GetDecimal(8);
                c.CardStatus = dataRecord.GetInt32(9);

            });

            return await Task.FromResult(c);
        }
    }
}
