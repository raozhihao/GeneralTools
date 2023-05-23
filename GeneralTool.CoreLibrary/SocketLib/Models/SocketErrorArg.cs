using System;
using System.Net.Sockets;

namespace GeneralTool.CoreLibrary.SocketLib.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SocketErrorArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ex"></param>
        public SocketErrorArg(Socket client, Exception ex)
        {
            this.Client = client;
            this.Exception = ex;
        }

        /// <summary>
        /// 
        /// </summary>
        public Socket Client { get; }
        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; }
    }
}
