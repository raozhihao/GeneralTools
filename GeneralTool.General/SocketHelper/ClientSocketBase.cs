using GeneralTool.General.ExceptionHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

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
            clientSocket.Send(buffer);
            var list = GetResponseCommand();

            return list;
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

        #region Private 方法

        private List<byte> GetResponseCommand()
        {
            byte[] buffer = new byte[4096];

            List<byte> list = new List<byte>();

            while (this.clientSocket.Available > 0)
            {
                if (!this.clientSocket.IsClientConnected())
                {
                    this.IsConnected = false;
                    list.Clear();
                    this.ErroMsg = "服务器已失去连接";
                    throw new Exception(this.ErroMsg);
                }
                int len = clientSocket.Receive(buffer);
                list.AddRange(buffer.Take(len));
                if (len < buffer.Length)
                {
                    break;
                }
            }

            return list;
        }

        #endregion Private 方法
    }
}