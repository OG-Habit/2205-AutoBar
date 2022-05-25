using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBar
{
    public static class Constants
    {
        public static readonly string SERVER = "sql6.freemysqlhosting.net";
        public static readonly string USERID = "sql6494729";
        public static readonly string PASSWORD = "gEB2fyY5T4";
        public static readonly string DATABASE = "sql6494729";
        public static readonly string CONNECTION_STRING = new MySqlConnectionStringBuilder
        {
            Server = SERVER,
            UserID = USERID,
            Password = PASSWORD,
            Database = DATABASE
        }.ConnectionString;

        public static readonly int IMG_QR_LENGTH = 500;
    }
}
