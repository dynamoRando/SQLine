using System;
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

        public static List<AppCommandDetail> GetAppCommandDetails()
        {
            var result = new List<AppCommandDetail>();

            var command = new AppCommandDetail();
            command.CommandText = USE_KEYWORD;
            command.CommandDescription = "Specifies the current database";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add($"{USE_KEYWORD} <databaseName>");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUESTION;
            command.CommandDescription = "Used to list some properties about the server or database";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add("? dbs|t");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUESTION_TABLE;
            command.CommandDescription = "Lists the tables in the current database";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(QUESTION_TABLE);
            command.CommandExamples.Add(QUESTION_TABLE + " <prefix>");
            command.CommandExamples.Add($"{QUESTION_TABLE} -schema <schemaName> <tablePrefix>");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUESTION_TABLES_UPDATE;
            command.CommandDescription = "Updates the current cache and then lists the tables in the current database";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(QUESTION_TABLES_UPDATE);

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUESTION_TABLE_SCHEMA;
            command.CommandDescription = "Lists the schema for the specified table";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(QUESTION_TABLE_SCHEMA + " <tableName>");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUESTION_DATABASES;
            command.CommandDescription = "Lists the databases on the current server";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(QUESTION_DATABASES);

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUESTION_DATABASES_UPDATE;
            command.CommandDescription = "Update the cache and then lists the database on the current server";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(QUESTION_DATABASES_UPDATE);

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = CONNECT_KEYWORD;
            command.CommandDescription = "Connect to a specific server. Specify only the server name if using trusted connection or preferred connection.";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(CONNECT_KEYWORD + " <serverName> (trusted connection) ");
            command.CommandExamples.Add(CONNECT_KEYWORD + " -sn <serverName> -un <userName> -pw <passWord> (regular connection)");
            command.CommandExamples.Add(CONNECT_KEYWORD + " <preferredConnection from app.settings>");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUERY_KEYWORD;
            command.CommandDescription = "Specify a query to run";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add("q select * from schema.TableName");

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = QUIT;
            command.CommandDescription = "Quits the application";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(QUIT);

            result.Add(command);

            command = new AppCommandDetail();
            command.CommandText = EXIT;
            command.CommandDescription = "Exits the application";
            command.CommandExamples = new List<string>();
            command.CommandExamples.Add(EXIT);

            result.Add(command);

            return result;
        }
    }
}
