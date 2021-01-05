using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLine
{
    /// <summary>
    /// The text editor window. A subwindow of AppLine
    /// </summary>
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

        internal static void SetWidth(int width)
        {
            Window.Width = width;
        }

        internal static void SetHeight(int height)
        {
            Window.Height = height;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
