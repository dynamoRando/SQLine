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
        static Label _label;
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

            _label = new Label(string.Empty)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            _label.Text = "FOOBAR";

            Window.Add(_label);
        }

        internal static void SetLabel(string contents)
        {
            _label.Text += contents;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
