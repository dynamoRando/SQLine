using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLineGUI.UI
{
    public static class AppLine
    {
        static Window win;

        public static void Init()
        {
            Application.Init();
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => {
                    Application.RequestStop ();
                })
            }),
        });

            win = new Window("asdf")
            {
                X = 0,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };

            win.KeyDown += Win_KeyDown;

            // Add both menu and win in a single call
            Application.Top.Add(menu, win);
            Application.Run();
        }

        private static void Win_KeyDown(View.KeyEventEventArgs obj)
        {
            win.Title = obj.KeyEvent.KeyValue.ToString();
        }
    }
}
