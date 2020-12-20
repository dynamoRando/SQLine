using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public static class AppConnectionString
    {
        internal static class SQLServer
        {
            public static string GetTrustedConnection(string serverName)
            {
                return $"Server={serverName};Database=master;Trusted_Connection = True;";
            }

            public static string GetTrustedConnection(string serverName, string databaseName)
            {
                return $"Server={serverName};Database={databaseName};Trusted_Connection = True;";
            }

            public static string GetTrustedCurrentConnectionString()
            {
                return $"Server={AppCache.ServerName};Database={AppCache.CurrentDatabase};Trusted_Connection = True;";
            }

            public static string GetUserNamePasswordConnection(string serverName, string userName, string password)
            {
                return $"Server={serverName};Database=master;User Id={userName};Password={password};";
            }

            public static string GetUsernamePasswordCurrentConnectionString()
            {
                return $"Server={AppCache.ServerName};Database={AppCache.CurrentDatabase};User Id={AppCache.UserName};Password={AppCache.Password};";
            }

            public static string GetCurrentConnectionString()
            {
                if (string.IsNullOrEmpty(AppCache.UserName))
                {
                    return GetTrustedCurrentConnectionString();
                }
                else
                {
                    return GetUsernamePasswordCurrentConnectionString();
                }
            }
        }
    }
}
