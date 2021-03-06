﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using SQLineCore;

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
        public static void Connect(string serverName)
        {
            Console.WriteLine($"Connecting to {serverName}");
            AppCache.ServerName = serverName;
            GetDatabases(serverName);
        }

        public static void GetTableSchema(string tableName)
        {
            if (AppCache.Tables.Count == 0)
            {
                GetTables();
            }

            var table = AppCache.Tables.FirstOrDefault(t => t.TableName == tableName);

            if (table != null)
            {
                var connString = AppConnectionString.SQLServer.GetTrustedConnection(AppCache.ServerName, AppCache.CurrentDatabase);
                using (var conn = new SqlConnection(connString))
                using (var comm = new SqlCommand(AppSQLCommand.GetTablesSchema.Replace("<objectId>", table.ObjectId.ToString()), conn))
                {
                    conn.Open();
                    table.Columns.Clear();
                    Console.WriteLine($"Connected to {AppCache.ServerName} - {AppCache.CurrentDatabase}, getting tables...");
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

        public static void ParseCommand(string command)
        {
            if (command.StartsWith(AppCommands.QUESTION))
            {
                AppCommandQuestions.HandleQuestion(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.USE_KEYWORD + " "))
            {
                AppCommandUse.HandleUsingCommand(command, App.Mode);
            }

            if (command.StartsWith(AppCommands.CONNECT_KEYWORD + " "))
            {
                AppCommandConnect.HandleConnect(command, App.Mode);
            }
        }

        public static void SetDatabase(string databaseName)
        {
            AppCache.CurrentDatabase = databaseName;
            App.Mode = AppMode.UsingDatabase;
        }

        public static void ListCachedDatabases()
        {
            Console.WriteLine($"Listing databases on server {AppCache.ServerName}");
            foreach (var dbName in AppCache.Databases)
            {
                Console.WriteLine($"- {dbName}");
            }
        }

        public static void ListTables(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                Console.WriteLine($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName}");
                foreach (var table in AppCache.Tables)
                {
                    Console.WriteLine($"- {table.SchemaName}.{table.TableName}");
                }
            }
            else
            {
                Console.WriteLine($"Listing tables from database {AppCache.CurrentDatabase} on server {AppCache.ServerName} with prefix '{prefix}'");
                var tables = AppCache.Tables.Where(t => t.TableName.StartsWith(prefix, true, CultureInfo.InvariantCulture)).ToList();
                foreach (var table in tables)
                {
                    Console.WriteLine($"- {table.SchemaName}.{table.TableName}");
                }
            }
        }

        public static void GetTables()
        {
            var connString = AppConnectionString.SQLServer.GetTrustedConnection(AppCache.ServerName, AppCache.CurrentDatabase);
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetTables, conn))
            {
                conn.Open();
                AppCache.Tables.Clear();
                Console.WriteLine($"Connected to {AppCache.ServerName} - {AppCache.CurrentDatabase}, getting tables...");
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
        }

        public static void GetDatabases(string serverName)
        {
            var connString = AppConnectionString.SQLServer.GetTrustedConnection(AppCache.ServerName);
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.GetSystemTableInfo, conn))
            {
                conn.Open();
                AppCache.Databases.Clear();
                Console.WriteLine($"Connected to {serverName} - reading databases...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string dbName = reader["name"].ToString();
                        AppCache.Databases.Add(dbName);
                    }
                }
            }

            ListCachedDatabases();

            Mode = AppMode.ConnectedToServer;
        }

        public static void ShowTableSchema(string prefix)
        {
            var table = AppCache.Tables.FirstOrDefault(t => t.TableName == prefix);
            int maxColLength = table.Columns.Select(c => c.ColumnName.Length).ToList().Max();
            Console.WriteLine($"Showing schema for table {table.SchemaName}.{table.TableName} in database {AppCache.CurrentDatabase} on server {AppCache.ServerName}");
            string formatter = "{0,-" + maxColLength.ToString() + "} {1,-10} {2,10} {3,-5}";
            string[] headers = { "COLUMNNAME", "DATATYPE", "MAXLENGTH", "ISNULLABLE" };
            Console.WriteLine(formatter, headers);
            foreach (var column in table.Columns)
            {
                string[] values = { column.ColumnName, column.DataType, column.MaxLength.ToString(), column.IsNullable.ToString() };
                Console.WriteLine(formatter, values);
            }
        }
        public static void MainMenu()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter a server name to connect to or 'localhost' to connect to an instance locally");
            Console.WriteLine($"Connection will use default to Trusted Connection");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
