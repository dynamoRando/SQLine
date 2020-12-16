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
        internal static void ConnectedToServer()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter 'use' followed by a database name to switch your session to a specific database. Use Tab for autocomplete.");
            Console.WriteLine($"Type '? dbs' to list all databases on the server");
            Console.WriteLine($"Type '? dbs update' to update cache first and list all databases on the server");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void PendingConnection()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Enter a server name to connect to or 'localhost' to connect to an instance locally");
            Console.WriteLine($"Connection will use default to Trusted Connection");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }

        internal static void UsingDatabase()
        {
            Console.WriteLine($"Press Esc at any time to exit program");
            Console.WriteLine($"Press 'cn <prefix> to connect to a different server. Use Tab for autocomplete (uses connection history/preferences). - not implemented");
            Console.WriteLine($"Enter 'use' followed by a database name to switch your session to a specific database.  Use Tab for autocomplete.");
            Console.WriteLine($"Type '? dbs' to list all databases on the server");
            Console.WriteLine($"Type '? dbs update' to update cache first and list all databases on the server");
            Console.WriteLine($"Type '? t/v/sp update' to to update cache of all tables/views/sprocs in the current database - not fully implemented");
            Console.WriteLine($"Type '? t/v/sp <prefix>' to list all tables/views/sprocs in the current database, or those with specified prefix - not fully implemented");
            Console.WriteLine($"Type '? t/v s <prefix>' to show schema details of the table/view - not fully implemented");
            Console.WriteLine($"Type 'q <query text>' to execute a query against the current database - not implemented");
            Console.WriteLine($"Type 'o table/csv to change the preferred output from table format to CSV format - not implemented");
            Console.WriteLine($"Press Ctrl+Q to enter query mode - not implemented");
            Console.WriteLine($"Press Ctrl+E to execute the currently cached query against the current database - not implemented");
            Console.WriteLine($"Type '?' at any time to show a list of available commands");
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
