using System;
using System.Reflection;

namespace SQLine
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new App();

            Console.WriteLine($"SQL Line v{Assembly.GetExecutingAssembly().GetName().Version}");
           
            while (App.KeepRunning)
            {
                app.MainMenu();
            }
        }
    }
}
