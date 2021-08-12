using System;
using System.Collections.Generic;

namespace GeneralTool.General.LinqExtensions
{
    /// <summary>
    /// 对IEnumerable进行扩展
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Public 方法

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="ts">
        /// </param>
        /// <param name="action">
        /// </param>
        public static void For<T>(this IEnumerable<T> ts, Action<int, T> action)
        {
            int index = 0;
            foreach (T item in ts)
            {
                action(index++, item);
            }
        }

        /// <summary>
        /// 对IEnumerable进行循环计算
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="ts">
        /// </param>
        /// <param name="action">
        /// </param>
        public static void Foreach<T>(this IEnumerable<T> ts, Action<T> action)
        {
            foreach (T item in ts)
            {
                action?.Invoke(item);
            }
        }

        /// <summary>
        /// 将集合转换为另一个类型的集合
        /// </summary>
        /// <typeparam name="Tout">转换后的类型</typeparam>
        /// <typeparam name="Tin">集合类型</typeparam>
        /// <param name="enumables">要转换的集合</param>
        /// <param name="converter">转换器</param>
        /// <returns></returns>
        public static IEnumerable<Tout> ConvertAll<Tin, Tout>(this IEnumerable<Tin> enumables, Converter<Tin, Tout> converter = null)
        {
            foreach (var item in enumables)
            {
                if (converter != null)
                {
                    yield return converter(item);
                }
                else
                    yield return (Tout)Convert.ChangeType(item, typeof(Tout));
            }
        }

        #endregion Public 方法
    }
}