using System;
using System.Collections.Generic;

namespace GeneralTool.General.LinqExtensions
{
    /// <summary>
    /// 对IEnumerable进行扩展
    /// </summary>
    public static class IEnumerableExtensions
    {

        /// <summary>
        /// 对IEnumerable进行循环计算
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        public static void Foreach<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (T item in ts)
            {
                action?.Invoke(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="action"></param>
        public static void For<T>(this IEnumerable<T> ts, Action<int, T> action)
        {
            int index = 0;
            foreach (T item in ts)
            {
                action(index++, item);
            }
        }


    }
}
