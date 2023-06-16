using System;

namespace GeneralTool.CoreLibrary.DbHelper
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
            DataType = dataType;
        }
    }
}
