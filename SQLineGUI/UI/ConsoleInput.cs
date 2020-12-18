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

        #region Public Fields
        internal static Window _console;
        #endregion

        #region Private Fields
        static Label _test;
        #endregion


        static public void Init()
        {
            _console = new Window("Console")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            var textField = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(50),
            };

            _test = new Label()
            {
                X = Pos.Right(textField) + 1,
                Y = Pos.Top(textField),
                Width = Dim.Fill(1)
            };

            _console.Add(textField);
            _console.Add(_test);
            _console.KeyPress += _console_KeyPress;
        }

        private static void _console_KeyPress(View.KeyEventEventArgs obj)
        {
            _test.Text = obj.KeyEvent.Key.ToString();
        }
    }
}
