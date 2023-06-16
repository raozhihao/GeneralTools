using System.Text;
using System.Text.RegularExpressions;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 字符串类型扩展
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string Fomart(this string str, params object[] parameters) => string.Format(str, parameters);

        /// <summary>
        /// 清除所有指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="removeCharArr">要清除的符号列表</param>
        /// <returns></returns>
        public static string TrimAll(this string str, params char[] removeCharArr)
        {
            foreach (char item in removeCharArr)
            {
                str = str.Replace(item + "", "");
            }
            return str;
        }

        /// <summary>
        /// 连接指定的 System.String 数组的元素。
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <param name="obj">
        /// 要连接的第一个对象。
        /// </param>
        /// <returns>
        /// </returns>
        public static string Concat(this string str, object obj)
        {
            return str + obj;
        }

        /// <summary>
        /// 连接指定的 System.String 数组的元素。
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <param name="objs">
        /// 实例的数组
        /// </param>
        /// <returns>
        /// </returns>

        public static string Concat(this string str, params object[] objs)
        {
            if (objs == null || objs.Length == 0)
            {
                return str;
            }

            foreach (object obj in objs)
            {
                str += obj;
            }
            return str;
        }

        /// <summary>
        /// 连接指定的 System.String 数组的元素。
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <param name="strs">
        /// 字符串实例的数组
        /// </param>
        /// <returns>
        /// </returns>
        public static string Concat(this string str, params string[] strs)
        {
            if (strs == null || strs.Length == 0)
            {
                return str;
            }
            string concatStr = string.Concat(strs);
            return string.Concat(str, concatStr);
        }

        /// <summary>
        /// 获取字符串的字节数组
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 以UTF-8格式获取字符串的字节数组
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] ToBytes(this string str)
        {
            return str.ToBytes(Encoding.UTF8);
        }

        /// <summary>
        /// 将字符串清洗后只剩中文
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToChinese(this string text)
        {
            string c = "";
            foreach (char ch in text)
            {
                if (Regex.IsMatch(ch.ToString(), @"[\u4e00-\u9fbb]+"))
                    c += ch.ToString();
            }
            return c;
        }

    }
}
