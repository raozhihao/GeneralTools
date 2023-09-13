using System;
using System.Collections;
using System.Collections.Generic;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 为数组类型支持Linq查询
    /// </summary>
    public static class ArraryExtensions
    {
        #region Public 方法

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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrary"></param>
        /// <param name="action"></param>
        public static void For<T>(this IEnumerable<T> arrary, Action<T> action)
        {
            foreach (T item in arrary)
            {
                action?.Invoke(item);
            }
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
        /// <param name="count"></param>
        /// <returns></returns>
        public static T RandomTout<T>(this T[] enumables, int startIndex = 0, int count = -1)
        {
            if (enumables.Length == 0)
                return default;

            if (startIndex + 1 == enumables.Length || startIndex == 0)
                return enumables[startIndex];

            if (count < startIndex) count = startIndex + 1;
            if (count > enumables.Length) count = enumables.Length;

            int index = RandomEx.Next(startIndex, count);
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
            object[,] arr = new object[rows, cols];
            int count = enumables.Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = (cols * i) + j;
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
            T[,] arr = new T[rows, cols];
            int count = enumables.Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = (cols * i) + j;
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
        /// 转换为指定进制的字符串
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="fomart">要对每个字节转换的格式</param>
        /// <returns></returns>
        public static string FomartDatas<T>(this IEnumerable<T> arr, string fomart = "{0:X2}", string spaceSymbol = " ")
        {
            string str = string.Empty;
            foreach (T item in arr)
            {
                str += string.Format(fomart, item) + spaceSymbol;
            }

            return str;
        }

        #endregion Public 方法
    }
}