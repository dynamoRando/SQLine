using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using core = SQLineCore;

namespace SQLine
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
        internal static void HandleTab(string input)
        {
            // if this is the first time the user has pressed the tab key, save the current entered line from the user
            // otherwise we want to iterate to the next value in the tab list depending on what the pending command is

            if (_tabCount == 0)
            {
                _tabPrefix = input;
            }

            if (core.App.Mode == core.AppMode.ConnectedToServer || core.App.Mode == core.AppMode.UsingDatabase)
            {
                if (_tabPrefix.StartsWith(core.AppCommands.USE_KEYWORD, StringComparison.CurrentCultureIgnoreCase))
                {
                    HandleTabUseDatabase(_tabPrefix);
                }

                if (_tabPrefix.StartsWith(core.AppCommands.QUESTION_TABLE_SCHEMA, StringComparison.CurrentCultureIgnoreCase))
                {
                    HandleTabTableSchema(_tabPrefix);
                }
            }

            if (_tabPrefix.StartsWith(core.AppCommands.CONNECT_KEYWORD, StringComparison.CurrentCultureIgnoreCase))
            {
                HandleConnectionTabComplete(_tabPrefix);
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
            currentInput = currentInput.Replace(core.AppCommands.USE_KEYWORD, string.Empty).Trim();
            HandleTabAutoComplete(currentInput, core.AppCommands.USE_KEYWORD, core.AppCache.Databases);
        }

        /// <summary>
        /// Attempts to auto complete the table name for displaying schema information based on the prefix provided
        /// </summary>
        /// <param name="currentInput">The first few letters of the table name</param>
        private static void HandleTabTableSchema(string currentInput)
        {
            currentInput = currentInput.Replace(core.AppCommands.QUESTION_TABLE_SCHEMA, string.Empty).Trim();
            HandleTabAutoComplete(currentInput, core.AppCommands.QUESTION_TABLE_SCHEMA, core.AppCache.Tables.Select(t => t.TableName).ToList());
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

            
            string line = commandPrefix + " " + outputItem;
            ConsoleInput.SetInput(line);
        }

        private static void HandleConnectionTabComplete(string currentInput)
        {
            currentInput = currentInput.Replace(core.AppCommands.CONNECT_KEYWORD, string.Empty).Trim();
            var list = core.AppCache.Settings.Connections.Select(con => con.Name).ToList();
            HandleTabAutoComplete(currentInput, core.AppCommands.CONNECT_KEYWORD, list);
        }


        #endregion

    }
}
