using AutoBar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBar.Services
{
    public interface ILostCardService
    {
        Task ReportLostCard();
        Task RequestNewCard();
    }
}
