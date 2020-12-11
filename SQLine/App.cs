using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQLine
{
    class App
    {
        public static bool KeepRunning = true;
        public static string currentDatabase = string.Empty;

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

            var connString = $"Server={serverName};Database=master;Trusted_Connection = True;";

            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand($"SELECT * FROM sys.databases", conn))
            {
                conn.Open();
                Console.WriteLine($"Listing databases...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("- " + reader["name"].ToString());
                    }
                }
            }
        }

        private string ReadPrompt()
        {
            Console.Write("-> ");
            string result = Console.ReadLine();

            if (string.Equals(result, "exit"))
            {
                KeepRunning = false;
                Console.WriteLine("Exiting...");
            }

            return result;
        }
    }
}
