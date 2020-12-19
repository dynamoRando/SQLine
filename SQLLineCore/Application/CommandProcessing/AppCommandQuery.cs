using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SQLineCore.Application.CommandProcessing
{
    public static class AppCommandQuery
    {

        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static List<string> HandleQuery(string command, AppMode mode)
        {
            var result = new List<string>();

            if (mode == AppMode.UsingDatabase)
            {
                string input = command.Substring(2);

                var connString = AppConnectionString.SQLServer.GetTrustedCurrentConnectionString();

                var adapter = new SqlDataAdapter(input, connString);
                var table = new DataTable();
                adapter.Fill(table);

                string columnsHeader = string.Empty;
                foreach (DataColumn column in table.Columns)
                {
                    columnsHeader += column.ColumnName;
                }

                result.Add(columnsHeader);

                string value = string.Empty;

                foreach(DataRow row in table.Rows)
                {
                    value = string.Empty;
                    foreach (DataColumn col in table.Columns)
                    {
                        value += row[col].ToString();
                    }
                    result.Add(value);
                }
            }

            return result;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
