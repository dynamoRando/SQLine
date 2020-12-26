using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using SQLineCore;
using SQLineCore.Application.CommandProcessing;
using System.Resources;

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
        public static event EventHandler GettingTableSchema;
        public static event EventHandler GotTableSchema;
        public static event EventHandler GettingTables;
        public static event EventHandler GotTables;
        public static event EventHandler ExecutingQuery;
        public static event EventHandler ExecutedQuery;
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

        public static void GetTableSchema(string tableName, string schema)
        {
            if (AppCache.Tables.Count == 0)
            {
                GetTables();
            }

            var table = AppCache.Tables.FirstOrDefault(t => string.Equals(t.TableName, tableName, StringComparison.CurrentCultureIgnoreCase));

            if (table != null)
            {
                var gettingSchema = GettingTableSchema;
                gettingSchema?.Invoke(null, null);

                var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
                using (var conn = new SqlConnection(connString))
                using (var comm = new SqlCommand(AppSQLCommand.GetTablesSchema.Replace("<objectId>", table.ObjectId.ToString()), conn))
                {
                    OpenConnection(conn);
                    table.Columns.Clear();
                    using (SqlDataReader reader = comm.ExecuteReader())
                    {
                        var schemaTable = reader.GetSchemaTable();
                        while (reader.Read())
                        {
                            var column = new ColumnInfo();
                            column.ColumnName = reader["ColumnName"].ToString();
                            column.MaxLength = Convert.ToInt32(reader["ColumnMaxLength"]);
                            column.DataType = reader["ColumnDataType"].ToString();
                            column.IsNullable = Convert.ToBoolean(reader["IsNullable"].ToString());
                            table.Columns.Add(column);
                        }
                    }
                }

                var gotSchema = GotTableSchema;
                gotSchema?.Invoke(null, null);
            }
        }

        public static DataTable ParseQuery(string command)
        {
            DataTable result = new DataTable();

            if (command.StartsWith(AppCommands.QUERY_KEYWORD + " "))
            {
                var executingQuery = ExecutingQuery;
                executingQuery?.Invoke(null, null);
                result = AppCommandQuery.GetQueryResult(command, App.Mode);
                var executedQuery = ExecutedQuery;
                executedQuery?.Invoke(null, null);
            }

            return result;
        }

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

        public static List<string> ListCachedDatabases()
        {
            var result = new List<string>();
            result.Add($"Listing databases on server {AppCache.ServerName}");
            foreach (var dbName in AppCache.Databases)
            {
                result.Add($"- {dbName}");
            }

            return result;
        }

        /// <summary>
        /// List the tables in the database 
        /// </summary>
        /// <param name="prefix">Filter results by a prefix ("StartsWith and Contains"). Pass string.Empty for all values</param>
        /// <param name="schema">Filter results for tables in a specified schema. Pass string.Empty for all values</param>
        /// <returns>A list of tables in the database filtered by the parameters.</returns>
        public static List<string> ListTables(string prefix, string schema)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(schema) && string.IsNullOrEmpty(prefix))
            {
                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} for schema {schema}");
                var tables = AppCache.Tables.Where(t => t.SchemaName.Equals(schema, StringComparison.CurrentCultureIgnoreCase)).ToList();
                foreach (var table in tables)
                {
                    result.Add($"- {table.FullName}");
                }
            }
            else if (!string.IsNullOrEmpty(schema) && !string.IsNullOrEmpty(prefix))
            {
                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} for schema {schema} with prefix '{prefix}'");
                var tables = AppCache.Tables.
                    Where(t => t.SchemaName.Equals(schema, StringComparison.CurrentCultureIgnoreCase))
                    .Where(x => x.TableName.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)).ToList();
                foreach (var table in tables)
                {
                    result.Add($"- {table.FullName}");
                }

                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} for schema {schema} that contain '{prefix}'");
                var tables2 = AppCache.Tables.
                    Where(t => t.SchemaName.Equals(schema, StringComparison.CurrentCultureIgnoreCase))
                    .Where(x => x.TableName.Contains(prefix, StringComparison.CurrentCultureIgnoreCase)).ToList();
                foreach (var table in tables2)
                {
                    result.Add($"- {table.FullName}");
                }
            }
            // show every table
            else if (string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(schema))
            {
                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName}");
                foreach (var table in AppCache.Tables)
                {
                    result.Add($"- {table.FullName}");
                }
            }
            // show filtered tables in every schema
            else if (!string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(schema))
            {
                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} with prefix '{prefix}'");
                var tables = AppCache.Tables.Where(t => t.TableName.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)).ToList();
                foreach (var table in tables)
                {
                    result.Add($"- {table.FullName}");
                }

                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} that contain '{prefix}'");
                var tables2 = AppCache.Tables.Where(t => t.TableName.Contains(prefix, StringComparison.CurrentCultureIgnoreCase)).ToList();
                foreach (var table in tables2)
                {
                    result.Add($"- {table.FullName}");
                }
            }

            return result;
        }

        public static List<string> GetTables()
        {
            var gettingTables = GettingTables;
            gettingTables?.Invoke(null, null);

            var result = new List<string>();

            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetTables, conn))
            {
                OpenConnection(conn);
                AppCache.Tables.Clear();
                result.Add($"Connected to {AppCache.ServerName} - {AppCache.CurrentDatabase}, getting tables...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var table = new TableInfo();
                        table.TableName = reader["TableName"].ToString();
                        table.SchemaName = reader["SchemaName"].ToString();
                        table.ObjectId = Convert.ToInt32(reader["ObjectId"]);
                        AppCache.Tables.Add(table);
                    }
                }
            }

            var gotTables = GotTables;
            gotTables?.Invoke(null, null);

            return result;
        }

        public static List<string> GetDatabases(string serverName, string userName, string password)
        {
            EventHandler connecting = null;
            EventHandler connected = null;
            EventHandler gettingDatabases = null;
            EventHandler gotDatabases = null;

            List<string> result = new List<string>();
            var connString = AppConnectionString.SQLServer.GetUserNamePasswordConnection(serverName, userName, password);
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetSystemTableInfo, conn))
            {
                OpenConnection(conn);

                AppCache.Databases.Clear();
                result.Add($"Connected to {serverName} - reading databases...");

                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    gettingDatabases = GettingDatabases;
                    gettingDatabases.Invoke(null, null);

                    while (reader.Read())
                    {
                        string dbName = reader["name"].ToString();
                        AppCache.Databases.Add(dbName);
                    }
                }
            }

            gotDatabases = GotDatabases;
            gotDatabases?.Invoke(null, null);

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
            using (var comm = new SqlCommand(AppSQLCommand.GetSystemTableInfo, conn))
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
            var result = new List<string>();

            var table = AppCache.Tables.FirstOrDefault(t => string.Equals(t.TableName, prefix, StringComparison.CurrentCultureIgnoreCase));
            if (table != null)
            {
                int maxColLength = table.Columns.Select(c => c.ColumnName.Length).ToList().Max();
                result.Add($"Showing schema for table {table.SchemaName}.{table.TableName} in database {AppCache.CurrentDatabase} on server {AppCache.ServerName}");
                string formatter = "{0,-" + maxColLength.ToString() + "} {1,-10} {2,10} {3,-5}";
                string[] headers = { "COLUMNNAME", "DATATYPE", "MAXLENGTH", "ISNULLABLE" };
                result.Add(string.Format(formatter, headers));

                foreach (var column in table.Columns)
                {
                    string[] values = { column.ColumnName, column.DataType, column.MaxLength.ToString(), column.IsNullable.ToString() };
                    result.Add(string.Format(formatter, values));
                }
            }

            return result;
        }

        #endregion

        #region Private Methods
        private static void OpenConnection(SqlConnection connection)
        {
            var connecting = ConnectingToServer;
            connecting?.Invoke(null, null);
            connection.Open();
            var connected = ConnectedToServer;
            connected?.Invoke(null, null);
        }
        #endregion

    }
}
