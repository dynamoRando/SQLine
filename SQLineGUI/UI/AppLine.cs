using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLineGUI.UI
{
    public static class AppLine
    {
        public static void Init()
        {
            Application.Init();
            ConsoleInput.Init();

            Application.Top.Add(ConsoleInput._console);
            Application.Run();
        }
    }
}
