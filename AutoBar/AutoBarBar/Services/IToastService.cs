using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Services
{
    public interface IToastService
    {
        void ShowLongMessage(string message);
        void ShowShortMessage(string message);
    }
}
