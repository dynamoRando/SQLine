using SQLineCore;
using SQLine.UI.UIChanges;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLine
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
                case UICommands.OUTPUT:
                    ConsoleOutput.Hide();
                    break;
                case UICommands.EDITOR:
                    TextEditor.Hide();
                    break;
                case UICommands.GUIDE:
                    ConsoleInput.HideGuide();
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
                case UICommands.OUTPUT:
                    ConsoleOutput.Show();
                    break;
                case UICommands.EDITOR:
                    TextEditor.Show();
                    break;
                case UICommands.GUIDE:
                    ConsoleInput.ShowGuide();
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
