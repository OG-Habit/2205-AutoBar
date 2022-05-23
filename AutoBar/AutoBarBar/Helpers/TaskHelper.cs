using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace AutoBarBar
{
    public static class TaskHelper
    {
        public static async void Await(this Task T)
        {
            try
            {
                await T;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
