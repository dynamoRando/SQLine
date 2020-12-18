using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using core = SQLineCore;

namespace SQLineGUI.UI
{
    static class ConsoleInput
    {
        #region Public Properties
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        static Label _test;
        static TextField _input;
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

            _input = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(75),
            };

            _test = new Label()
            {
                X = Pos.Right(_input) + 1,
                Y = Pos.Top(_input),
                Width = Dim.Fill(1)
            };

            Window.Add(_input);
            Window.Add(_test);
            Window.KeyPress += Window_KeyPress;
        }

        #endregion

        #region Private Methods
        private static void Window_KeyPress(View.KeyEventEventArgs obj)
        {
            _test.Text = obj.KeyEvent.Key.ToString();
            string input = _input.Text.ToString();
            Key key = obj.KeyEvent.Key;
            var result = new List<string>();

            if (input == string.Empty && key != Key.CursorUp)
            {
                return;
            }

            switch (key)
            {
                case Key.Enter:
                    KeyUpBehavior.ResetKeyUpCount();
                    KeyUpBehavior.AddCommandToHistory(input);
                    result = core.App.ParseCommand(input);
                    _input.Text = string.Empty;
                    HandleResult(result);
                    break;
                case Key.Tab:
                    break;
                case Key.CursorUp:
                    string line = KeyUpBehavior.HandleKeyUp();
                    result.Add(line);
                    HandleResult(result);
                    break;
                default:
                    break;
            }
        }
        #endregion

        private static void HandleResult(List<string> result)
        {
            if (result != null)
            {
                ConsoleOutput.SetLabel(result);
            }
        }
    }
}
