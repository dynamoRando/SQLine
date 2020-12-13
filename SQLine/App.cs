using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;

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
        public static List<TableInfo> _tables = new List<TableInfo>();

        private static string _GetTablesCmd = "SELECT s.name SchemaName, t.name TableName, object_id ObjectId FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id";
        private static string _GetTableSchemaCmd = "SELECT c.name ColumnName, c.max_length ColumnMaxLength, c.is_nullable IsNullable, t.name ColumnDataType FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id join sys.objects o on c.object_id = o.object_id WHERE o.object_id = <objectId>";

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
            Console.WriteLine($"Enter 'use' followed by a database name to switch your session to a specific database. Use Tab for autocomplete.");
            Console.WriteLine($"Type '? dbs' to list all databases on the server");
            Console.WriteLine($"Type '? dbs update' to update cache first and list all databases on the server");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void UsingDatabaseHelpMenu()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter 'use' followed by a database name to switch your session to a specific database.  Use Tab for autocomplete.");
            Console.WriteLine($"Type '? dbs' to list all databases on the server");
            Console.WriteLine($"Type '? dbs update' to update cache first and list all databases on the server");
            Console.WriteLine($"Type '? t/v/s update' to to update cache of all tables/views/sprocs in the current database - not fully implemented");
            Console.WriteLine($"Type '? t/v/s <prefix>' to list all tables/views/sprocs in the current database, or those with specified prefix - not fully implemented");
            Console.WriteLine($"Type '? t/v s' to show schema details of the table/view - not fully implemented");
            Console.WriteLine($"Type 'q '<query text>' to execute a query against the current database - not implemented");
            Console.WriteLine($"Type 'o table/csv to change the preferred output from table format to CSV format - not implemented");
            Console.WriteLine($"Press Ctrl+Q to enter query mode - not implemented");
            Console.WriteLine($"Press Ctrl+E to execute the currently cached query against the current database - not implemented");
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

                if (command.StartsWith("? t"))
                {
                    if (command == "? t")
                    {
                        if (_tables.Count == 0)
                        {
                            GetTables();
                        }

                        ListTables(string.Empty);
                    }
                    else
                    {
                        if (_tables.Count == 0)
                        {
                            GetTables();
                        }

                        string prefix = command.Replace("? t", string.Empty).Trim();
                        ListTables(prefix);
                    }
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

        internal static void ListTables(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                Console.WriteLine($"Listing tables from database {_currentDatabase} on server {_serverName}");
                foreach (var table in _tables)
                {
                    Console.WriteLine($"- {table.SchemaName}.{table.TableName}");
                }
            }
            else
            {
                Console.WriteLine($"Listing tables from database {_currentDatabase} on server {_serverName} with prefix '{prefix}'");
                var tables = _tables.Where(t => t.TableName.StartsWith(prefix, true, CultureInfo.InvariantCulture)).ToList();
                foreach (var table in tables)
                {
                    Console.WriteLine($"- {table.SchemaName}.{table.TableName}");
                }
            }
        }

        internal static void GetTables()
        {
            var connString = $"Server={_serverName};Database={_currentDatabase};Trusted_Connection = True;";
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(_GetTablesCmd, conn))
            {
                conn.Open();
                _tables.Clear();
                Console.WriteLine($"Connected to {_serverName} - {_currentDatabase}, getting tables...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var table = new TableInfo();
                        table.TableName = reader["TableName"].ToString();
                        table.SchemaName = reader["SchemaName"].ToString();
                        table.ObjectId = Convert.ToInt32(reader["ObjectId"]);
                        _tables.Add(table);
                    }
                }
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
