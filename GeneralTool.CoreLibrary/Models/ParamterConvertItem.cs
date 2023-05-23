using System;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// </summary>
    public struct ParamterConvertItem
    {
        #region Public 属性

        /// <summary>
        /// </summary>
        public Func<string, object> Converter { get; set; }

        /// <summary>
        /// </summary>
        public Type Type { get; set; }

        #endregion Public 属性
    }
}