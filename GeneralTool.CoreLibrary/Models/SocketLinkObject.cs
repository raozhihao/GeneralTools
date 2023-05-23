using System;
using System.Net.Sockets;
using System.Threading;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// </summary>
    [Serializable]
    public struct SocketLinkObject
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="LinkSocket">
        /// </param>
        /// <param name="LinkThread">
        /// </param>
        public SocketLinkObject(Socket LinkSocket, Thread LinkThread)
        {
            this.LinkSocket = LinkSocket;
            this.LinkThread = LinkThread;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// </summary>
        public Socket LinkSocket { get; set; }

        /// <summary>
        /// </summary>
        public Thread LinkThread { get; set; }

        #endregion Public 属性
    }
}