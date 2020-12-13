﻿using System;
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

        internal static void PendingConnectionHelpMenu()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter a server name to connect to or 'localhost' to connect to an instance locally");
            Console.WriteLine($"Connection will use default to Trusted Connection");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void MainMenu()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter a server name to connect to or 'localhost' to connect to an instance locally");
            Console.WriteLine($"Connection will use default to Trusted Connection");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void ConnectedToServerHelpMenu()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter 'use' followed by a database name to switch your session to a specific database");
            Console.WriteLine($"Type '? dbs' to list all databases on the server");
            Console.WriteLine($"Type '? dbs update' to update cache first and list all databases on the server");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void UsingDatabaseHelpMenu()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter 'use' followed by a database name to switch your session to a specific database");
            Console.WriteLine($"Type '? dbs' to list all databases on the server");
            Console.WriteLine($"Type '? dbs update' to update cache first and list all databases on the server");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void Connect(string serverName)
        {
            Console.WriteLine($"Connecting to {serverName}");
            _serverName = serverName;
            GetDatabases(serverName);
        }

        internal static void ParseCommand(string command)
        {
            if (App._mode == AppMode.UsingDatabase)
            {
                if (command == "?")
                {
                    UsingDatabaseHelpMenu();
                }

                if (command.StartsWith("use "))
                {
                    string dbName = command.Replace("use", string.Empty).Trim();
                    SetDatabase(dbName);
                }

                if (command == "? dbs")
                {
                    ListCachedDatabases();
                }

                if (command == "? dbs update")
                {
                    GetDatabases(_serverName);
                }
            }

            if (App._mode == AppMode.PendingConnection)
            {
                if (command == "?")
                {
                    PendingConnectionHelpMenu();
                }

            }

            if (App._mode == AppMode.ConnectedToServer)
            {
                if (command == "?")
                {
                    ConnectedToServerHelpMenu();
                }

                if (command.StartsWith("use "))
                {
                    string dbName = command.Replace("use", string.Empty).Trim();
                    SetDatabase(dbName);
                }

                if (command == "? dbs")
                {
                    ListCachedDatabases();
                }

                if (command == "? dbs update")
                {
                    GetDatabases(_serverName);
                }
            }
        }

        internal static void SetDatabase(string databaseName)
        {
            _currentDatabase = databaseName;
            App._mode = AppMode.UsingDatabase;
        }

        internal static void ListCachedDatabases()
        {
            Console.WriteLine($"Listing databases on server {_serverName}");
            foreach (var dbName in _databases)
            {
                Console.WriteLine($"- {dbName}");
            }
        }

        internal static void GetDatabases(string serverName)
        {
            var connString = $"Server={serverName};Database=master;Trusted_Connection = True;";
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand($"SELECT * FROM sys.databases", conn))
            {
                conn.Open();
                _databases.Clear();
                Console.WriteLine($"Connected to {serverName} - reading databases...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dbName = reader["name"].ToString();
                        _databases.Add(dbName);
                    }
                }
            }

            ListCachedDatabases();

            _mode = AppMode.ConnectedToServer;
        }
    }
}
