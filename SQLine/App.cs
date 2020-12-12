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
        public static string _currentDatabase = string.Empty;
        public static string _serverName = string.Empty;
        public static List<string> _databases = new List<string>();

        public void MainMenu()
        {
            Console.WriteLine($"Enter a server name to connect to or localhost to connect to an instance locally");
            Console.WriteLine($"Write 'exit' to exit the app at any time");

            var server = ReadPrompt();

            if (!string.IsNullOrEmpty(server))
            {
                Connect(server);
            }
        }

        public void Connect(string serverName)
        {
            Console.WriteLine($"Connecting to {serverName}");
            GetDatabases(serverName);
            _serverName = serverName;
        }

        public void GetDatabases(string serverName)
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
        }

        public string ReadPrompt()
        {
            if (string.IsNullOrEmpty(_serverName))
            {
                Console.Write("-> ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(_serverName);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(" -> ");
            }
            
            string result = Console.ReadLine().Trim();

            if (string.Equals(result, "exit"))
            {
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
            }

            return result;
        }
    }
}
