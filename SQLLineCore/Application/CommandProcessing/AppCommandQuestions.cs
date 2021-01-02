using System.Collections.Generic;
using System.Linq;

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
                result = HandleQuestionTable(command);
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

        #region Private Methods
        private static List<string> HandleQuestionTable(string command)
        {
            var result = new List<string>();

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
                //? t -schema foo
                //? t -schema foo prefix
                if (command.Contains(AppCommands.SCHEMA, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    string schemaName = string.Empty;
                    string prefix = string.Empty;

                    var array = command.Split(" ");
                    var items = array.ToList();
                    items.Remove(AppCommands.QUESTION);
                    items.Remove("t");
                    int index = items.IndexOf("-schema");
                    schemaName = items[index + 1];
                    if (items.Count() > index + 2)
                    {
                        prefix = items[index + 2];
                    }

                    result = App.ListTables(prefix, schemaName);
                }
                else
                {
                    //? t prefix
                    // show all tables with the specified prefix
                    string prefix = command.Replace(AppCommands.QUESTION_TABLE, string.Empty).Trim();
                    result = App.ListTables(prefix, string.Empty);
                }
            }

            return result;
        }
        #endregion

    }
}
