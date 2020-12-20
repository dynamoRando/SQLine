using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace SQLineGUI
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
        static ScrollView _outputScroll;
        #endregion

        #region Public Methods
        internal static void Init()
        {
            Window = new Window("Output [Scroll Up To See History]")
            {
                X = 0,
                Y = 7,
                Width = 100,
                Height = Dim.Fill()
            };

            _output = new ListView(_outputList)
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            //_outputScroll = new ScrollView(new Rect(2, 2, 50, 20))
            _outputScroll = new ScrollView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ContentSize = new Size(200, 100),
                ShowVerticalScrollIndicator = true,
                ShowHorizontalScrollIndicator = true,
            };

            _outputScroll.Add(_output);

            _outputScroll.KeepContentAlwaysInViewport = true;

            Window.Add(_outputScroll);
            
            //Window.Add(_output);
        }

        internal static void SetLabel(string content)
        {
            var list = new List<string>();
            list.Add(content);
            SetLabel(list);
        }

        internal static void SetWidth(int width)
        {
            Window.Width = width;
        }

        internal static void SetHeight(int height)
        {
            Window.Height = height;
        }

        internal static void SetLabel(List<string> contents)
        {
            _outputList.Add(DateTime.Now.ToString() + " >>"); ;
            _outputList.AddRange(contents);
            SetCurrentSeletedPosition();
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
        private static void SetScrollViewContentSize(int width, int height)
        {
            _outputScroll.ContentSize = new Size(width, height);
        }
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
                        int index = _outputList.LastIndexOf(line);

                        if (index > maxEntry)
                        {
                            maxEntry = index;
                        }
                    }
                }
            }

            _output.MoveEnd();
            _output.SelectedItem = maxEntry;
        }
        #endregion
    }
}
