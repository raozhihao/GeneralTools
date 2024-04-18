using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace GeneralTool.CoreLibrary.SocketLib.Models
{
    /// <summary>
    /// 接收事件数据包
    /// </summary>
    public class ReceiveArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packBuffer"></param>
        /// <param name="client"></param>
        public ReceiveArg(IEnumerable<byte> packBuffer, Socket client)
        {
            PackBuffer = packBuffer;
            Client = client;
        }

        /// <summary>
        /// 接收到的数据包
        /// </summary>
        public IEnumerable<byte> PackBuffer { get; }
        /// <summary>
        /// 工作的socket
        /// </summary>
        public Socket Client { get; }

        /// <summary>
        /// 指示是否已经处理了
        /// </summary>
        public bool Handled { get; set; }
    }
}
