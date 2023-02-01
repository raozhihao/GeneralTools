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
            this.Startup(IPAddress.Parse("127.0.0.1"), port);
        }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public override void Startup(IPAddress address, int port)
        {
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.Socket.Connect(new IPEndPoint(address, port));

            this.BeginReceive(this.Socket);

            this.IsConnected = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public bool Send(byte[] buffer)
        {
            return base.Send(buffer, this.Socket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Send(string msg)
        {
            return base.Send(msg, this.Socket);
        }


        /// <summary>
        /// 关闭连接
        /// </summary>
        public override void Close()
        {
            this.Log.Debug($"关闭 {this} 的连接");
            this.IsConnected = false;
            if (this.Socket == null) return;
            try
            {
                this.Socket.Close();
                this.Socket.Dispose();

                this.CurrentSockets.Clear();
            }
            catch
            {

            }
        }
    }
}
