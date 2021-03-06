﻿using SQLine.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using SQLineCore;

//UI example taken from:
//https://codereview.stackexchange.com/questions/139172/autocompleting-console-input

namespace SQLine
{
    class Program
    {
        static void Main(string[] args)
        {
            App.Mode = AppMode.PendingConnection;

            Console.WriteLine($"SQLine v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
            Console.WriteLine("---");
            Console.WriteLine("Press Esc to exit");
            Console.WriteLine();
            App.MainMenu();
            ConsoleInterface.ShowPrefix();

            var input = Console.ReadKey(intercept: true);

            while (input.Key != ConsoleKey.Escape)
            {
                switch (input.Key)
                {
                    case ConsoleKey.Tab:
                        HandleTabInput();
                        break;
                    case ConsoleKey.Enter:
                        KeyUpBehavior.ResetKeyUpCount();
                        KeyUpBehavior.AddCommandToHistory(ConsoleInterface.Builder.ToString());
                        HandleEnterKeyInput();
                        ConsoleInterface.Builder.Clear();
                        break;
                    case ConsoleKey.UpArrow:
                        KeyUpBehavior.HandleKeyUp();
                        break;
                    default:
                        HandleKeyInput(input);
                        break;
                }

                input = Console.ReadKey(intercept: true);
            }
            Console.Write(input.KeyChar);
        }

        private static void HandleTabInput()
        {
            TabBehavior.HandleTab();
        }

        private static void HandleEnterKeyInput()
        {
            EnterBehavior.HandleEnter();
        }

        private static void HandleKeyInput(ConsoleKeyInfo input)
        {
            TabBehavior.ResetTabValues();

            string currentInput = ConsoleInterface.Builder.ToString();
            if (input.Key == ConsoleKey.Backspace && currentInput.Length > 0)
            {
                ConsoleInterface.Builder.Remove(ConsoleInterface.Builder.Length - 1, 1);
                ConsoleInterface.ClearCurrentLine();

                currentInput = currentInput.Remove(currentInput.Length - 1);
                ConsoleInterface.ShowPrefix();
                Console.Write(currentInput);
            }
            else
            {
                var key = input.KeyChar;
                ConsoleInterface.Builder.Append(key);
                ConsoleInterface.ClearCurrentLine();
                ConsoleInterface.BuildCurrentConsoleLine();
            }
        }

       
    }
}
