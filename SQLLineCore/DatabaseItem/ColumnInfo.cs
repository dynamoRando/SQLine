using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLineCore
{
    public class ColumnInfo
    {
        public string ColumnName = string.Empty;
        public bool IsNullable = false;
        public int MaxLength = 0;
        public string DataType = string.Empty;
        public int Ordinal = 0;
    }
}
