using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

/*
    * May you do good and not evil.
    * May you find forgiveness for yourself and forgive others.
    * May you share freely, never taking more than you give.
    * 
    * Obediently yours.
*/

namespace SQLine
{
    class App
    {
        public static string _currentDatabase;
        public static string _serverName;
        public static List<string> _databases = new List<string>();
        public static AppMode _mode;

        public App()
        {
            _mode = AppMode.PendingConnection;
        }

        internal static void MainMenu()
        {
            Console.WriteLine($"Enter a server name to connect to or localhost to connect to an instance locally");
            Console.WriteLine($"Connection will use Trusted Connection");
            Program.ShowPrefix();
        }

        internal static void Connect(string serverName)
        {
            Console.WriteLine($"Connecting to {serverName}");
            _serverName = serverName;
            GetDatabases(serverName);
        }

        internal static void ParseCommand(string command)
        {
            if ((App._mode == AppMode.ConnectedToServer || App._mode == AppMode.UsingDatabase) && command.StartsWith("use "))
            {
                string dbName = command.Replace("use", string.Empty).Trim();
                SetDatabase(dbName);
                Program.ShowPrefix();
            }
        }

        internal static void SetDatabase(string databaseName)
        {
            _currentDatabase = databaseName;
            App._mode = AppMode.UsingDatabase;
        }

        internal static void GetDatabases(string serverName)
        {
            var connString = $"Server={serverName};Database=master;Trusted_Connection = True;";
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand($"SELECT * FROM sys.databases", conn))
            {
                conn.Open();
                _databases.Clear();
                Console.WriteLine($"Listing databases...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dbName = reader["name"].ToString();
                        _databases.Add(dbName);
                        Console.WriteLine($"- {dbName}");
                    }
                }
            }

            _mode = AppMode.ConnectedToServer;
            Program.ShowPrefix();
        }

    }
}
