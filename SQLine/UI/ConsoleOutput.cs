using System;
using System.Collections.Generic;
using System.Data;
using Terminal.Gui;
using Terminal.Gui.Views;

namespace SQLine
{
    static class ConsoleOutput
    {
        #region Public Properties
        internal static Window Window { get; set; }
        #endregion

        #region Private Fields
        static List<string> _outputList = new List<string>();
        static ListView _output;
        static TableView _table;
        static TableStyle _tableStyle;
        #endregion

        #region Public Methods
        internal static void Init()
        {
            Window = new Window("Output [Scroll Up To See History]")
            {
                X = 0,
                Y = 22,
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

            _table = new TableView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            _tableStyle = new TableStyle();
            _tableStyle.AlwaysShowHeaders = true;

            _table.Style = _tableStyle;

            Window.Add(_output);
            Window.Add(_table);

            HideTable();
        }

        internal static void HideTable()
        {
            _table.Visible = false;
        }

        internal static void ShowTable()
        {
            _table.Visible = true;
        }

        internal static void HideOutput()
        {
            _output.Visible = false;
        }

        internal static void ShowOutput()
        {
            _output.Visible = true;
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
            SetListViewToFill();
        }

        internal static void SetHeight(int height)
        {
            Window.Height = height;
            SetListViewToFill();
        }

        internal static void SetLabel(List<string> contents)
        {
            _outputList.Add(DateTime.Now.ToString() + " >>"); ;
            _outputList.AddRange(contents);
            HideTable();
            ShowOutput();
            SetCurrentSelectedPosition();
        }

        internal static void ShowTableData(DataTable data)
        {
            
            _table.Table = data;
            HideOutput();
            ShowTable();
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
        private static void SetListViewToFill()
        {
            _output.Width = Dim.Fill();
            _output.Height = Dim.Fill();
        }

        private static void SetCurrentSelectedPosition()
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