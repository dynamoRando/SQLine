using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLineGUI.UI
{
    static class ConsoleOutput
    {
        #region Public Properties
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        static Label _test;
        #endregion

        #region Public Methods
        internal static void Init()
        {
            Window = new Window("Output")
            {
                X = 0,
                Y = 7,
                Width = 100,
                Height = Dim.Fill()
            };

            var textField = new TextField(string.Empty)
            {
                X = 1,
                Y = 1,
                Width = Dim.Percent(50),
            };

            _test = new Label()
            {
                X = Pos.Right(textField) + 1,
                Y = Pos.Top(textField),
                Width = Dim.Fill(1)
            };
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
