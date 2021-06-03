using System;

namespace GeneralTool.General.ValueTypeExtensions
{
    /// <summary>
    /// 整数扩展类
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// 拆分一个整形到2个字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] splitIntToByts(this int value)
        {
            return new byte[] { (byte)(value >> 8), (byte)(((UInt16)value) << 8 >> 8) };
        }
    }
}
