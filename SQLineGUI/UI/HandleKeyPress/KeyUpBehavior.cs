using SQLineGUI.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineGUI
{
    static class KeyUpBehavior
    {
        #region Private Fields
        internal static int _keyUpCount = 0;
        internal static List<string> _enteredCommands = new List<string>();
        #endregion

        #region Public Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        internal static void ResetKeyUpCount()
        {
            _keyUpCount = 0;
        }

        internal static void ResetKeyUpHistory()
        {
            _enteredCommands.Clear();
        }

        internal static void HandleKeyUp()
        {
            _keyUpCount++;
            _enteredCommands.Reverse();

            string line = string.Empty;

            if (_keyUpCount <= _enteredCommands.Count())
            {
                line = _enteredCommands[_keyUpCount - 1];
            }
            else if (_keyUpCount > _enteredCommands.Count)
            {
                _keyUpCount -= _enteredCommands.Count();
                line = _enteredCommands[_keyUpCount - 1];
            }

            _enteredCommands.Reverse();

            HandleResult(line);
        }

        internal static void AddCommandToHistory(string command)
        {
            _enteredCommands.Add(command);
        }
        #endregion

        #region Private Methods
        private static void HandleResult(string result)
        {
            ConsoleInput.SetInput(result);
        }
        #endregion


    }
}
