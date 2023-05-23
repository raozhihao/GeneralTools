using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace GeneralTool.CoreLibrary.Models
{
    /// <summary>
    /// Socket回传消息
    /// </summary>
    public class SocketReceiveArgs : EventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="clinetSocket">
        /// </param>
        /// <param name="buffer">
        /// </param>
        public SocketReceiveArgs(Socket clinetSocket, List<byte> buffer)
        {
            this.ClinetSocket = clinetSocket;
            this.Buffer = buffer;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 保存的数据
        /// </summary>
        public List<byte> Buffer { get; set; }

        /// <summary>
        /// 客户端socket
        /// </summary>
        public Socket ClinetSocket { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 尝试向客户端发送消息
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        public bool TrySend(byte[] buffer)
        {
            try
            {
                this.ClinetSocket.Send(buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Public 方法
    }
}