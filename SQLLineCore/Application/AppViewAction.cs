using SQLineCore.DatabaseItem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace SQLineCore
{
    internal static class AppViewAction
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static List<string> GetViews()
        {
            var result = new List<string>();

            var connString = AppConnectionString.SQLServer.GetCurrentConnectionString();
            using (var conn = new SqlConnection(connString))
            using (var comm = new SqlCommand(AppSQLCommand.SQLServerCommand.GetViews, conn))
            {
                App.OpenConnection(conn);
                AppCache.Views.Clear();
                result.Add($"Connected to {AppCache.ServerName} - {AppCache.CurrentDatabase}, getting views...");
                using (SqlDataReader reader = comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var view = new ViewInfo();
                        view.ViewName = reader["ViewName"].ToString();
                        view.SchemaName = reader["SchemaName"].ToString();
                        view.ObjectId = Convert.ToInt32(reader["ObjectId"]);
                        AppCache.Views.Add(view);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
