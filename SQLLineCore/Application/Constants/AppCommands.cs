using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public static class AppCommands
    {
        public static string USE_KEYWORD = "use";
        public static string QUESTION = "?";
        public static string QUESTION_TABLE = "? t";
        public static string QUESTION_TABLES_UPDATE = "? t update";
        public static string QUESTION_TABLE_SCHEMA = "? t s";
        public static string QUESTION_DATABASES = "? dbs";
        public static string QUESTION_DATABASES_UPDATE = "? dbs update";
        public static string CONNECT_KEYWORD = "cn";
        public static string QUERY_KEYWORD = "q";
        public static string SHOW = "show";
        public static string HIDE = "hide";
    }
}
