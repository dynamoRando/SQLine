using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLineCore;
using SQLineGUI.UI;
using core = SQLineCore;

namespace SQLineGUI
{
    static class EnterBehavior
    {
        #region Private Fields

        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static void HandleEnter(string command)
        {
            var result = new List<string>();
            KeyUpBehavior.AddCommandToHistory(command);
            KeyUpBehavior.ResetKeyUpCount();
            result = core.App.ParseCommand(command);
            HandleResult(result);
        }
        #endregion

        #region Private Methods
        private static void HandleResult(List<string> result)
        {
            if (result != null)
            {
                ConsoleOutput.SetLabel(result);
            }
        }
        #endregion

    }
}
