using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// 路径扩展类
    /// </summary>
    public static class PathExtensions
    {
        #region Public 方法

        /// <summary>
        /// 将当前路径字符串与字符串路径数组组合成一个新的路径字符串
        /// </summary>
        /// <param name="path">
        /// 当前字符串路径
        /// </param>
        /// <param name="paths">
        /// 要拼接的字符串路径数组
        /// </param>
        /// <returns>
        /// 返回拼接好的字符串路径
        /// </returns>
        public static string CombineExtension(this string path, params string[] paths)
        {
            char[] splitChar = new char[] { '\\', '/' };
            string joinStr = System.IO.Path.DirectorySeparatorChar.ToString();
            if (paths == null)
            {
                return path + "";
            }
            IEnumerable<string[]> pathTmp = paths.Select(s => s.Split(splitChar, StringSplitOptions.RemoveEmptyEntries));
            IEnumerable<string> pathsTmp = pathTmp.Select(s => string.Join(joinStr, s));
            string tmp = string.Join(joinStr, pathsTmp);
            if (string.IsNullOrWhiteSpace(path))
            {
                return tmp;
            }

            string[] startTmp = path.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);
            string startStr = string.Join(joinStr, startTmp);
            return string.Join(joinStr, startStr, tmp);
        }

        #endregion Public 方法
    }
}