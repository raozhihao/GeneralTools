using System;
using System.Collections.Generic;

namespace GeneralTool.General.LinqExtensions
{
    /// <summary>
    /// 为数组类型支持Linq查询
    /// </summary>
    public static class ArraryExtensions
    {
        #region Public 方法
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrary"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this T[] arrary, Predicate<T> match)
        {
            for (int i = 0; i < arrary.Length; i++)
            {
                if (match(arrary[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 查找指定值位置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrary"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this T[] arrary, T item)
        {
            for (int i = 0; i < arrary.Length; i++)
            {
                if (item.Equals(arrary[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 循环数组类型
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="arrary">
        /// </param>
        /// <param name="action">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> For<T>(this T[] arrary, Action<T> action)
        {
            for (int i = 0; i < arrary.Length; i++)
            {
                action?.Invoke(arrary[i]);
            }
            return arrary;
        }

        /// <summary>
        /// 循环数组类型
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="arrary">
        /// </param>
        /// <param name="action">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<T> Foreach<T>(this T[] arrary, Action<T> action)
        {
            foreach (T t in arrary)
            {
                action?.Invoke(t);
            }
            return arrary;
        }

        #endregion Public 方法
    }
}