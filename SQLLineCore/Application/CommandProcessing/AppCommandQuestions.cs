using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public static class AppCommandQuestions
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static void HandleQuestion(string command, AppMode mode)
        {
            if (mode == AppMode.ConnectedToServer)
            {
                HandleConnectedToServer(command);
            }

            if (mode == AppMode.PendingConnection)
            {
                HandlePendingConnection(command);
            }

            if (mode == AppMode.UsingDatabase)
            {
                HandleUsingDatabase(command);
            }
        }
        #endregion

        #region Private Methods
        internal static void HandleUsingDatabase(string command)
        {
            if (command == AppCommands.QUESTION)
            {
                HelpMenu.UsingDatabase();
            }

            if (command == AppCommands.QUESTION_DATABASES)
            {
                App.ListCachedDatabases();
            }

            if (command == AppCommands.QUESTION_DATABASES_UPDATE)
            {
                App.GetDatabases(AppCache.ServerName);
            }

            if (command == AppCommands.QUESTION_DATABASES_UPDATE)
            {
                App.GetDatabases(AppCache.ServerName);
            }

            if (command.StartsWith(AppCommands.QUESTION_TABLE))
            {
                if (AppCache.Tables.Count == 0)
                {
                    App.GetTables();
                }

                if (command == AppCommands.QUESTION_TABLES_UPDATE)
                {
                    App.GetTables();
                    App.ListTables(string.Empty);
                }

                if (command.StartsWith(AppCommands.QUESTION_TABLE_SCHEMA))
                {
                    string prefix = command.Replace(AppCommands.QUESTION_TABLE_SCHEMA, string.Empty).Trim();
                    App.GetTableSchema(prefix);
                    App.ShowTableSchema(prefix);
                }
                // show all tables
                else if (command == AppCommands.QUESTION_TABLE)
                {
                    App.ListTables(string.Empty);
                }
                else
                {
                    // show all tables with the specified prefix
                    string prefix = command.Replace(AppCommands.QUESTION_TABLE, string.Empty).Trim();
                    App.ListTables(prefix);
                }
            }
        }

        internal static void HandlePendingConnection(string command)
        {
            if (command == AppCommands.QUESTION)
            {
                HelpMenu.PendingConnection();
            }
        }

        internal static void HandleConnectedToServer(string command)
        {
            if (command == AppCommands.QUESTION)
            {
                HelpMenu.ConnectedToServer();
            }

            if (command == AppCommands.QUESTION_DATABASES)
            {
                App.ListCachedDatabases();
            }

            if (command == AppCommands.QUESTION_DATABASES_UPDATE)
            {
                App.GetDatabases(AppCache.ServerName);
            }
        }
        #endregion

    }
}
