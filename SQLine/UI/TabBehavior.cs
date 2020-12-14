using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLine.UI
{
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
        /// <param name="input">The characters entered before the user pressed the Tab key</param>
        internal static void HandleTab(string input)
        {
            if (App.Mode == AppMode.ConnectedToServer || App.Mode == AppMode.UsingDatabase)
            {
                if (input.StartsWith("use"))
                {
                    HandleTabUseDatabase(input);
                }

                if (input.StartsWith("? t s"))
                {
                    HandleTabTableSchema(input);
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
        private static void HandleTabUseDatabase(string currentInput)
        {
            string outputItem = string.Empty;
            currentInput = currentInput.Replace("use", string.Empty).Trim();

            var matches = AppCache.Databases.Where(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));

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
            string line = "use " + outputItem;
            ConsoleInterface.Builder.Clear();
            ConsoleInterface.Builder.Append(line);
            Console.Write(ConsoleInterface.Builder.ToString());
        }

        private static void HandleTabTableSchema(string currentInput)
        {
            string outputItem = string.Empty;
            currentInput = currentInput.Replace("? t s", string.Empty).Trim();

            var matches = AppCache.Tables.Where(item => item.TableName != currentInput && item.TableName.
            StartsWith(currentInput, true, CultureInfo.InvariantCulture)).Select(t => t.TableName);

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
            string line = "? t s " + outputItem;
            ConsoleInterface.Builder.Clear();
            ConsoleInterface.Builder.Append(line);
            Console.Write(ConsoleInterface.Builder.ToString());
        }
        #endregion

    }
}
