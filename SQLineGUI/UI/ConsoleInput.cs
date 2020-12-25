using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using core = SQLineCore;

namespace SQLineGUI
{
    static class ConsoleInput
    {
        #region Public Properties
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        static Label _label;
        static TextField _input;
        static Label _statusUpdate;
        #endregion

        #region Public Methods
        static internal void SetInput(string input)
        {
            _input.Text = input;
        }

        internal static void SetLabel(string input)
        {
            _label.Text = input;
        }

        internal static void SetStatusLabel(string input)
        {
            _statusUpdate.Text = input;
        }

        static internal void Init()
        {
            Window = new Window("Console [Press Esc to quit application]")
            {
                X = 0,
                Y = 1,
                Width = 100,
                Height = 7
            };

            _input = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(40),
            };

            _label = new Label()
            {
                X = Pos.Right(_input) + 1,
                Y = Pos.Top(_input),
                Width = Dim.Fill(1)
            };

            _statusUpdate = new Label()
            {
                X = 1,
                Y = Pos.Bottom(_input) + 1,
                Width = Dim.Percent(40)
            };

            Window.Add(_input);
            Window.Add(_label);
            Window.Add(_statusUpdate);

            _statusUpdate.Text = "[STATUS_HERE]";

            Debug.WriteLine("Registering KeyDown Event");
            Window.KeyDown += Window_KeyDown;
        }

        private static void Window_KeyDown(View.KeyEventEventArgs obj)
        {
            string input = _input.Text.ToString();
            Key key = obj.KeyEvent.Key;

            Debug.WriteLine(DateTime.Now.ToString() + " ConsoleInput: " + key.ToString());

            switch (key)
            {
                case Key.Enter:
                    if (input == string.Empty)
                    {
                        return;
                    }
                    else
                    {
                        EnterBehavior.HandleEnter(input);
                        _input.Text = string.Empty;
                    }

                    break;
                case Key.Tab:
                    TabBehavior.HandleTab(input);
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

        #region Private Methods
        #endregion


    }
}
