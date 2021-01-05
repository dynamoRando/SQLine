using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using core = SQLineCore;
using System.Reflection;
using System.IO;
using SQLineCore;

namespace SQLine
{
    /// <summary>
    /// Represents the main UI window. From this window, all other sub-windows are added
    /// </summary>
    public static class AppLine
    {
        #region Public Properties
        internal static Toplevel Top { get; set; }
        internal static Window Window { get; set; }
        #endregion

        #region Public Methods
        public static void Init()
        {
            core.App.Mode = core.AppMode.PendingConnection;

            Top = new Toplevel();
            Window = new Window()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            Application.Init();
            ConsoleInput.Init();
            ConsoleOutput.Init();
            TextEditor.Init();

            TextEditor.Hide();

            AddWindows();
            Top.Add(Window);

            LoadSettings();
            SetupTestSettings();

            Application.Top.Add(Top);
            Application.Run();
        }
        #endregion


        #region Private Methods

        private static void AddWindows()
        {
            Window.Add(ConsoleInput.Window);
            Window.Add(ConsoleOutput.Window);
            Window.Add(TextEditor.Window);
        }

        private static void LoadSettings()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            var folder = System.IO.Path.GetDirectoryName(location);

            foreach (var file in Directory.GetFiles(folder))
            {
                if (file.Contains("app.settings", StringComparison.CurrentCultureIgnoreCase))
                {
                    core.App.LoadAppSettings(file);
                }
            }
        }

        private static void SetupTestSettings()
        {
            if (AppCache.Settings == null)
            {
                var location = Assembly.GetExecutingAssembly().Location;
                var folder = System.IO.Path.GetDirectoryName(location);


                AppCache.Settings = new AppSettings();
                AppCache.Settings.Connections = new List<ConnectionPreference>();

                var connection = new ConnectionPreference();
                connection.Nickname = "dev";
                connection.UserName = "sa";
                connection.Password = "";
                connection.ServerName = "localhost";

                AppCache.Settings.Connections.Add(connection);

                App.SaveAppSettings(Path.Join(folder, "app.settings"));
            }
        }
        #endregion
    }
}
