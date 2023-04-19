using System;

namespace GeneralTool.General.Extensions
{
    /// <summary>
    /// 整数扩展类
    /// </summary>
    public static class Int32Extensions
    {
        #region Public 方法

        /// <summary>
        /// 拆分一个整形到2个字节
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] SplitIntToByts(this int value)
        {
            return new byte[] { (byte)(value >> 8), (byte)(((UInt16)value) << 8 >> 8) };
        }

        #endregion Public 方法
    }
}