using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLineCore;
using SQLineCore.DatabaseItem;

namespace SQLineCore
{
    public static class AppCache
    {
        #region Private Fields
        #endregion

        #region Public Properties
        public static string CurrentDatabase { get; set; }
        public static string ServerName { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static List<string> Databases { get; set; } = new List<string>();
        public static AppSettings Settings { get; set; }

        public static List<TableInfo> Tables { get; set; } = new List<TableInfo>();
        public static List<ViewInfo> Views { get; set; } = new List<ViewInfo>();
        public static List<ProcedureInfo> Procedures { get; set; } = new List<ProcedureInfo>();
        #endregion

        #region Constructors
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

    }
}
