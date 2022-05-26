using AutoBarBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBarBar.Helpers
{
    public class DataClassBartender : BaseViewModel
    {
        private DataClassBartender() { }

        static DataClassBartender _instance;
        public static DataClassBartender GetInstance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new DataClassBartender();
                    return _instance;
                }

                return _instance;
            }
        }
    }
}
