using System;
using System.Net;
using System.Net.Sockets;

using GeneralTool.General.Interfaces;
using GeneralTool.General.SocketLib.Models;


namespace GeneralTool.General.SocketLib
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
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Socket.Bind(new IPEndPoint(address, port));
            this.Socket.Listen(10);
            Console.WriteLine("开始异步接收连接");
            this.Socket.BeginAccept(AcceptCallback, this.Socket);
            this.IsConnected = true;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            //接收到连接
            var serverSocket = ar.AsyncState as Socket;
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

            serverSocket.BeginAccept(AcceptCallback, serverSocket);

            this.ClientConnctedEvent?.Invoke(this, new SocketArg(client));

            this.BeginReceive(client);
        }
    }
}
