using System;
using System.Collections.Generic;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// QueryString帮助类
    /// </summary>
    public static class QueryExtentions
    {
        #region Public 方法

        /// <summary>
        /// 获取Url中的QueryString字符串
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetQueryString(this string url)
        {
            return url.Substring(url.IndexOf('?') + 1); ;
        }

        /// <summary>
        /// 将query的字典转为QueryString
        /// </summary>
        /// <param name="queryDictionary">
        /// </param>
        /// <returns>
        /// </returns>
        public static string ParseDictionaryToQueryString(this Dictionary<string, object> queryDictionary)
        {
            if (queryDictionary == null || queryDictionary.Count == 0)
            {
                return "";
            }

            List<string> list = new List<string>();
            foreach (var item in queryDictionary)
            {
                list.Add($"{item.Key}={item.Value.SerializeToJsonString()}");
            }

            return string.Join("&", list);
        }

        /// <summary>
        /// 将QueryString字符串转为字典
        /// </summary>
        /// <param name="queryStrings">
        /// 字符串
        /// </param>
        /// <returns>
        /// </returns>
        public static Dictionary<string, string> ParseQueryToDictionary(this string queryStrings)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var split = queryStrings.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in split)
            {
                var tmpArr = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                var key = tmpArr[0];
                var val = tmpArr[1];
                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, val);
                }
            }

            return dic;
        }

        /// <summary>
        /// 将url地址中的QueryString提取出来
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <returns>
        /// </returns>
        public static Dictionary<string, string> ParseUrlToQueryDictionary(this string url)
        {
            string queryString = url.GetQueryString();
            return queryString.ParseQueryToDictionary();
        }

        #endregion Public 方法
    }
}