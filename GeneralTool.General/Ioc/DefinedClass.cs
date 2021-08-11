using System;
using System.Reflection;

namespace GeneralTool.General.Ioc
{
    /// <summary>
    /// 定义类
    /// </summary>
    class DefinedClass
    {
        /// <summary>
        /// 实例
        /// </summary>
        public object Instance { get; set; }
        /// <summary>
        /// 初始化方法
        /// </summary>
        public MethodInfo InitMethod { get; set; }
        /// <summary>
        /// 接口类型
        /// </summary>
        public Type InterfaceType { get; set; }
        /// <summary>
        /// 实例类型
        /// </summary>
        public Type TType { get; set; }
    }
}
