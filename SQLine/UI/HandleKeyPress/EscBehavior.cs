using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLine
{
    internal static class EscBehavior
    {
        #region Public Methods
        internal static void HandleEsc()
        {
            ConsoleOutput.SetLabel("Exiting Application...");
            Environment.Exit(0);
        }
        #endregion
    }
}
