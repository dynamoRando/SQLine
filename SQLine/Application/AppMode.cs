﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLine
{
    internal enum AppMode
    {
        Unknown,
        PendingConnection,
        ConnectedToServer,
        UsingDatabase
    }
}