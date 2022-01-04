using System;
using System.Collections.Generic;
using System.Net.Sockets;

using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.SocketLib;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 客户端基础实现
    /// </summary>
    public class ClientSocketBase
    {
        #region Private 字段

        private readonly Socket clientSocket;
        private readonly string host;
        private readonly int port;
        private readonly SocketCommon common = new SocketCommon();

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">
        /// </param>
        /// <param name="port">
        /// </param>
        public ClientSocketBase(string host = "127.0.0.1", int port = 55155)
        {
            this.host = host;
            this.port = port;

            clientSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErroMsg { get; private set; }

        /// <summary>
        /// 客户端是否处于连接中
        /// </summary>
        public bool IsConnected
        {
            get; private set;
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            System.Diagnostics.Trace.WriteLine("调用客户端Close");
            if (clientSocket == null)
            {
                return;
            }
            clientSocket.Close();
            clientSocket.Dispose();
            this.IsConnected = false;
        }

        /// <summary>
        /// 发送命令并获取返回命令
        /// </summary>
        /// <param name="buffer">
        /// </param>
        /// <returns>
        /// </returns>
        public List<byte> Send(byte[] buffer)
        {
            this.ErroMsg = "";
            var len = common.Send(this.clientSocket, buffer);
            if (len != buffer.Length)
            {
                throw common.ErroException;
            }

            var headSize = common.ReadHeadSize(this.clientSocket);
            if (headSize <= 0)
            {
                throw common.ErroException;
            }
            var package = common.Read(this.clientSocket, headSize);

            return package.Buffer;
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        public void Start()
        {
            try
            {
                clientSocket.Connect(host, port);
                clientSocket.Blocking = true;
                clientSocket.SetSocketKeepAlive();
                this.IsConnected = true;
            }
            catch (Exception ex)
            {
                throw new SocketConnectException($"服务端连接失败 {host}:{port}", ex);
            }
        }

        #endregion Public 方法


    }
}