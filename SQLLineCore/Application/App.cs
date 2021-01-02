using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using SQLineCore;
using System.Resources;
using System.IO;

/*
    * May you do good and not evil.
    * May you find forgiveness for yourself and forgive others.
    * May you share freely, never taking more than you give.
    * 
    * Obediently yours.
*/

namespace SQLineCore
{
    public static class App
    {
        #region Private Fields
        #endregion

        #region Public Properties
        public static AppMode Mode { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Events
        public static event EventHandler ConnectingToServer;
        public static event EventHandler ConnectedToServer;
        public static event EventHandler GettingDatabases;
        public static event EventHandler GotDatabases;
        #endregion

        #region Public Methods
        public static List<string> Connect(string serverName)
        {
            var result = new List<string>();
            result.Add($"Connecting to {serverName}");
            AppCache.ServerName = serverName;
            var items = GetDatabases(serverName);
            result.AddRange(items);

            return result;
        }

        public static List<string> Connect(string serverName, string userName, string password)
        {
            var result = new List<string>();

            result.Add($"Connecting to {serverName}");
            AppCache.ServerName = serverName;
            AppCache.UserName = userName;
            AppCache.Password = password;
            result.AddRange(GetDatabases(serverName, userName, password));

            return result;
        }

        /// <summary>
        /// Executes a SQL query and returns the results
        /// </summary>
        /// <param name="command">The SQL statement to Execute</param>
        /// <returns>The results of the SQL query</returns>
        public static DataTable ParseQuery(string command)
        {
            return AppDatabaseAction.ParseQuery(command);
        }

        /// <summary>
        /// Handles a command from the UI
        /// </summary>
        /// <param name="command">The command to parse and action on</param>
        /// <returns>Results from the command, if any</returns>
        public static List<string> ParseCommand(string command)
        {
            var result = new List<string>();

            if (command.StartsWith(AppCommands.QUESTION, StringComparison.CurrentCultureIgnoreCase))
            {
                result = AppCommandQuestions.HandleQuestion(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.USE_KEYWORD + " ", StringComparison.CurrentCultureIgnoreCase))
            {
                AppCommandUse.HandleUsingCommand(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.CONNECT_KEYWORD + " ", StringComparison.CurrentCultureIgnoreCase))
            {
                result = AppCommandConnect.HandleConnect(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.QUERY_KEYWORD + " ", StringComparison.CurrentCultureIgnoreCase))
            {
                result = AppCommandQuery.HandleQuery(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.QUIT, StringComparison.CurrentCultureIgnoreCase) || command.StartsWith(AppCommands.EXIT, StringComparison.CurrentCultureIgnoreCase))
            {
                AppCommandQuitExit.HandleQuitOrExit();
            }

            return result;
        }

        public static void SetDatabase(string databaseName)
        {
            AppCache.CurrentDatabase = databaseName;
            App.Mode = AppMode.UsingDatabase;
        }

        /// <summary>
        /// Returns the list of databases on the current server
        /// </summary>
        /// <returns></returns>
        public static List<string> ListCachedDatabases()
        {
            return AppDatabaseAction.ListCachedDatabases();
        }

        /// <summary>
        /// List the tables in the database 
        /// </summary>
        /// <param name="prefix">Filter results by a prefix ("StartsWith and Contains"). Pass string.Empty for all values</param>
        /// <param name="schema">Filter results for tables in a specified schema. Pass string.Empty for all values</param>
        /// <returns>A list of tables in the database filtered by the parameters.</returns>
        public static List<string> ListTables(string prefix, string schema)
        {
            return AppTableAction.ListTables(prefix, schema);
        }

        public static List<string> GetDatabases(string serverName, string userName, string password)
        {
            List<string> result = new List<string>();
            var connString = AppConnectionString.SQLServer.GetUserNamePasswordConnection(serverName, userName, password);
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.SQLServerCommand.GetSystemTableInfo, conn))
            {
                OpenConnection(conn);

                AppCache.Databases.Clear();
                result.Add($"Connected to {serverName} - reading databases...");

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dbName = reader["name"].ToString();
                        AppCache.Databases.Add(dbName);
                    }
                }
            }

            result = ListCachedDatabases();

            Mode = AppMode.ConnectedToServer;

            return result;
        }

        public static List<string> GetDatabases(string serverName)
        {
            EventHandler getDatabases = null;
            EventHandler gotDatabases = null;

            List<string> result = new List<string>();
            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.SQLServerCommand.GetSystemTableInfo, conn))
            {
                OpenConnection(conn);

                AppCache.Databases.Clear();
                result.Add($"Connected to {serverName} - reading databases...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        getDatabases = GettingDatabases;
                        getDatabases?.Invoke(null, null);

                        string dbName = reader["name"].ToString();
                        AppCache.Databases.Add(dbName);
                    }
                }
            }

            result = ListCachedDatabases();

            gotDatabases = GotDatabases;
            gotDatabases?.Invoke(null, null);

            Mode = AppMode.ConnectedToServer;

            return result;
        }

        public static List<string> ShowTableSchema(string prefix)
        {
            return AppTableAction.ShowTableSchema(prefix);
        }

        public static void LoadAppSettings(string filePath)
        {
            AppCache.Settings = GetAppSettings(filePath);
        }

        public static void SaveAppSettings(string filePath)
        {
            var lines = JsonSerializer.Serialize(AppCache.Settings);
            File.WriteAllText(filePath, lines);
        }

        public static AppSettings GetAppSettings(string filePath)
        {
            var settings = new AppSettings();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllText(filePath);
                settings = JsonSerializer.Deserialize<AppSettings>(lines);
            }

            return settings;
        }

        public static void GetTableSchema(string tableName, string schema)
        {
            AppTableAction.GetTableSchema(tableName, schema);
        }

        public static List<string> GetTables()
        {
            return AppTableAction.GetTables();
        }

        internal static void OpenConnection(SqlConnection connection)
        {
            var connecting = ConnectingToServer;
            connecting?.Invoke(null, null);
            connection.Open();
            var connected = ConnectedToServer;
            connected?.Invoke(null, null);
        }

        #endregion

        #region Private Methods

        #endregion

    }
}
