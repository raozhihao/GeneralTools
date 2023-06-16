using System;
using System.Net;
using System.Net.Sockets;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;

namespace GeneralTool.CoreLibrary.SocketLib
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SocketServer<T> : BaseSocket<T> where T : ReceiveState, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public override Socket Socket { get; protected set; }

        /// <inheritdoc/>
        public override bool IsConnected { get; protected set; }

        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event EventHandler<SocketArg> ClientConnctedEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public SocketServer(ILog log = null) : base(log)
        {

        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public override void Startup(IPAddress address, int port)
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                SendBufferSize = 1024 * 1024 * 10,
                ReceiveBufferSize = 1024 * 1024 * 10
            };
            Socket.Bind(new IPEndPoint(address, port));
            Socket.Listen(10);
            Console.WriteLine("开始异步接收连接");
            _ = Socket.BeginAccept(AcceptCallback, Socket);
            IsConnected = true;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            //接收到连接
            Socket serverSocket = ar.AsyncState as Socket;
            Socket client = null;
            try
            {
                client = serverSocket.EndAccept(ar);
            }
            catch (Exception ex)
            {
                CloseClient(client, ex);
                return;
            }

            _ = serverSocket.BeginAccept(AcceptCallback, serverSocket);

            ClientConnctedEvent?.Invoke(this, new SocketArg(client));

            BeginReceive(client);
        }
    }
}
