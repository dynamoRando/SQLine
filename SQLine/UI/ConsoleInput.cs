﻿using System;
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
        #endregion

        #region Public Methods
        static internal void Init()
        {
            Window = new Window("Console [Press Esc to quit application]")
            {
                X = 0,
                Y = 1,
                Width = 100,
                Height = 20
            };

            _input = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(95),
            };

            _statusUpdate = new Label()
            {
                X = 1,
                Y = Pos.Bottom(_input) + 1,
                Width = Dim.Percent(90)
            };

            var labelPossibleCommands = new Label()
            {
                X = 1,
                Y = Pos.Bottom(_statusUpdate) + 1,
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

            //_labelCommandDescription.Text = "[Command Description] /r/n" + Environment.NewLine + " foo";

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

            Window.Add(_input);
            Window.Add(_statusUpdate);
            Window.Add(labelPossibleCommands);
            Window.Add(_listPossibleCommandsView);
            Window.Add(_labelCommandDescription);
            Window.Add(labelCommandExamples);
            Window.Add(_listPossibleCommandExampleView);

            Debug.WriteLine("Registering Key Events");

            _input.KeyDown += _input_KeyDown;
            _input.KeyUp += _input_KeyUp;
            
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

        #endregion

        #region Private Methods
        private static void _input_KeyUp(View.KeyEventEventArgs obj)
        {
            string input = _input.Text.ToString();
            Key key = obj.KeyEvent.Key;

            switch (key)
            {
                case Key.Enter:
                    if (input == string.Empty)
                    {
                        return;
                    }
                    else
                    {
                        ResetCommandSuggestions();
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
                    HandleCommandSuggestions();
                    break;
            }
        }

        private static void _input_KeyDown(View.KeyEventEventArgs obj)
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
                        ResetCommandSuggestions();
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

        private static void _listPossibleCommandsView_SelectedItemChanged(ListViewItemEventArgs obj)
        {
            var item = _listPossibleCommandsView.SelectedItem;

            if (_listPossibleCommands.Count() == 0)
            {
                return;
            }

            var selectedCommand = _listPossibleCommands[item];
            var possibleCommand = core.AppCommands.GetAppCommandDetails().
                Where(c => c.CommandText.StartsWith(selectedCommand, StringComparison.CurrentCultureIgnoreCase)).ToList().FirstOrDefault();

            if (possibleCommand != null)
            {
                _commandExamples.Clear();
                _commandExamples.AddRange(possibleCommand.CommandExamples);
                _labelCommandDescription.Text = possibleCommand.CommandDescription;
            }
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
                var possibleCommands = core.AppCommands.GetAppCommandDetails().Where(c => c.CommandText.StartsWith(input, StringComparison.CurrentCultureIgnoreCase)).ToList();
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
            core.App.ExecutingQuery += App_ExecutingQuery;
            core.App.ExecutedQuery += App_ExecutedQuery;
        }

        private static void App_ExecutedQuery(object sender, EventArgs e)
        {
            _statusUpdate.Text = "Executed Query.";
        }

        private static void App_ExecutingQuery(object sender, EventArgs e)
        {
            _statusUpdate.Text = "Executing Query...";
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
