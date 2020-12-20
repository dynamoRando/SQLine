using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineGUI.UI.UIChanges
{
    class UICommandSizeWindow
    {
        #region Private Fields
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static void HandleSizeWindow(string command)
        {
            // size window output -w 500 -h 500
            // size window editor -w 500 -h 500
            string input = command.Replace(UICommands.SIZE_WINDOW, string.Empty).Trim();

            if (input.Contains(UICommands.EDITOR))
            {
                SetEditor(input);
            }

            if (input.Contains(UICommands.OUTPUT))
            {
                SetOutput(input);
            }
        }
        #endregion

        #region Private Methods
        private static void SetEditor(string command)
        {
            // editor -w 500 -h 500
            command = command.Replace(UICommands.EDITOR, string.Empty).Trim();
            // -w 500 -h 500
            var values = GetHeightWidthValues(command);

            foreach (var value in values)
            {
                if (value.Contains("w"))
                {
                    string item = value.Replace("w", string.Empty).Trim();
                    TextEditor.SetWidth(Convert.ToInt32(item));
                }

                if (value.Contains("h"))
                {
                    string item = value.Replace("h", string.Empty).Trim();
                    TextEditor.SetHeight(Convert.ToInt32(item));
                }
            }

        }

        private static void SetOutput(string command)
        {
            // output -w 500 -h 500
            command = command.Replace(UICommands.OUTPUT, string.Empty).Trim();
            var values = GetHeightWidthValues(command);

            foreach (var value in values)
            {
                if (value.Contains("w"))
                {
                    string item = value.Replace("w", string.Empty).Trim();
                    ConsoleOutput.SetWidth(Convert.ToInt32(item));
                }

                if (value.Contains("h"))
                {
                    string item = value.Replace("h", string.Empty).Trim();
                    ConsoleOutput.SetHeight(Convert.ToInt32(item));
                }
            }
        }

        private static string[] GetHeightWidthValues(string command)
        {
            return command.Split("-");
        }
        #endregion

    }
}
