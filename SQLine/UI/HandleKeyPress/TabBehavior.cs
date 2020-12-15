using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLine.UI
{
    /// <summary>
    /// A class for handling the Tab keypress to auto complete various commands/values
    /// </summary>
    static class TabBehavior
    {
        #region Private Fields
        /// <summary>
        /// The number of times the user has pressed the tab key
        /// </summary>
        private static int _tabCount = 0;

        /// <summary>
        /// Represents the characters entered by the user before hitting tab
        /// </summary>
        internal static string _tabPrefix = string.Empty;
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        /// <summary>
        /// Handles the tab key press
        /// </summary>
        internal static void HandleTab()
        {
            // if this is the first time the user has pressed the tab key, save the current entered line from the user
            // otherwise we want to iterate to the next value in the tab list depending on what the pending command is
            if (_tabCount == 0)
            {
                _tabPrefix = ConsoleInterface.Builder.ToString();
            }

            if (App.Mode == AppMode.ConnectedToServer || App.Mode == AppMode.UsingDatabase)
            {
                if (_tabPrefix.StartsWith(AppCommands.USE_KEYWORD))
                {
                    HandleTabUseDatabase(_tabPrefix);
                }

                if (_tabPrefix.StartsWith(AppCommands.QUESTION_TABLE_SCHEMA))
                {
                    HandleTabTableSchema(_tabPrefix);
                }
            }
        }

        internal static void IncrementTabCount()
        {
            _tabCount++;
        }

        internal static void ResetTabValues()
        {
            _tabCount = 0;
            _tabPrefix = string.Empty;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Attempts to auto complete the database name based on the prefix provided
        /// </summary>
        /// <param name="currentInput">The first few letters of the database name</param>
        private static void HandleTabUseDatabase(string currentInput)
        {
            currentInput = currentInput.Replace(AppCommands.USE_KEYWORD, string.Empty).Trim();
            HandleTabAutoComplete(currentInput, AppCommands.USE_KEYWORD, AppCache.Databases);
        }

        /// <summary>
        /// Attempts to auto complete the table name for displaying schema information based on the prefix provided
        /// </summary>
        /// <param name="currentInput">The first few letters of the table name</param>
        private static void HandleTabTableSchema(string currentInput)
        {
            currentInput = currentInput.Replace(AppCommands.QUESTION_TABLE_SCHEMA, string.Empty).Trim();
            HandleTabAutoComplete(currentInput, AppCommands.QUESTION_TABLE_SCHEMA, AppCache.Tables.Select(t => t.TableName).ToList());
        }

        /// <summary>
        /// Renders on console screen the potential match on the line
        /// </summary>
        /// <param name="currentInput">The current prefix value - minus the actual command. This is the thing we'll be searching for.</param>
        /// <param name="commandPrefix">The type of command we are processing</param>
        /// <param name="potentialValues">A list of poential values we want to try to match to</param>
        private static void HandleTabAutoComplete(string currentInput, string commandPrefix, List<string> potentialValues)
        {
            string outputItem = string.Empty;
            var matches = potentialValues.Where(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));

            if (matches.Count() == 0)
            {
                return;
            }

            _tabCount++;

            if (_tabCount <= matches.Count())
            {
                outputItem = matches.ToList()[_tabCount - 1];
            }
            else if (_tabCount > matches.Count())
            {
                _tabCount -= matches.Count();
                outputItem = matches.ToList()[_tabCount - 1];
            }

            ConsoleInterface.ClearCurrentLine();
            ConsoleInterface.ShowPrefix();
            string line = commandPrefix + " " + outputItem;
            ConsoleInterface.Builder.Clear();
            ConsoleInterface.Builder.Append(line);
            Console.Write(ConsoleInterface.Builder.ToString());
        }


        #endregion

    }
}
