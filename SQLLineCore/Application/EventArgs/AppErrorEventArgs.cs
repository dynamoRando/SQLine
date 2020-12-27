using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SQLineCore.Application.EventArgs
{
    public class AppErrorEventArgs : System.EventArgs
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public StackTrace StackTrace { get; set; } 
    }
}
