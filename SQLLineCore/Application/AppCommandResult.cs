using System;
using System.Linq;

namespace SQLineCore
{
    public static class AppCommandResult
    {
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