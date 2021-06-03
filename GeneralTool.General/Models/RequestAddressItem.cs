using System.Reflection;

namespace GeneralTool.General.Models
{
    internal struct RequestAddressItem
    {
        public string Url { get; set; }

        public object Target { get; set; }

        public MethodInfo MethodInfo { get; set; }
    }
}
