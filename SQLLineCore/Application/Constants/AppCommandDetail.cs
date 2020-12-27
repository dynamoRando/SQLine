using System;
using System.Collections.Generic;
using System.Text;

namespace SQLineCore
{
    public class AppCommandDetail
    {
        public string CommandText { get; set; }
        public string CommandDescription { get; set; }
        public List<string> CommandExamples { get; set; }
    }
}
