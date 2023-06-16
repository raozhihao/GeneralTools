using System;

namespace GeneralTool.CoreLibrary.DbHelper
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultValueAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string DefaultValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultValue"></param>
        public DefaultValueAttribute(string defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}
