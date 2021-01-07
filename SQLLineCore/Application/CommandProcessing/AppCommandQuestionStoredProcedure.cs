using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineCore
{
    internal static class AppCommandQuestionStoredProcedure
    {
        #region Public Methods
        internal static List<string> HandleCommand(string command)
        {
            var result = new List<string>();

            if (command.Equals(AppCommands.QUESTION_STORED_PROCEDURE, StringComparison.CurrentCultureIgnoreCase))
            {
                AppStoredProcedureAction.GetProcedures();
                result = AppStoredProcedureAction.ListProcedures();
            }

            return result;
        }
        #endregion
    }
}
