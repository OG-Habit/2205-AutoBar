using AutoBarBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar.Services
{
    public interface IRevenueService
    {
        Task<Revenue> GetRevenues();
    }
}
