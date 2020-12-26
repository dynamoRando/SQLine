using System.Collections.Generic;

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
        internal static List<string> HandleQuestion(string command, AppMode mode)
        {
            var result = new List<string>();

            if (mode == AppMode.ConnectedToServer)
            {
                result = HandleConnectedToServer(command);
            }

            if (mode == AppMode.PendingConnection)
            {
                result = HandlePendingConnection(command);
            }

            if (mode == AppMode.UsingDatabase)
            {
                result = HandleUsingDatabase(command);
            }

            return result;
        }
        #endregion

        #region Private Methods
        internal static List<string> HandleUsingDatabase(string command)
        {
            var result = new List<string>();

            if (command == AppCommands.QUESTION)
            {
                result = HelpMenu.UsingDatabase();
            }

            if (command == AppCommands.QUESTION_DATABASES)
            {
                result = App.ListCachedDatabases();
            }

            if (command == AppCommands.QUESTION_DATABASES_UPDATE)
            {
                result = App.GetDatabases(AppCache.ServerName);
            }

            if (command == AppCommands.QUESTION_DATABASES_UPDATE)
            {
                result = App.GetDatabases(AppCache.ServerName);
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
                    result = App.ListTables(string.Empty, string.Empty);
                }

                if (command.StartsWith(AppCommands.QUESTION_TABLE_SCHEMA))
                {
                    string prefix = command.Replace(AppCommands.QUESTION_TABLE_SCHEMA, string.Empty).Trim();
                    App.GetTableSchema(prefix, string.Empty);
                    result = App.ShowTableSchema(prefix);
                }
                // show all tables
                else if (command == AppCommands.QUESTION_TABLE)
                {
                    result = App.ListTables(string.Empty, string.Empty);
                }
                else
                {
                    // show all tables with the specified prefix
                    string prefix = command.Replace(AppCommands.QUESTION_TABLE, string.Empty).Trim();
                    result = App.ListTables(prefix, string.Empty);
                }
            }

            return result;
        }

        internal static List<string> HandlePendingConnection(string command)
        {
            var result = new List<string>();

            if (command == AppCommands.QUESTION)
            {
                result = HelpMenu.PendingConnection();
            }

            return result;
        }

        internal static List<string> HandleConnectedToServer(string command)
        {
            var result = new List<string>();
            if (command == AppCommands.QUESTION)
            {
                result = HelpMenu.ConnectedToServer();
            }

            if (command == AppCommands.QUESTION_DATABASES)
            {
                result = App.ListCachedDatabases();
            }

            if (command == AppCommands.QUESTION_DATABASES_UPDATE)
            {
                result = App.GetDatabases(AppCache.ServerName);
            }

            return result;
        }
        #endregion

    }
}
