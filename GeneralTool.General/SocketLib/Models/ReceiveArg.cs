﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace GeneralTool.General.SocketLib.Models
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
            this.PackBuffer = packBuffer;
            this.Client = client;
        }

        /// <summary>
        /// 接收到的数据包
        /// </summary>
        public IEnumerable<byte> PackBuffer { get; }
        /// <summary>
        /// 工作的socket
        /// </summary>
        public Socket Client { get; }
    }
}
