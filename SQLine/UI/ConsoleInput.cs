using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using core = SQLineCore;

namespace SQLine
{
    /// <summary>
    /// The console input window. A subwindow of AppLine
    /// </summary>
    static class ConsoleInput
    {
        #region Public Properties
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        static TextField _input;
        static Label _statusUpdate;
        static Label _labelCommandDescription;
        static ListView _listPossibleCommandsView;
        static ListView _listPossibleCommandExampleView;
        static List<string> _listPossibleCommands = new List<string>();
        static List<string> _commandExamples = new List<string>();
        static Window _commandGuideWindow;
        #endregion

        #region Public Methods
        static internal void Init()
        {
            Window = new Window("Console [Press Esc to quit application]")
            {
                X = 0,
                Y = 1,
                Width = 100,
                Height = 25
            };

            _input = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(95)
            };

            _statusUpdate = new Label()
            {
                X = 1,
                Y = Pos.Bottom(_input) + 1,
                Width = Dim.Percent(90)
            };

            _commandGuideWindow = new Window("Command Guide (Click to see description)")
            {
                X = 1,
                Y = Pos.Bottom(_statusUpdate) + 1,
                Width = Dim.Percent(90),
                Height = Dim.Fill()
            };

            var labelPossibleCommands = new Label()
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(90)
            };

            labelPossibleCommands.Text = " - Possible Commands:";

            _listPossibleCommandsView = new ListView(_listPossibleCommands)
            {
                X = 1,
                Y = Pos.Bottom(labelPossibleCommands) + 1,
                Width = Dim.Percent(95),
                Height = Dim.Percent(25)
            };

            _listPossibleCommandsView.SelectedItemChanged += _listPossibleCommandsView_SelectedItemChanged;

            _labelCommandDescription = new Label()
            {
                X = 1,
                Y = Pos.Bottom(_listPossibleCommandsView) + 1,
                Width = Dim.Percent(90),
                Height = 2
            };

            var labelCommandExamples = new Label()
            {
                X = 1,
                Y = Pos.Bottom(_labelCommandDescription) + 1,
                Width = Dim.Percent(90)
            };

            labelCommandExamples.Text = " - Examples:";

            _listPossibleCommandExampleView = new ListView(_commandExamples)
            {
                X = 1,
                Y = Pos.Bottom(labelCommandExamples) + 1,
                Width = Dim.Percent(95),
                Height = Dim.Percent(25)
            };

            //TestLayout();

            _commandGuideWindow.Add(labelPossibleCommands);
            _commandGuideWindow.Add(_listPossibleCommandsView);
            _commandGuideWindow.Add(_labelCommandDescription);
            _commandGuideWindow.Add(labelCommandExamples);
            _commandGuideWindow.Add(_listPossibleCommandExampleView);

            Window.Add(_input);
            Window.Add(_statusUpdate);
            Window.Add(_commandGuideWindow);

            Debug.WriteLine("Registering Key Events");

            // this seems stupid, but the Tab event does not always fire on the keyUp event
            // because it is trying to move to the next control to focus and there does not appear to be a 
            // way to prevent that from happening
            _input.KeyUp += _input_KeyUp;
            _input.KeyDown += _input_KeyDown;

            ListenForCoreEvents();
        }

        static internal void SetWindowTitle(string input)
        {
            Window.Title = $"Console {input}";
        }
        static internal void SetInput(string input)
        {
            _input.Text = input;
        }

        internal static void SetStatusLabel(string input)
        {
            _statusUpdate.Text = input;
        }

        internal static void ShowGuide()
        {
            _commandGuideWindow.Visible = true;
        }

        internal static void HideGuide()
        {
            _commandGuideWindow.Visible = false;
        }
        #endregion

        #region Private Methods

        private static void TestLayout()
        {
            _listPossibleCommands.Add("TEST 1");
            _listPossibleCommands.Add("TEST 2");
            _listPossibleCommands.Add("TEST 3");
        }

        private static void _input_KeyDown(View.KeyEventEventArgs obj)
        {
            Debug.WriteLine("KeyDown Event");

            string input = _input.Text.ToString();
            Key key = obj.KeyEvent.Key;

            Debug.WriteLine(DateTime.Now.ToString() + " ConsoleInput: " + key.ToString());
            Debug.WriteLine($"{DateTime.Now.ToString()} ConsoleInput Value: {input}");

            // we only handle the Tab keypress here since the KeyUp event was not always catching it
            switch (key)
            {
                case Key.Tab:
                    TabBehavior.HandleTab(input);
                    Window.FocusPrev();
                    break;
                default:
                    break;
            }
        }

        private static void _input_KeyUp(View.KeyEventEventArgs obj)
        {
            Debug.WriteLine("KeyUp Event");

            string input = _input.Text.ToString();
            Key key = obj.KeyEvent.Key;

            Debug.WriteLine(DateTime.Now.ToString() + " ConsoleInput: " + key.ToString());
            Debug.WriteLine($"{DateTime.Now.ToString()} ConsoleInput Value: {input}");
            
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
                        ResetCommandSuggestions();
                    }

                    break;
                case Key.Tab:
                    // this was not always being captured, so moved to the key down event
                    // TabBehavior.HandleTab(input);
                    // Window.FocusPrev();
                    break;
                case Key.CursorUp:
                    KeyUpBehavior.HandleKeyUp();
                    break;
                case Key.Esc:
                    EscBehavior.HandleEsc();
                    break;
                case Key.Backspace:
                    TabBehavior.ResetTabValues();
                    break;
                default:
                    HandleCommandSuggestions();
                    break;
            }
        }

        private static void _listPossibleCommandsView_SelectedItemChanged(ListViewItemEventArgs obj)
        {
            var item = _listPossibleCommandsView.SelectedItem;

            if (_listPossibleCommands.Count() == 0)
            {
                return;
            }

            var selectedCommand = _listPossibleCommands[item];

            var possibleCommand = GetAllCommands().
                Where(c => c.CommandText.StartsWith(selectedCommand, StringComparison.CurrentCultureIgnoreCase)).ToList().FirstOrDefault();

            if (possibleCommand != null)
            {
                _commandExamples.Clear();
                _commandExamples.AddRange(possibleCommand.CommandExamples);
                _labelCommandDescription.Text = possibleCommand.CommandDescription;
            }
        }

        private static List<core.AppCommandDetail> GetAllCommands()
        {
            var list = new List<core.AppCommandDetail>();
            list.AddRange(core.AppCommands.GetAppCommandDetails());
            list.AddRange(UICommands.GetUICommandDetails());

            return list;
        }

        private static void ResetCommandSuggestions()
        {
            _listPossibleCommands.Clear();
            _commandExamples.Clear();
            _labelCommandDescription.Text = string.Empty;
        }

        private static void HandleCommandSuggestions()
        {
            ResetCommandSuggestions();

            string input = _input.Text.ToString();

            Debug.WriteLine($"CommandSuggestions: {input}");

            if (input == string.Empty)
            {
                ResetCommandSuggestions();
                return;
            }
            else
            {
                var possibleCommands = GetAllCommands().Where(c => c.CommandText.StartsWith(input, StringComparison.CurrentCultureIgnoreCase)).ToList();
                _listPossibleCommands.AddRange(possibleCommands.Select(possible => possible.CommandText).ToList());

                if (_listPossibleCommands.Count == 1)
                {
                    var command = _listPossibleCommands.First();
                    var commandExamples = possibleCommands.Where(c => c.CommandText == command).First().CommandExamples;
                    _commandExamples.AddRange(commandExamples);
                }
            }
        }

        private static void ListenForCoreEvents()
        {
            core.App.GettingDatabases += HandleGettingDatabase;
            core.App.GotDatabases += App_GotDatabases;
            core.App.ConnectingToServer += HandleConnecting;
            core.App.ConnectedToServer += App_ConnectedToServer;
        }

        private static void App_GotDatabases(object sender, EventArgs e)
        {
            _statusUpdate.Text = "Got databases.";
        }

        private static void App_ConnectedToServer(object sender, EventArgs e)
        {
            _statusUpdate.Text = "Connected to server.";
        }

        private static void HandleConnecting(object sender, EventArgs e)
        {
            _statusUpdate.Text = "Connecting...";
        }

        private static void HandleGettingDatabase(object sender, EventArgs e)
        {
            _statusUpdate.Text = "Getting databases...";
        }
        #endregion


    }
}
