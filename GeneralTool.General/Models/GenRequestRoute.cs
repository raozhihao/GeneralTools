using System;
using System.Net.Sockets;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class GenRequestRoute : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public RequestAddressItem AddressItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ServerRequest ServerRequest { get; set; }

        /// <summary>
        /// 获取客户端Socket
        /// </summary>
        public Socket Client { get; internal set; }

        /// <summary>
        /// 将字节数组返回发送给客户端
        /// </summary>
        public Action<byte[], Socket> SendToClinet { get; internal set; }
    }
}
