using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLineCore;

namespace SQLine
{
    /// <summary>
    /// Responsible for drawing various items on the console screen
    /// </summary>
    static class ConsoleInterface
    {
        #region Private Fields
        static string _prefix = "-> ";
        #endregion

        #region Public Properties
        /// <summary>
        /// Represents the current string being entered by the user
        /// </summary>
        public static StringBuilder Builder { get; set; } = new StringBuilder();
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public static void BuildCurrentConsoleLine()
        {
            ConsoleInterface.ShowPrefix();
            Console.Write(ConsoleInterface.Builder.ToString());
        }

        /// <remarks>
        /// https://stackoverflow.com/a/8946847/1188513
        /// </remarks>>
        internal static void ClearCurrentLine()
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }

        internal static void ShowPrefix()
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            ConsoleColor currentBackground = Console.BackgroundColor;

            switch (App.Mode)
            {
                case AppMode.ConnectedToServer:
                    WriteServerPrefix();
                    WriteEndingPrefix(currentForeground, currentBackground);
                    break;
                case AppMode.UsingDatabase:
                    WriteServerPrefix();
                    WriteSpacer(currentForeground, currentBackground);
                    WriteDatabasePrefix();
                    WriteEndingPrefix(currentForeground, currentBackground);
                    break;
                default:
                    Console.Write(_prefix);
                    break;
            }
        }
        #endregion

        #region Private Methods
        private static void WriteDatabasePrefix()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(AppCache.CurrentDatabase);
        }

        private static void WriteEndingPrefix(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(" " + _prefix);
        }

        private static void WriteServerPrefix()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(AppCache.ServerName);
        }

        private static void WriteSpacer(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(" > ");
        }
        #endregion


    }
}
