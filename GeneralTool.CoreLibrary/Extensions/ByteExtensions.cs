using System.Text;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 字节扩展类
    /// </summary>
    public static class ByteExtensions
    {
        #region Public 方法

        /// <summary>
        /// 将字节数组转为字符串
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <param name="encoding">
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToStrings(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将字节数组转为字符串
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <returns>
        /// </returns>
        public static string ToStrings(this byte[] bytes)
        {
            return bytes.ToStrings(Encoding.UTF8);
        }

        #endregion Public 方法
    }
}