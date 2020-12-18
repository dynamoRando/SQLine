using SQLineGUI.UI.HandleKeyPress;
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
        static internal void SetInput(string input)
        {
            _input.Text = input;
        }

        static internal void Init()
        {
            Window = new Window("Console [Press Esc to quit application]")
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

            switch (key)
            {
                case Key.Enter:

                    if (input == string.Empty)
                    {
                        return;
                    }

                    EnterBehavior.HandleEnter(input);
                    _input.Text = string.Empty;
                    break;
                case Key.Tab:
                    break;
                case Key.CursorUp:
                    KeyUpBehavior.HandleKeyUp();
                    break;
                case Key.Esc:
                    EscBehavior.HandleEsc();
                    break;
                default:
                    break;
            }
        }
        #endregion


    }
}
