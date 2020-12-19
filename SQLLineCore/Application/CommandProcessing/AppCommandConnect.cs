using core = SQLineCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineCore
{
    public static class AppCommandConnect
    { 
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static List<string> HandleConnect(string command, AppMode mode)
        {
            var result = new List<string>();
            string serverName = command.Replace(core.AppCommands.CONNECT_KEYWORD, string.Empty).Trim();
            result = core.App.Connect(serverName);
            core.App.Mode = AppMode.ConnectedToServer;

            return result;
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
