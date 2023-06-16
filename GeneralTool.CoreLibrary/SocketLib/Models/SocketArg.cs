using System;
using System.Net.Sockets;

namespace GeneralTool.CoreLibrary.SocketLib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public SocketArg(Socket client)
        {
            Client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        public Socket Client { get; }
    }
}
