using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLine.UI
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
        internal static void HandleEnter()
        {
            Console.WriteLine();
            if (App.Mode == AppMode.PendingConnection && ConsoleInterface.Builder.ToString() != "?")
            {
                App.Connect(ConsoleInterface.Builder.ToString());
            }
            else
            {
                App.ParseCommand(ConsoleInterface.Builder.ToString());
            }

            ConsoleInterface.ShowPrefix();
        }
        #endregion

        #region Private Methods
        #endregion

    }
}
