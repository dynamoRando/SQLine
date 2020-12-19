using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public static class HelpMenu
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static List<string> ConnectedToServer()
        {
            var list = new List<string>();
            list.Add($"Press Esc at any time to exit program");
            list.Add($"Enter 'use' followed by a database name to switch your session to a specific database. Use Tab for autocomplete.");
            list.Add($"Type '? dbs' to list all databases on the server");
            list.Add($"Type '? dbs update' to update cache first and list all databases on the server");
            list.Add($"Type '?' at any time to show a list of available commands");

            return list;
        }

        internal static List<string> PendingConnection()
        {
            var list = new List<string>();
            list.Add($"Press Esc at any time to exit program");
            list.Add($"Enter a server name to connect to or 'localhost' to connect to an instance locally");
            list.Add($"Connection will use default to Trusted Connection");
            list.Add($"Type '?' at any time to show a list of available commands");

            return list;
        }

        internal static List<string> UsingDatabase()
        {
            var list = new List<string>();
            list.Add($"Press Esc at any time to exit program");
            list.Add($"Press 'cn <prefix> to connect to a different server. Use Tab for autocomplete (uses connection history/preferences). - not implemented");
            list.Add($"Enter 'use' followed by a database name to switch your session to a specific database.  Use Tab for autocomplete.");
            list.Add($"Type '? dbs' to list all databases on the server");
            list.Add($"Type '? dbs update' to update cache first and list all databases on the server");
            list.Add($"Type '? t/v/sp update' to to update cache of all tables/views/sprocs in the current database - not fully implemented");
            list.Add($"Type '? t/v/sp <prefix>' to list all tables/views/sprocs in the current database, or those with specified prefix - not fully implemented");
            list.Add($"Type '? t/v s <prefix>' to show schema details of the table/view - not fully implemented");
            list.Add($"Type 'q <query text>' to execute a query against the current database - not implemented");
            list.Add($"Type 'o table/csv to change the preferred output from table format to CSV format - not implemented");
            list.Add($"Press Ctrl+Q to enter query mode - not implemented");
            list.Add($"Press Ctrl+E to execute the currently cached query against the current database - not implemented");
            list.Add($"Type '?' at any time to show a list of available commands");

            return list;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
