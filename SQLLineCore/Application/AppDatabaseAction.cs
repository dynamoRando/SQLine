using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace SQLineCore
{
    /// <summary>
    /// Handles any action from the application that pertains to a database
    /// </summary>
    internal static class AppDatabaseAction
    {
        #region Public Methods
        /// <summary>
        /// Returns a list of the databases on the server that is cached in memory. Use 'update' to force a refresh
        /// </summary>
        /// <returns>A list of databases from the current connection</returns>
        internal static List<string> ListCachedDatabases()
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
        /// Executes a query against the database and returns a data table representing the results
        /// </summary>
        /// <param name="command">The command to execute</param>
        /// <returns>A data table of the results from the query</returns>
        internal static DataTable ParseQuery(string command)
        {
            DataTable result = new DataTable();

            if (command.StartsWith(AppCommands.QUERY_KEYWORD + " "))
            {
                result = AppCommandQuery.GetQueryResult(command, App.Mode);
            }

            return result;
        }
        
        #endregion
    }
}
