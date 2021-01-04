using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SQLineCore.DatabaseItem
{
    public class ViewInfo
    {
        public string SchemaName = string.Empty;
        public string ViewName = string.Empty;
        public string FullName => $"{SchemaName}.{ViewName}";
        public BigInteger ObjectId = 0;
        public List<ColumnInfo> Columns = new List<ColumnInfo>();
    }
}
