using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralTool.CoreLibrary.Extensions
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
            foreach (Tin item in enumables)
            {
                yield return converter != null ? converter(item) : (Tout)Convert.ChangeType(item, typeof(Tout));
            }
        }

        public static IEnumerable<Tout> Converts<Tin, Tout>(this IEnumerable<Tin> enumables) where Tin : IConvertible
        {
            foreach (Tin item in enumables)
            {
                yield return (Tout)Convert.ChangeType(item, typeof(Tout));
            }
        }

        public static IEnumerable<object> Converts<Tin>(this IEnumerable<Tin> enumables, Type convertType)
        {
            foreach (Tin item in enumables)
            {
                yield return Convert.ChangeType(item, convertType);
            }
        }

        /// <summary>
        /// 找到对应的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumables"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static int FindIndexEx<T>(this IEnumerable<T> enumables, Predicate<T> predicate)
        {
            int index = -1;
            foreach (T item in enumables)
            {
                if (predicate != null)
                {
                    if (!predicate(item))
                    {
                        return index + 1;
                    }
                }

            }
            return index;
        }

        /// <summary>
        /// 随机获取集合中的一项数据
        /// </summary>
        /// <typeparam name="T">要操作的集合类型</typeparam>
        /// <param name="enumables">要操作的集合</param>
        /// <param name="startIndex">开始下标</param>
        /// <returns></returns>
        public static T RandomTout<T>(this IEnumerable<T> enumables, int startIndex = 0)
        {
            if (!enumables.Any())
                return default;

            if (startIndex + 1 == enumables.Count() || startIndex == 0)
                return enumables.ElementAtOrDefault(startIndex);

            int index = RandomEx.Next(startIndex, enumables.Count());
            return enumables.ElementAtOrDefault(index);
        }
        #endregion Public 方法
    }
}