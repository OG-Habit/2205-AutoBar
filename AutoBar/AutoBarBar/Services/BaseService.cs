using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using static AutoBarBar.Constants;

namespace AutoBarBar.Services
{
    public class BaseService
    {
        protected void GetItems<T>(string cmd, Action<DbDataRecord, T> action)
        {
            try
            {
                using (var conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (var command = new MySqlCommand(cmd, conn))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var list = reader.GetEnumerator();
                            while (list.MoveNext())
                            {
                                T temp = (T)Activator.CreateInstance(typeof(T));
                                DbDataRecord dataRecord = (DbDataRecord)list.Current;
                                // All of the code above action() are required to query from the remote database.
                                // action() - what you want to do with the query result

                                action(dataRecord, temp);
                            }
                        }
                    }
                }
            }
            // Tip: If the app breaks down, place the red dot on the "var a = e.Message" line.
            catch (Exception e)
            {
                var a = e.Message;
            }
        }

        protected void AddItem<T>(string cmd, Action<DbDataRecord, T> action)
        {
            try
            {
                using (var conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (var command = new MySqlCommand(cmd, conn))
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var list = reader.GetEnumerator();
                            while (list.MoveNext())
                            {
                                T temp = (T)Activator.CreateInstance(typeof(T));
                                DbDataRecord dataRecord = (DbDataRecord)list.Current;

                                action(dataRecord, temp);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                var a = e.Message;
            }
        }

        protected void DeleteItem<T>(string cmd, Action<DbDataRecord, T> action)
        {

        }

        protected void UpdateItem(string cmd, Action action = null)
        {
            try
            {
                using (var conn = new MySqlConnection(CONNECTION_STRING))
                {
                    conn.Open();
                    using (var command = new MySqlCommand(cmd, conn))
                    {
                        command.Connection = conn;
                        command.CommandText = cmd;
                        command.ExecuteNonQuery();

                        action?.Invoke();
                    }
                }
            }
            catch (Exception e)
            {
                var a = e.Message;
            }
        }
    }
}
