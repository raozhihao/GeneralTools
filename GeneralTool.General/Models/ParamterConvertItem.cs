using System;

namespace GeneralTool.General.Models
{
    internal struct ParamterConvertItem
    {
        public Type Type { get; set; }

        public Func<string, object> Converter { get; set; }
    }
}
