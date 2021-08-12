using System;

namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 映射表名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTableAttribute : Attribute
    {
        /// <summary>
        /// 表名称
        /// </summary>
        public string DataTableName { get; set; }
        /// <summary>
        /// 映射表名称
        /// </summary>
        /// <param name="tableName">表名称</param>
        public DataTableAttribute(string tableName)
        {
            this.DataTableName = tableName.ToLower();
        }
    }
}
