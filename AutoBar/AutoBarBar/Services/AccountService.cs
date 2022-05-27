using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountService))]
namespace AutoBarBar.Services
{
    public class AccountService : BaseService, IAccountService
    {
        User u = new User() { UserType = 0 };
        public async Task<User> LoginUser(string email, string password)
        {
            string cmd = $@"
                SELECT Users.ID, Users.FirstName, Users.LastName, Users.Email, Users.ImageLink, Users.UserType,
                CASE Users.UserType
                WHEN 1 THEN Admins.ID
                WHEN 2 THEN Bartenders.ID
                END AS ""StaffID""
                FROM Users, Bartenders, Admins
                WHERE
                (Users.Email = ""{email}"" AND Users.Password = ""{password}"") AND
                (Users.ID = Bartenders.UserID OR Users.ID = Admins.UserID)
                LIMIT 1
            ";

            GetItem<User>(cmd, ref u, (dataRecord, user) =>
            {
                u.ID = dataRecord.GetInt32(0);
                u.FirstName = dataRecord.GetString(1);
                u.LastName = dataRecord.GetString(2);
                u.Email = dataRecord.GetString(3);
                u.ImageLink = dataRecord.GetValue(4).ToString();
                u.UserType = dataRecord.GetInt32(5);
                u.StaffID = dataRecord.GetInt32(6);
            });

            return await Task.FromResult(u);
        }
    }
}
