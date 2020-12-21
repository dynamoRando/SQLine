using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using core = SQLineCore;

namespace SQLineGUI
{
    public static class AppLine
    {
        #region Public Properties
        internal static Toplevel Top { get; set; }
        internal static Window Window { get; set; }
        #endregion


        #region Public Methods
        public static void Init()
        {
            core.App.Mode = core.AppMode.PendingConnection;

            Top = new Toplevel();
            Window = new Window()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            Application.Init();
            ConsoleInput.Init();
            ConsoleOutput.Init();
            TextEditor.Init();

            TextEditor.Hide();

            AddWindows();
            Top.Add(Window);

            Application.Top.Add(Top);
            Application.Run();
        }
        #endregion


        #region Private Methods
     
        private static void AddWindows()
        {
            Window.Add(ConsoleInput.Window);
            Window.Add(ConsoleOutput.Window);
            Window.Add(TextEditor.Window);
        }
        #endregion
    }
}
