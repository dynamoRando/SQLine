using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLineGUI
{
    static class TextEditor
    {
        #region Public Fields
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        #endregion

        #region Public Methods
        internal static void Init()
        {
            Window = new Window("Editor")
            {
                X = 102,
                Y = 1,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
        }

        internal static void Hide()
        {
            Window.Visible = false;
        }

        internal static void Show()
        {
            Window.Visible = true;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
