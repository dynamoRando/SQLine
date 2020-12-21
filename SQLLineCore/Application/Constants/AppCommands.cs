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
        public static string USER_NAME = "un";
        public static string PASSWORD = "pw";
        public static string SERVERNAME = "sn";    
        public static string QUERY_KEYWORD = "q";
        public static string QUIT = "quit";
        public static string EXIT = "exit";
       
        public static List<string> GetCommands()
        {
            var result = new List<string>();

            result.Add(USE_KEYWORD);
            result.Add(QUESTION);
            result.Add(QUESTION_TABLE);
            result.Add(QUESTION_TABLES_UPDATE);
            result.Add(QUESTION_TABLE_SCHEMA);
            result.Add(QUESTION_DATABASES);
            result.Add(QUESTION_DATABASES_UPDATE);
            result.Add(CONNECT_KEYWORD);
            result.Add(QUERY_KEYWORD);

            return result;
        }
    }
}
