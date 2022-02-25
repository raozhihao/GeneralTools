using System;
using System.Collections;
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


        /// <summary>
        /// 随机获取集合中的一项数据
        /// </summary>
        /// <typeparam name="T">要操作的集合类型</typeparam>
        /// <param name="enumables">要操作的集合</param>
        /// <param name="startIndex">开始下标</param>
        /// <returns></returns>
        public static T RandomTout<T>(this T[] enumables, int startIndex = 0)
        {
            if (enumables.Length == 0)
                return default;

            if (startIndex + 1 == enumables.Length || startIndex == 0)
                return enumables[startIndex];

            var index = RandomEx.Next(startIndex, enumables.Length);
            return enumables[index];
        }

        /// <summary>
        /// 将指定类型的一维数组放入二维数组中
        /// </summary>
        /// <param name="enumables">要操作的数据集合</param>
        /// <param name="rows">二维数组的行数</param>
        /// <param name="cols">二维数组的列数</param>
        /// <returns></returns>
        public static object[,] ToSecondArrary(this IList enumables, int rows, int cols)
        {
            //将一维数组放入二维数组中
            var arr = new object[rows, cols];
            var count = enumables.Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var index = cols * i + j;
                    if (index >= count)
                    {
                        break;
                    }
                    arr[i, j] = enumables[index];
                }
            }

            return arr;
        }

        /// <summary>
        /// 将指定类型的一维数组放入二维数组中
        /// </summary>
        /// <param name="enumables">要操作的数据集合</param>
        /// <param name="rows">二维数组的行数</param>
        /// <param name="cols">二维数组的列数</param>
        /// <returns></returns>
        public static T[,] ToSecondArrary<T>(this IList<T> enumables, int rows, int cols)
        {
            //将一维数组放入二维数组中
            var arr = new T[rows, cols];
            var count = enumables.Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var index = cols * i + j;
                    if (index >= count)
                    {
                        break;
                    }
                    arr[i, j] = enumables[index];
                }
            }

            return arr;
        }
        #endregion Public 方法
    }
}