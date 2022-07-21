using System;
using System.Reflection;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct RequestAddressItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
        ///
        public object Target { get; set; }
        ///
        public MethodInfo MethodInfo { get; set; }
    }
}
