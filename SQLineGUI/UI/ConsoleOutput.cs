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
        static List<string> _outputList = new List<string>();
        static Label _label;
        static ListView _output;
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

            //_label = new Label(string.Empty)
            //{
            //    X = 0,
            //    Y = 0,
            //    Width = Dim.Fill(),
            //    Height = Dim.Fill(),
            //};

            _output = new ListView(_outputList)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            Window.Add(_output);
        }

        internal static void SetLabel(List<string> contents)
        {
            _outputList.Add(DateTime.Now.ToString() + " >>"); ;
            _outputList.AddRange(contents);
            SetCurrentSeletedPosition();
        }
        #endregion

        #region Private Methods
        private static void SetCurrentSeletedPosition()
        {
            int maxEntry = 0;

            foreach (var line in _outputList)
            {
                if (line.EndsWith(">>"))
                {
                    var item = line.Replace(" >>", string.Empty);
                    DateTime date;
                    if (DateTime.TryParse(item, out date))
                    {
                        int index = _outputList.IndexOf(line);
                        if (index > maxEntry)
                        {
                            maxEntry = index;
                        }
                    }
                }
            }

            _output.SelectedItem = maxEntry;
        }
        #endregion
    }
}
