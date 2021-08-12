using System;

namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 原始数据库中的字段名称,当自定义类的名称不与数据库相同时使用
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataPropertyAttribute : Attribute
    {
        /// <summary>
        /// 数据库原始字段名称
        /// </summary>
        public string DataName { get; set; }

        /// <summary>
        /// 确定自定义类属性映射到数据库字段上的名称
        /// </summary>
        /// <param name="dataName">对应数据库字段上的名称</param>
        public DataPropertyAttribute(string dataName)
        {
            this.DataName = dataName.ToLower();
        }

    }
}
