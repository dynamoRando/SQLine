using System.Collections.Generic;

namespace SQLineCore
{
    public static class AppCommands
    {
        /// <summary>
        /// "use"
        /// </summary>
        public static string USE_KEYWORD = "use";

        /// <summary>
        /// "?"
        /// </summary>
        public static string QUESTION = "?";

        /// <summary>
        /// "? t"
        /// </summary>
        public static string QUESTION_TABLE = "? t";

        /// <summary>
        /// "? t update"
        /// </summary>
        public static string QUESTION_TABLES_UPDATE = "? t update";
        
        /// <summary>
        /// "? t s"
        /// </summary>
        public static string QUESTION_TABLE_SCHEMA = "? t s";
        
        /// <summary>
        /// "? dbs"
        /// </summary>
        public static string QUESTION_DATABASES = "? dbs";

        /// <summary>
        /// "? dbs update"
        /// </summary>
        public static string QUESTION_DATABASES_UPDATE = "? dbs update";

        /// <summary>
        /// "cn"
        /// </summary>
        public static string CONNECT_KEYWORD = "cn";

        /// <summary>
        /// "un"
        /// </summary>
        public static string USER_NAME = "un";

        /// <summary>
        /// "pw"
        /// </summary>
        public static string PASSWORD = "pw";

        /// <summary>
        /// "sn"
        /// </summary>
        public static string SERVERNAME = "sn";    

        /// <summary>
        /// "q"
        /// </summary>
        public static string QUERY_KEYWORD = "q";

        /// <summary>
        /// "quit"
        /// </summary>
        public static string QUIT = "quit";

        /// <summary>
        /// "exit"
        /// </summary>
        public static string EXIT = "exit";

        /// <summary>
        /// "schema"
        /// </summary>
        public static string SCHEMA = "schema";
       
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
            result.Add(QUIT);
            result.Add(EXIT);
            result.Add(SCHEMA);

            return result;
        }
    }
}
