using System.Net;

namespace GeneralTool.CoreLibrary.Extensions
{
    /// <summary>
    /// 一些杂项扩展
    /// </summary>
    public static class OtherExtensions
    {
        /// <summary>
        /// 转换IP地址
        /// </summary>
        /// <param name="nCurrentIp"></param>
        /// <returns></returns>
        public static IPAddress ParseToIp(this uint nCurrentIp)
        {
            uint i1 = ((nCurrentIp) & 0xff000000) >> 24;
            uint i2 = ((nCurrentIp) & 0x00ff0000) >> 16;
            uint i3 = ((nCurrentIp) & 0x0000ff00) >> 8;
            uint i4 = (nCurrentIp) & 0x000000ff;
            return $"{i1}.{i2}.{i3}.{i4}".ParseToIPAddress();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress ParseToIPAddress(this string ip)
            => IPAddress.Parse(ip);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPAddress"></param>
        /// <returns></returns>
        public static string ParseToIPAddress(IPAddress iPAddress)
            => iPAddress.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nCurrentIp"></param>
        /// <returns></returns>
        public static string ParseToIPString(this uint nCurrentIp)
            => nCurrentIp.ParseToIp().ToString();

    }
}
