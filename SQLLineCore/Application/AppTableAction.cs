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
        #endregion
    }
}
