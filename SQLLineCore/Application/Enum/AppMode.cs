using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public enum AppMode
    {
        Unknown,
        PendingConnection,
        ConnectedToServer,
        UsingDatabase
    }
}
