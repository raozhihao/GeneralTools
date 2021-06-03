using System.Net.Sockets;
using System.Threading;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 
    /// </summary>
    public struct SocketLinkObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Socket LinkSocket { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Thread LinkThread { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LinkSocket"></param>
        /// <param name="LinkThread"></param>
        public SocketLinkObject(Socket LinkSocket, Thread LinkThread)
        {
            this.LinkSocket = LinkSocket;
            this.LinkThread = LinkThread;
        }
    }
}
