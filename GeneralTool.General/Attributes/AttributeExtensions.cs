﻿using GeneralTool.General.LinqExtensions;

namespace GeneralTool.General.Attributes
{
    /// <summary>
    /// 特性扩展类
    /// </summary>
    public static class AttributeExtensions
    {
        /// <summary>
        /// 获取对象上的特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetAttributeByClass<T>(this object value)
        {
            T t = default(T);
            object[] customAttributes = value.GetType().GetCustomAttributes(inherit: false);

            customAttributes.Foreach(a =>
            {
                if (a is T)
                {
                    t = (T)a;
                    return;
                }
            });

            return t;
        }
    }
}
