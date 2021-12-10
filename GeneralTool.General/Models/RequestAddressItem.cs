using System;
using System.Reflection;

using GeneralTool.General.NetHelper;

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

        /// <summary>
        /// 请求方法
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        #endregion Public 属性
    }
}