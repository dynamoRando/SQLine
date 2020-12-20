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

                var columns = new List<ColumnInfo>();
                var columnNames = new List<string>();

                int maxColumnNameLength = 0;

                foreach (DataColumn column in table.Columns)
                {
                    columnNames.Add(column.ColumnName);

                    if (column.ColumnName.Length > maxColumnNameLength)
                    {
                        maxColumnNameLength = column.ColumnName.Length;
                    }

                    //MaxLength = column.MaxLength == -1 ? 10 : column.MaxLength,
                    var col = new ColumnInfo()
                    {
                        ColumnName = column.ColumnName,
                        MaxLength = column.MaxLength,
                        DataType = column.DataType.ToString(),
                        Ordinal = column.Ordinal
                    };

                    if (maxColumnNameLength > col.MaxLength)
                    {
                        col.MaxLength = maxColumnNameLength;
                    }

                    columns.Add(col);
                }

                string formatter = BuildFormatter(columns);
                result.Add(string.Format(formatter, columnNames.ToArray()));

                var columnValues = new List<string>();

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        columnValues.Add(row[col].ToString());
                    }
                    result.Add(string.Format(formatter, columnValues.ToArray()));
                    columnValues.Clear();
                }
            }

            return result;
        }
        #endregion

        #region Private Methods
        private static string BuildFormatter(List<ColumnInfo> columns)
        {
            StringBuilder result = new StringBuilder();
            string prefix = string.Empty;

            //  string formatter = "{0,-" + maxColLength.ToString() + "} {1,-10} {2,10} {3,-5}";
            foreach (var column in columns)
            {
                prefix = string.Empty;

                if (IsLeftAligned(column))
                {
                    prefix = "-";
                }
                else
                {
                    prefix = string.Empty;
                }

                result.Append("{" + column.Ordinal + "," + prefix + column.MaxLength.ToString() + "} ");
            }

            return result.ToString().Trim();
        }

        /// <summary>
        /// Attempts to determine if the field is text. If yes, it will return true, otherwise false
        /// </summary>
        /// <param name="column">The column to align</param>
        /// <returns>True if the column is text, otherwise false</returns>
        private static bool IsLeftAligned(ColumnInfo column)
        {
            if (column.DataType.Contains("string", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
