using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineCore
{
    /// <summary>
    /// Handles all application commands that are related to views in a database
    /// </summary>
    internal static class AppCommandQuestionView
    {
        #region Public Methods
        internal static List<string> HandleCommand(string command)
        {
            var result = new List<string>();

            if (command.Equals(AppCommands.QUESTION_VIEW, StringComparison.CurrentCultureIgnoreCase))
            {
                AppViewAction.GetViews();
                result = AppViewAction.ListViews();
            }

            return result;
        }
        #endregion
    }
}
