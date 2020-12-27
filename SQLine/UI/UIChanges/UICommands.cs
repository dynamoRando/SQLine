using System.Collections.Generic;

namespace SQLine.UI.UIChanges
{
    static class UICommands
    {
        public static string SHOW = "show";
        public static string HIDE = "hide";
        public static string SIZE_WINDOW = "size window";
        public static string EDITOR = "editor";
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
    }
}
