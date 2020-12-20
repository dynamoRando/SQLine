using SQLineCore;
using SQLineGUI.UI.UIChanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineGUI
{
    static class UIBehavior
    { 
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        public static void HandleUICommand(string command)
        {
            if (command.StartsWith(UICommands.SHOW + " "))
            {
                UICommandShowHide.HandleShow(command);
            }

            if (command.StartsWith(UICommands.HIDE + " "))
            {
                UICommandShowHide.HandleHide(command);
            }

            if (command.StartsWith(UICommands.SIZE_WINDOW + " "))
            {
                UICommandSizeWindow.HandleSizeWindow(command);
            }
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
