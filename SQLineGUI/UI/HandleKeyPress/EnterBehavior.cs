using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLineCore;
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
            TabBehavior.ResetTabValues();
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

            if (core.App.Mode == AppMode.ConnectedToServer || core.App.Mode == AppMode.UsingDatabase)
            {
                var label = string.Empty;
                if (!string.IsNullOrEmpty(core.AppCache.ServerName))
                { 
                    label = $"[Server]: {core.AppCache.ServerName}";
                }

                if(!string.IsNullOrEmpty(core.AppCache.CurrentDatabase))
                {
                    label += $" [Database]: {core.AppCache.CurrentDatabase}";
                }

                ConsoleInput.SetLabel(label);
            }

        }
        #endregion

    }
}
