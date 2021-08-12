namespace GeneralTool.General.LinqExtensions
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
            foreach (var item in removeCharArr)
            {
                str = str.Replace(item + "", "");
            }
            return str;
        }
    }
}
