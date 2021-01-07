using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SQLineCore.DatabaseItem
{
    public class ProcedureInfo
    {
        public string SchemaName = string.Empty;
        public string ProcedureName = string.Empty;
        public string FullName => $"{SchemaName}.{ProcedureName}";
        public BigInteger ObjectId = 0;
    }
}
