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
    }
}
