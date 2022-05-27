using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public interface IRewardService
    {
        Task<List<Reward>> GetRewards();
    }
}
