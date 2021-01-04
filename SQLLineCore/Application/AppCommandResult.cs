using System;
using System.Linq;

namespace SQLineCore
{
    /// <summary>
    /// Used to determine if the command will return a result that is text based or a data table
    /// </summary>
    public static class AppCommandResult
    {
        /// <summary>
        /// Returns true if the app command supplied should return a text based result
        /// </summary>
        /// <param name="command">The app command</param>
        /// <returns>True if the result should be text based, otherwise false</returns>
        public static bool IsText(string command)
        {
            bool result = false;
            
            var commands = AppCommands.GetCommands();
            commands.Remove(AppCommands.QUERY_KEYWORD);

            if (commands.Any(c => command.StartsWith(c, StringComparison.CurrentCultureIgnoreCase)))
            {
                result = true;
            }

            return result;
        }


        /// <summary>
        /// Returns true if the app command supplied should return a data table result, otherwise false
        /// </summary>
        /// <param name="command">The app commmand</param>
        /// <returns>True if the result should be data table based, otherwise false</returns>
        public static bool IsData(string command)
        {
            bool result = false;

            if (command.StartsWith(AppCommands.QUERY_KEYWORD))
            {
                result = true;
            }

            return result;
        }
    }
}