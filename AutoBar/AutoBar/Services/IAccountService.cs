using AutoBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBar.Services
{
    public interface IAccountService
    {
        Task<Customer> LoginCustomer(string email, string password);

        Task<User> SignUpCustomer(string FirstName, string LastName, string Email, string Password, string Contact, string Birthday, string Sex);
    }
}
