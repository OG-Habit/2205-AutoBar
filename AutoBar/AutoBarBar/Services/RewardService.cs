using AutoBarBar.Models;
using AutoBarBar.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(RewardService))]
namespace AutoBarBar.Services
{
    public class RewardService : BaseService, IRewardService
    {
        public async Task<List<Reward>> GetRewards()
        {
            List<Reward> rewards = new List<Reward>();
            string cmd = $@"
                SELECT ID, Name, Points
                FROM Rewards
                WHERE IsDeleted = 0;
            ";

            GetItems<Reward>(cmd, (dataStore, reward) =>
            {
                reward.ID = dataStore.GetInt32(0);
                reward.Name = dataStore.GetString(1);
                reward.Points = dataStore.GetDecimal(2);

                rewards.Add(reward);
            });

            return await Task.FromResult(rewards);
        }
    }
}