﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    /// <summary>
    /// Defines various SQL commands that are used in the console application
    /// </summary>
    public static class AppSQLCommand
    {
        internal static string GetTables = "SELECT s.name SchemaName, t.name TableName, object_id ObjectId FROM sys.tables t inner join sys.schemas s on t.schema_id = s.schema_id";
        internal static string GetTablesSchema = "SELECT c.name ColumnName, c.max_length ColumnMaxLength, c.is_nullable IsNullable, t.name ColumnDataType FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id join sys.objects o on c.object_id = o.object_id WHERE o.object_id = <objectId>";
        internal static string GetSystemTableInfo = "SELECT * FROM sys.databases";
    }
}
