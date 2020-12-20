using SQLineCore;
using SQLineGUI.UI.UIChanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineGUI
{
    internal static class UICommandShowHide
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
            var windowName = command.Replace(UICommands.HIDE, string.Empty).Trim().ToLower();

            switch(windowName)
            {
                case "output":
                    ConsoleOutput.Hide();
                    break;
                case "editor":
                    TextEditor.Hide();
                    break;
                default:
                    break;
            }
        }

        internal static void HandleShow(string command)
        {
            var windowName = command.Replace(UICommands.SHOW, string.Empty).Trim().ToLower();

            switch (windowName)
            {
                case "output":
                    ConsoleOutput.Show();
                    break;
                case "editor":
                    TextEditor.Show();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
