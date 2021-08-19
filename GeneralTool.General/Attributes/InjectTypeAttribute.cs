using System;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 需要注册的类型属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InjectTypeAttribute : Attribute
    {
    }
}
