using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using SQLineCore;
using SQLineCore.Application.CommandProcessing;

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

        #region Public Methods
        public static List<string> Connect(string serverName)
        {
            var result = new List<string>();
            result.Add($"Connecting to {serverName}");
            AppCache.ServerName = serverName;
            result.AddRange(GetDatabases(serverName));

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

        public static void GetTableSchema(string tableName)
        {
            if (AppCache.Tables.Count == 0)
            {
                GetTables();
            }

            var table = AppCache.Tables.FirstOrDefault(t => string.Equals(t.TableName,tableName, StringComparison.CurrentCultureIgnoreCase));

            if (table != null)
            {
                var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
                using (var conn = new SqlConnection(connString))
                using (var comm = new SqlCommand(AppSQLCommand.GetTablesSchema.Replace("<objectId>", table.ObjectId.ToString()), conn))
                {
                    conn.Open();
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
            }
        }

        public static DataTable ParseQuery(string command)
        {
            DataTable result = new DataTable();

            if (command.StartsWith(AppCommands.QUERY_KEYWORD + " "))
            {
                result = AppCommandQuery.GetQueryResult(command, App.Mode);
            }

            return result;
        }

        public static List<string> ParseCommand(string command)
        {
            var result = new List<string>();

            if (command.StartsWith(AppCommands.QUESTION))
            {
                result = AppCommandQuestions.HandleQuestion(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.USE_KEYWORD + " "))
            {
                AppCommandUse.HandleUsingCommand(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.CONNECT_KEYWORD + " "))
            {
                result = AppCommandConnect.HandleConnect(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.QUERY_KEYWORD + " "))
            {
                result = AppCommandQuery.HandleQuery(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.QUIT, true, CultureInfo.CurrentCulture) || command.StartsWith(AppCommands.EXIT, true, CultureInfo.CurrentCulture))
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

        public static List<string> ListTables(string prefix)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(prefix))
            {
                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName}");
                foreach (var table in AppCache.Tables)
                {
                    result.Add($"- {table.SchemaName}.{table.TableName}");
                }
            }
            else
            {
                result.Add($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} with prefix '{prefix}'");
                var tables = AppCache.Tables.Where(t => t.TableName.StartsWith(prefix, true, CultureInfo.InvariantCulture)).ToList();
                foreach (var table in tables)
                {
                    result.Add($"- {table.SchemaName}.{table.TableName}");
                }
            }

            return result;
        }

        public static List<string> GetTables()
        {
            var result = new List<string>();

            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetTables, conn))
            {
                conn.Open();
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

            return result;
        }

        public static List<string> GetDatabases(string serverName, string userName, string password)
        {
            List<string> result = new List<string>();
            var connString = AppConnectionString.SQLServer.GetUserNamePasswordConnection(serverName, userName, password);
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetSystemTableInfo, conn))
            {
                conn.Open();
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
            List<string> result = new List<string>();
            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetSystemTableInfo, conn))
            {
                conn.Open();
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

        public static List<string> ShowTableSchema(string prefix)
        {
            var result = new List<string>();

            var table = AppCache.Tables.FirstOrDefault(t => string.Equals(t.TableName,prefix, StringComparison.CurrentCultureIgnoreCase));
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
        #endregion

    }
}
