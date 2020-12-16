using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public class TableInfo
    {
        public string SchemaName = string.Empty;
        public string TableName = string.Empty;
        public BigInteger ObjectId = 0;
        public List<ColumnInfo> Columns = new List<ColumnInfo>();
    }
}
