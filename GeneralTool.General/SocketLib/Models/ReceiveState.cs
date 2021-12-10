using System.Collections.Generic;
using System.Net.Sockets;

namespace GeneralTool.General.SocketLib.Models
{
    /// <summary>
    /// 数据包
    /// </summary>
    public class ReceiveState
    {
        /// <summary>
        /// 数据包缓冲区大小
        /// </summary>
        public int BufferSize { get; set; }
        /// <summary>
        /// 数据包缓冲区
        /// </summary>
        public byte[] Buffer { get; set; }
        /// <summary>
        /// 所有已接收到数据包缓冲区
        /// </summary>
        public List<byte> ListBytes { get; set; } = new List<byte>();
        /// <summary>
        /// 当前的socket
        /// </summary>
        public Socket WorkSocket { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ReceiveState()
        {
            this.BufferSize = 8192;
            this.Buffer = new byte[this.BufferSize];
        }
    }
}
