using System;

namespace GeneralTool.CoreLibrary.DbHelper
{
    /// <summary>
    /// 原始数据库中的字段名称,当自定义类的名称不与数据库相同时使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnPropertyAttribute : Attribute
    {
        /// <summary>
        /// 数据库原始字段名称
        /// </summary>
        public string DataName { get; set; }

        /// <summary>
        /// 数据库字段类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// 确定自定义类属性映射到数据库字段上的名称
        /// </summary>
        /// <param name="dataName">对应数据库字段上的名称</param>
        public DataColumnPropertyAttribute(string dataName)
        {
            DataName = dataName.ToLower();
        }

    }
}
