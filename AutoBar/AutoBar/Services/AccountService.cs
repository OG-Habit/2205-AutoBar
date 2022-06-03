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
                Customers.QRKey, Customers.Balance, Customers.Points
                FROM Users
                INNER JOIN Customers
                ON Users.ID = Customers.UserID
                WHERE
                (Users.Email = ""{email}"" AND Users.Password = ""{password}"")
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
                c.Balance = dataRecord.GetDecimal(6);
                c.Points = dataRecord.GetDecimal(7);
            });

            return await Task.FromResult(c);
        }

        public async Task<User> SignUpCustomer(string FirstName, string LastName, string Email, string Password, string Contact, string Birthday, string Sex)
        {
            throw new NotImplementedException();
        }
    }
}
