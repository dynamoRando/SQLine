using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

//UI example taken from:
//https://codereview.stackexchange.com/questions/139172/autocompleting-console-input
//clear console line
//https://stackoverflow.com/questions/8946808/can-console-clear-be-used-to-only-clear-a-line-instead-of-whole-console

namespace SQLine
{
    class Program
    {
        static StringBuilder _builder = new StringBuilder();
        static string _prefix = "-> ";

        static void Main(string[] args)
        {
            App._mode = AppMode.PendingConnection;

            Console.WriteLine($"SQLine v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            Console.WriteLine("---");
            Console.WriteLine("Press Esc to exit");
            Console.WriteLine();
            App.MainMenu();

            var data = new[]
            {
            "Bar",
            "Barbec",
            "Barbecue",
            "Batman",
        };
                       
            var input = Console.ReadKey(intercept: true);

            while (input.Key != ConsoleKey.Escape)
            {
                if (input.Key == ConsoleKey.Tab)
                {
                    HandleTabInput(data);
                }
                if (input.Key == ConsoleKey.Enter)
                {
                    HandleEnterKeyInput();
                    _builder.Clear();
                }
                else
                {
                    HandleKeyInput(data, input);
                }

                input = Console.ReadKey(intercept: true);
            }
            Console.Write(input.KeyChar);
        }

        internal static void ShowPrefix()
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            ConsoleColor currentBackground = Console.BackgroundColor;

            switch(App._mode)
            {
                case AppMode.ConnectedToServer:
                    WriteServerPrefix();
                    WriteEndingPrefix(currentForeground, currentBackground);
                    break;
                case AppMode.UsingDatabase:
                    WriteServerPrefix();
                    WriteDatabasePrefix();
                    WriteEndingPrefix(currentForeground, currentBackground);
                    break;
                default:
                    Console.Write(_prefix);
                    break;
            }
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

        private static void HandleTabInput(IEnumerable<string> data)
        {
            var currentInput = _builder.ToString();
            string match = string.Empty;

            if (App._mode == AppMode.ConnectedToServer)
            {
                currentInput = currentInput.Replace("use", string.Empty).Trim();

                // to do: need to cycle thru matches
                // will need to get a list of databases that starts with 
                // and then for each tab press cycle thru them

                match = App._databases.FirstOrDefault(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));
            }

            if (string.IsNullOrEmpty(match))
            {
                return;
            }

            ClearCurrentLine();
            _builder.Clear();

            if (App._mode == AppMode.ConnectedToServer)
            {
                string line = "use " + match;
                _builder.Append(line);
            }
            else
            {
                Console.Write(match);
                _builder.Append(match);
            }
        }

        private static void HandleEnterKeyInput()
        {
            Console.WriteLine();
            if (App._mode == AppMode.PendingConnection)
            {
                App.Connect(_builder.ToString());
            }
            else
            {
                App.ParseCommand(_builder.ToString());
            }
        }

        private static void HandleKeyInput(IEnumerable<string> data, ConsoleKeyInfo input)
        {
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
