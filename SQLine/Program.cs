using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

//UI example taken from:
//https://codereview.stackexchange.com/questions/139172/autocompleting-console-input

namespace SQLine
{
    class Program
    {
        static StringBuilder _builder = new StringBuilder();
        static string _prefix = "-> ";
        static int _tabCount = 0;
        static string _tabPrefix = string.Empty;

        static void Main(string[] args)
        {
            App._mode = AppMode.PendingConnection;

            Console.WriteLine($"SQLine v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            Console.WriteLine("---");
            Console.WriteLine("Press Esc to exit");
            Console.WriteLine();
            App.MainMenu();
            ShowPrefix();

            var input = Console.ReadKey(intercept: true);

            while (input.Key != ConsoleKey.Escape)
            {
                switch(input.Key)
                {
                    case ConsoleKey.Tab:
                        HandleTabInput();
                        break;
                    case ConsoleKey.Enter:
                        HandleEnterKeyInput();
                        _builder.Clear();
                        break;
                    default:
                        HandleKeyInput(input);
                        break;
                }
                
                input = Console.ReadKey(intercept: true);
            }
            Console.Write(input.KeyChar);
        }

        internal static void ShowPrefix()
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            ConsoleColor currentBackground = Console.BackgroundColor;

            switch (App._mode)
            {
                case AppMode.ConnectedToServer:
                    WriteServerPrefix();
                    WriteEndingPrefix(currentForeground, currentBackground);
                    break;
                case AppMode.UsingDatabase:
                    WriteServerPrefix();
                    WriteSpacer(currentForeground, currentBackground);
                    WriteDatabasePrefix();
                    WriteEndingPrefix(currentForeground, currentBackground);
                    break;
                default:
                    Console.Write(_prefix);
                    break;
            }
        }

        private static void WriteSpacer(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(" > ");
        }

        private static void WriteDatabasePrefix()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write(App._currentDatabase);
        }

        private static void WriteEndingPrefix(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(" " + _prefix);
        }

        private static void WriteServerPrefix()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(App._serverName);
        }

        /// <remarks>
        /// https://stackoverflow.com/a/8946847/1188513
        /// </remarks>>
        private static void ClearCurrentLine()
        {
            var currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }

        private static void HandleTabInput()
        {
            if (_tabCount == 0)
            {
                _tabPrefix = _builder.ToString();
            }
            
            var currentInput = _tabPrefix;
            string outputItem = string.Empty;

            if (App._mode == AppMode.ConnectedToServer || App._mode == AppMode.UsingDatabase)
            {
                currentInput = currentInput.Replace("use", string.Empty).Trim();

                var matches = App._databases.Where(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));
                _tabCount++;

                if (_tabCount <= matches.Count())
                {
                    outputItem = matches.ToList()[_tabCount - 1];
                }
                else if (_tabCount > matches.Count())
                {
                    _tabCount -= matches.Count();
                    outputItem = matches.ToList()[_tabCount - 1];
                }

                ClearCurrentLine();
                ShowPrefix();
                string line = "use " + outputItem;
                _builder.Clear();
                _builder.Append(line);
                Console.Write(_builder.ToString());

                if (string.IsNullOrEmpty(outputItem))
                {
                    return;
                }
            }
        }

        private static void HandleEnterKeyInput()
        {
            Console.WriteLine();
            if (App._mode == AppMode.PendingConnection && _builder.ToString() != "?")
            {
                App.Connect(_builder.ToString());
            }
            else
            {
                App.ParseCommand(_builder.ToString());
            }

            Program.ShowPrefix();
        }

        private static void HandleKeyInput(ConsoleKeyInfo input)
        {
            _tabCount = 0;
            _tabPrefix = string.Empty;

            string currentInput = _builder.ToString();
            if (input.Key == ConsoleKey.Backspace && currentInput.Length > 0)
            {
                _builder.Remove(_builder.Length - 1, 1);
                ClearCurrentLine();

                currentInput = currentInput.Remove(currentInput.Length - 1);
                ShowPrefix();
                Console.Write(currentInput);
            }
            else
            {
                var key = input.KeyChar;
                _builder.Append(key);
                //Console.Write(key);
                ClearCurrentLine();
                BuildCurrentConsoleLine();
            }
        }

        private static void BuildCurrentConsoleLine()
        {
            ShowPrefix();
            Console.Write(_builder.ToString());
        }
    }
}
