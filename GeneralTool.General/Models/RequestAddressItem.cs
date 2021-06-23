using System;
using System.Reflection;

namespace GeneralTool.General.Models
{
    [Serializable]
    public struct RequestAddressItem
    {
        public string Url { get; set; }

        public object Target { get; set; }

        public MethodInfo MethodInfo { get; set; }
    }
}
