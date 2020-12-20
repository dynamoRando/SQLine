using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineCore.Application.CommandProcessing
{
    internal static class AppCommandShowHide
    { 
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static void HandleHide(string command)
        {
            var windowName = command.Replace(AppCommands.HIDE, string.Empty).Trim().ToLower();

            switch(windowName)
            {
                case "console":
                    ConsoleOut
                    break;
                case "output":
                    break;
                default:
                    break;
            }
        }

        internal static void HandleShow(string command)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
