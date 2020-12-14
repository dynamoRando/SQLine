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
        #endregion

        #region Public Properties
        /// <summary>
        /// The number of times the user has pressed the tab key
        /// </summary>
        internal static int TabCount { get; set; } = 0;

        /// <summary>
        /// Represents the characters entered by the user before hitting tab
        /// </summary>
        internal static string TabPrefix { get; set; } = string.Empty;
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
            if (TabCount == 0)
            {
                TabPrefix = ConsoleInterface.Builder.ToString();
            }

            if (App.Mode == AppMode.ConnectedToServer || App.Mode == AppMode.UsingDatabase)
            {
                if (TabPrefix.StartsWith(AppCommands.USE_KEYWORD))
                {
                    HandleTabUseDatabase(TabPrefix);
                }

                if (TabPrefix.StartsWith(AppCommands.QUESTION_TABLE_SCHEMA))
                {
                    HandleTabTableSchema(TabPrefix);
                }
            }
        }

        internal static void ResetTabValues()
        {
            TabCount = 0;
            TabPrefix = string.Empty;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Attempts to auto complete the database name based on the prefix provided
        /// </summary>
        /// <param name="currentInput">The first few letters of the database name</param>
        private static void HandleTabUseDatabase(string currentInput)
        {
            string outputItem = string.Empty;
            currentInput = currentInput.Replace(AppCommands.USE_KEYWORD, string.Empty).Trim();
            HandleTabAutoComplete(currentInput, AppCommands.USE_KEYWORD, AppCache.Databases);
        }

        private static void HandleTabAutoComplete(string currentInput, string commandPrefix, List<string> potentialValues)
        {
            string outputItem = string.Empty;
            var matches = potentialValues.Where(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));

            if (matches.Count() == 0)
            {
                return;
            }

            TabCount++;

            if (TabCount <= matches.Count())
            {
                outputItem = matches.ToList()[TabCount - 1];
            }
            else if (TabCount > matches.Count())
            {
                TabCount -= matches.Count();
                outputItem = matches.ToList()[TabCount - 1];
            }

            ConsoleInterface.ClearCurrentLine();
            ConsoleInterface.ShowPrefix();
            string line = commandPrefix + " " + outputItem;
            ConsoleInterface.Builder.Clear();
            ConsoleInterface.Builder.Append(line);
            Console.Write(ConsoleInterface.Builder.ToString());
        }

        /// <summary>
        /// Attempts to auto complete the table name for displaying schema information based on the prefix provided
        /// </summary>
        /// <param name="currentInput">The first few letters of the table name</param>
        private static void HandleTabTableSchema(string currentInput)
        {
            string outputItem = string.Empty;
            currentInput = currentInput.Replace(AppCommands.QUESTION_TABLE_SCHEMA, string.Empty).Trim();
            HandleTabAutoComplete(currentInput, AppCommands.QUESTION_TABLE_SCHEMA, AppCache.Tables.Select(t => t.TableName).ToList());
        }
        #endregion

    }
}
