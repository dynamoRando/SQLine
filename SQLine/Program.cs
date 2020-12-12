using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        static string prefix = "-> ";

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

            if (App._mode == AppMode.ConnectedToServer)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write(App._serverName);
                Console.ForegroundColor = currentForeground;
                Console.BackgroundColor = currentBackground;
                Console.Write(" " + prefix);
            }
            else
            {
                Console.Write(prefix);
            }

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
            var match = data.FirstOrDefault(item => item != currentInput && item.StartsWith(currentInput, true, CultureInfo.InvariantCulture));
            if (string.IsNullOrEmpty(match))
            {
                return;
            }

            ClearCurrentLine();
            _builder.Clear();

            Console.Write(match);
            _builder.Append(match);
        }

        private static void HandleEnterKeyInput()
        {
            Console.WriteLine();
            if (App._mode == AppMode.PendingConnection)
            {
                App.Connect(_builder.ToString());
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
