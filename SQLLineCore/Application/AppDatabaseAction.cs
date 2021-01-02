using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace SQLineCore
{
    internal static class AppDatabaseAction
    {
        #region Public Methods
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

        internal static DataTable ParseQuery(string command)
        {
            DataTable result = new DataTable();

            if (command.StartsWith(AppCommands.QUERY_KEYWORD + " "))
            {
                //var executingQuery = ExecutingQuery;
                //executingQuery?.Invoke(null, null);
                result = AppCommandQuery.GetQueryResult(command, App.Mode);
                //var executedQuery = ExecutedQuery;
                //executedQuery?.Invoke(null, null);
            }

            return result;
        }
        
        #endregion
    }
}
