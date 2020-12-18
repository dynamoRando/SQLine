using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLineGUI.UI
{
    static class ConsoleInput
    {
        #region Public Properties
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        static Label _test;
        #endregion

        #region Public Methods
        static public void Init()
        {
            Window = new Window("Console")
            {
                X = 0,
                Y = 1,
                Width = 100,
                Height = 5 
            };

            var textField = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(75),
            };

            _test = new Label()
            {
                X = Pos.Right(textField) + 1,
                Y = Pos.Top(textField),
                Width = Dim.Fill(1)
            };

            Window.Add(textField);
            Window.Add(_test);
            Window.KeyPress += Window_KeyPress;
        }

        #endregion

        #region Private Methods
        private static void Window_KeyPress(View.KeyEventEventArgs obj)
        {
            _test.Text = obj.KeyEvent.Key.ToString();
        }
        #endregion
    }
}
