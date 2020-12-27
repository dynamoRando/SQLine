using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SQLineCore;
using SQLine.UI.UIChanges;
using core = SQLineCore;

namespace SQLine
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

            try
            {
                if (IsUICommand(command))
                {
                    UIBehavior.HandleUICommand(command);
                }
                else
                {
                    if (AppCommandResult.IsText(command))
                    {
                        result = core.App.ParseCommand(command);
                        HandleResult(result);
                    }
                    else
                    {
                        DataTable item;
                        item = core.App.ParseQuery(command);
                        HandleResult(item);
                    }
                }

                ConsoleInput.SetStatusLabel(string.Empty);
            }
            catch (Exception e)
            {
                HandleError(e);
            }
        }
        #endregion

        #region Private Methods
        private static void HandleResult(DataTable table)
        {
            if (table != null)
            {
                ConsoleOutput.ShowTableData(table);
            }
        }

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

                if (!string.IsNullOrEmpty(core.AppCache.CurrentDatabase))
                {
                    label += $" [Database]: {core.AppCache.CurrentDatabase}";
                }

                ConsoleInput.SetLabel(label);
            }

        }

        private static void HandleError(Exception e)
        {
            ConsoleInput.SetStatusLabel($"Error: {e.Message}");
        }

        private static bool IsUICommand(string command)
        {
            bool isUICommand = false;

            if (UICommands.GetCommands().Any(c => command.StartsWith(c, StringComparison.CurrentCultureIgnoreCase)))
            {
                isUICommand = true;
            }
            else
            {
                isUICommand = false;
            }

            return isUICommand;
        }
        #endregion

    }
}
