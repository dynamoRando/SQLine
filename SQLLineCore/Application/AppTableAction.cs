using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Linq;

namespace SQLineCore
{
    internal static class AppTableAction
    {
        #region Public Methods
        internal static List<string> GetTables()
        {
            //var gettingTables = App.GettingTables;
            //gettingTables?.Invoke(null, null);

            var result = new List<string>();

            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.SQLServerCommand.GetTables, conn))
            {
                App.OpenConnection(conn);
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

            //var gotTables = App.GotTables;
            //gotTables?.Invoke(null, null);

            return result;
        }

        internal static void GetTableSchema(string tableName, string schema)
        {
            if (AppCache.Tables.Count == 0)
            {
                GetTables();
            }

            var table = AppCache.Tables.FirstOrDefault(t => string.Equals(t.TableName, tableName, StringComparison.CurrentCultureIgnoreCase));

            if (table != null)
            {
                //var gettingSchema = GettingTableSchema;
                //gettingSchema?.Invoke(null, null);

                var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
                using (var conn = new SqlConnection(connString))
                using (var comm = new SqlCommand(AppSQLCommand.SQLServerCommand.GetTablesSchema.Replace("<objectId>", table.ObjectId.ToString()), conn))
                {
                    App.OpenConnection(conn);
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

                //var gotSchema = GotTableSchema;
                //gotSchema?.Invoke(null, null);
            }
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


        /// <summary>
        /// List the tables in the database 
        /// </summary>
        /// <param name="prefix">Filter results by a prefix ("StartsWith and Contains"). Pass string.Empty for all values</param>
        /// <param name="schema">Filter results for tables in a specified schema. Pass string.Empty for all values</param>
        /// <returns>A list of tables in the database filtered by the parameters.</returns>
        internal static List<string> ListTables(string prefix, string schema)
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

        #endregion
    }
}
