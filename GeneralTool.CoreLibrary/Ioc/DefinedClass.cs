using System;
using System.Reflection;

namespace GeneralTool.CoreLibrary.Ioc
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
        /// 是否为未调用构造函数的对象
        /// </summary>
        public bool IsUninitializedObject { get; set; }
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
