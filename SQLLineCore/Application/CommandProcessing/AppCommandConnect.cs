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
            string userName = string.Empty;
            string password = string.Empty;
            
            if (!serverName.Contains(core.AppCommands.USER_NAME))
            {
                result = core.App.Connect(serverName);
            }
            else 
            {
                var items = SplitServerUserNamePassword(serverName);
                foreach(var i in items)
                {
                    if (i.StartsWith(AppCommands.SERVERNAME))
                    {
                        serverName = GetServerName(i);
                    }

                    if (i.StartsWith(AppCommands.USER_NAME))
                    {
                        userName = GetUserName(i);
                    }

                    if (i.StartsWith(AppCommands.PASSWORD))
                    {
                        password = GetPassword(i);
                    }
                }

                result = core.App.Connect(serverName, userName, password);
            }

            core.App.Mode = AppMode.ConnectedToServer;

            return result;
        }
        #endregion

        #region Private Methods
        private static string[] SplitServerUserNamePassword(string command)
        {
            var items = command.Split("-");
            return items;
        }

        private static string GetServerName(string command)
        {
            string result = string.Empty;

            if (command.StartsWith(AppCommands.SERVERNAME))
            {
                result = command.Replace(AppCommands.SERVERNAME, string.Empty).Trim();
            }

            return result;
        }

        private static string GetUserName(string command)
        {
            string result = string.Empty;

            if (command.StartsWith(AppCommands.USER_NAME))
            {
                result = command.Replace(AppCommands.USER_NAME, string.Empty).Trim();
            }

            return result;
        }

        private static string GetPassword(string command)
        {
            string result = string.Empty;

            if (command.StartsWith(AppCommands.PASSWORD))
            {
                result = command.Replace(AppCommands.PASSWORD, string.Empty).Trim();
            }

            return result;
        }
        #endregion

    }
}
