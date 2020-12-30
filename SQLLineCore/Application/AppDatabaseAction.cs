using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineCore
{
    internal static class AppDatabaseAction
    {
        #region Public Methods
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
        #endregion
    }
}
