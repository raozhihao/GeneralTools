using System.Text;

namespace GeneralTool.General.ValueTypeExtensions
{
    /// <summary>
    /// 字符串相关扩展类
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 连接指定的 System.String 数组的元素。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="obj">要连接的第一个对象。</param>
        /// <returns></returns>
        public static string Concat(this string str, object obj)
        {
            return str + obj;
        }

        /// <summary>
        /// 连接指定的 System.String 数组的元素。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="objs">实例的数组</param>
        /// <returns></returns>

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
        /// <param name="str"></param>
        /// <param name="strs">字符串实例的数组</param>
        /// <returns></returns>
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
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 以UTF-8格式获取字符串的字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            return str.ToBytes(Encoding.UTF8);
        }
    }
}
