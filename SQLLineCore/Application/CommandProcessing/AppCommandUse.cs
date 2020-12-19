using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public static class AppCommandUse
    { 
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static void HandleUsingCommand(string command, AppMode mode)
        {
            if (mode == AppMode.PendingConnection)
            {
                string dbName = command.Replace(AppCommands.USE_KEYWORD, string.Empty).Trim();
                App.SetDatabase(dbName);
                App.GetTables();
            }

            if (mode == AppMode.UsingDatabase)
            {
                string dbName = command.Replace(AppCommands.USE_KEYWORD, string.Empty).Trim();
                App.SetDatabase(dbName);
                App.GetTables();
            }

            if (mode == AppMode.ConnectedToServer)
            {
                string dbName = command.Replace(AppCommands.USE_KEYWORD, string.Empty).Trim();
                App.SetDatabase(dbName);
                App.GetTables();
            }
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
