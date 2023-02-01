using System;

namespace GeneralTool.General.DbHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class DataTypeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataType"></param>
        public DataTypeAttribute(string dataType)
        {
            this.DataType = dataType;
        }
    }
}
