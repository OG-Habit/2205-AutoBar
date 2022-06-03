using AutoBar.Models;
using AutoBar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using Xamarin.Forms;

[assembly: Dependency(typeof(LostCardService))]
namespace AutoBar.Services
{
    public class LostCardService : BaseService, ILostCardService
    {


        private int UserID;

        public LostCardService()
        {
            SetUserID();
        }

        async void SetUserID()
        {
            UserID = Convert.ToInt32(await Xamarin.Essentials.SecureStorage.GetAsync("id"));
        }

        public Task ReportLostCard()
        {
            string cmd = $@"
                UPDATE Customers, Users
                SET CardStatus=4
                WHERE Customers.UserID = Users.ID AND Users.ID = {UserID};
            ";
            AddItem(cmd);
            return Task.CompletedTask;
        }

        public Task RequestNewCard()
        {
            string cmd = $@"
                UPDATE Customers, Users
                SET CardStatus=2
                WHERE Customers.UserID = Users.ID AND Users.ID = {UserID};
            ";
            AddItem(cmd);
            return Task.CompletedTask;
        }
    }
}