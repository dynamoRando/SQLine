using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineGUI.UI.UIChanges
{
    static class UICommands
    {
        public static string SHOW = "show";
        public static string HIDE = "hide";

        internal static List<string> GetCommands()
        {
            var result = new List<string>();

            result.Add(SHOW);
            result.Add(HIDE);

            return result;
        }
    }
}
