using SQLineCore.DatabaseItem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQLineCore
{
    internal static class AppStoredProcedureAction
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static List<string> GetProcedures()
        {
            var result = new List<string>();

            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.SQLServerCommand.GetProcedures, conn))
            {
                App.OpenConnection(conn);
                AppCache.Tables.Clear();
                result.Add($"Connected to {AppCache.ServerName} - {AppCache.CurrentDatabase}, getting tables...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var proc = new ProcedureInfo();
                        proc.ProcedureName = reader["ProcedureName"].ToString();
                        proc.SchemaName = reader["SchemaName"].ToString();
                        proc.ObjectId = Convert.ToInt32(reader["ObjectId"]);
                        AppCache.Procedures.Add(proc);
                    }
                }
            }

            return result;
        }

        internal static List<string> ListProcedures()
        {
            var result = new List<string>();
            result.Add($"Listing stored procedures from database {AppCache.CurrentDatabase} on server {AppCache.ServerName}");
            AppCache.Procedures.ForEach(v => result.Add($"- {v.FullName}"));
            return result;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
