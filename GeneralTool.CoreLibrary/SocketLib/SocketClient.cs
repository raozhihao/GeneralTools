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
    public class SocketClient<T> : BaseSocket<T> where T : ReceiveState, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public override Socket Socket { get; protected set; }

        /// <inheritdoc/>
        public override bool IsConnected { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public SocketClient(ILog log = null) : base(log)
        {

        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="port"></param>
        public override void Startup(int port)
        {
            Startup(IPAddress.Parse("127.0.0.1"), port);
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
                ReceiveBufferSize = 1024 * 1024 * 10
            };
            Socket.Connect(new IPEndPoint(address, port));

            BeginReceive(Socket);

            IsConnected = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool Send(byte[] buffer)
        {
            return base.Send(buffer, Socket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Send(string msg)
        {
            return base.Send(msg, Socket);
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            Log.Debug($"关闭 {this} 的连接");
            IsConnected = false;
            if (Socket == null) return;
            try
            {
                Socket.Close();
                Socket.Dispose();

                CurrentSockets.Clear();
            }
            catch
            {

            }
        }
    }
}
