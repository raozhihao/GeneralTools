using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 
    /// </summary>
    public struct ParamterConvertItem
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Func<string, object> Converter { get; set; }
    }
}
