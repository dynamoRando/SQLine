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
                result = AppCommandQuestionTable.HandleCommand(command);
            }

            return result;
        }

        #endregion

    }
}
