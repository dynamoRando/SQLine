using System;

namespace SQLineCore
{
    public static class AppCommandQuitExit
    {
        public static void HandleQuitOrExit()
        {
            Environment.Exit(0);
        }
    }
}