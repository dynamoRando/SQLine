using SQLineCore;
using System.Collections.Generic;

namespace SQLine
{
    static class UICommands
    {
        /// <summary>
        /// "show"
        /// </summary>
        public static string SHOW = "show";
        
        /// <summary>
        /// "hide"
        /// </summary>
        public static string HIDE = "hide";
        
        /// <summary>
        /// "size window"
        /// </summary>
        public static string SIZE_WINDOW = "size window";
        
        /// <summary>
        /// "editor"
        /// </summary>
        public static string EDITOR = "editor";
        
        /// <summary>
        /// "output"
        /// </summary>
        public static string OUTPUT = "output";

        internal static List<string> GetCommands()
        {
            var result = new List<string>();

            result.Add(SHOW);
            result.Add(HIDE);
            result.Add(SIZE_WINDOW);
            result.Add(EDITOR);
            result.Add(OUTPUT);

            return result;
        }

        internal static List<AppCommandDetail> GetUICommandDetails()
        {
            var result = new List<AppCommandDetail>();

            var command = new AppCommandDetail();
            command.CommandText = SHOW;
            command.CommandDescription = "Displays a window";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add($"{SHOW} {EDITOR}|{OUTPUT}");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = HIDE;
            command.CommandDescription = "Hides a window";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add($"{HIDE} {EDITOR}|{OUTPUT}");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = SIZE_WINDOW;
            command.CommandDescription = "Resizes a window by width or height (or both)";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add($"{SIZE_WINDOW} {EDITOR}|{OUTPUT} -w <number> -h <number>");

            result.Add(command);

            return result;
        }
    }
}
