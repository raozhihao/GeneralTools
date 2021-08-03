using System;
using System.Reflection;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// </summary>
    [Serializable]
    public struct RequestAddressItem
    {
        #region Public 属性

        /// <summary>
        /// 
        /// </summary>
        public MethodInfo MethodInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// </summary>
        public string Url { get; set; }

        #endregion Public 属性
    }
}