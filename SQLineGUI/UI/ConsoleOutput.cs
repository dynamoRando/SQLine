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
        static TextView _output;
        static ScrollView _outputScroll;
        static int _scrollViewContentHeight = 0;
        static int _scrollViewContentWidth = 0;
        static int _windowWidth = 0;
        static int _windowHeight = 0;
        #endregion

        #region Public Methods
        internal static void Init()
        {
            _windowWidth = 250;

            Window = new Window("Output")
            {
                X = 0,
                Y = 7,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            _scrollViewContentWidth = 250;
            _scrollViewContentHeight = 250;

            _output = new TextView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            _outputScroll = new ScrollView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ContentSize = new Size(_scrollViewContentWidth, _scrollViewContentHeight),
                ShowVerticalScrollIndicator = true,
                ShowHorizontalScrollIndicator = true,
            };

            _outputScroll.KeepContentAlwaysInViewport = true;
            _outputScroll.Add(_output);

            Window.Add(_outputScroll);

        }

        internal static void SetLabel(string content)
        {
            var list = new List<string>();
            list.Add(content);
            SetLabel(list);
        }

        internal static void SetWidth(int width)
        {
            _windowWidth = width;
            Window.Width = _windowWidth;
            _scrollViewContentWidth = _windowWidth;
            SetScrollViewContentSize(_scrollViewContentWidth, _scrollViewContentHeight);
        }

        internal static void SetHeight(int height)
        {
            _windowHeight = height;
            Window.Height = _windowHeight;
            _scrollViewContentHeight = _windowHeight;
            SetScrollViewContentSize(_scrollViewContentWidth, _scrollViewContentHeight);
        }

        internal static void SetLabel(List<string> contents)
        {
            _outputList.Clear();

            _outputList.Add(DateTime.Now.ToString() + " >>"); ;
            _outputList.AddRange(contents);
            _output.Text = string.Join(Environment.NewLine, _outputList.ToArray());
            HandleUIChanges();
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
        private static void HandleUIChanges()
        {
            SetScrollViewContentSize(GetMaxWidthInCollection(), GetTotalHeightInCollection());

            _output.SetNeedsDisplay();
            _outputScroll.SetNeedsDisplay();
            Window.SetNeedsDisplay();
        }

        private static void SetScrollViewContentSize(int width, int height)
        {
            _outputScroll.ContentSize = new Size(width, height);
        }

        private static int GetMaxWidthInCollection()
        {
                int maxLength = 0;

                foreach(var line in _outputList)
                {
                    if (line.Length > maxLength)
                    {
                        maxLength = line.Length;
                    }
                }

                return maxLength;
        }

        private static int GetTotalHeightInCollection()
        {
            return _outputList.Count();
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
            //_output.SelectedItem = maxEntry;
        }
        #endregion
    }
}
