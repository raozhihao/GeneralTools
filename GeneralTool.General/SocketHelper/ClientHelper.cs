using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Models;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 客户端帮助类
    /// </summary>
    public class ClientHelper
    {
        private readonly System.Net.Sockets.Socket clientSocket;
        private readonly SerializeHelpers serialize;
        private readonly string host;
        private readonly int port;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public ClientHelper(string host = "127.0.0.1", int port = 55155)
        {
            this.host = host;
            this.port = port;

            serialize = new SerializeHelpers();
            clientSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
            }
            catch (Exception ex)
            {
                throw new SocketConnectException($"服务端连接失败 {host}:{port}", ex);
            }
        }


        /// <summary>
        /// 发送命令并获取返回命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public ResponseCommand SendCommand(RequestCommand cmd)
        {
            if (!clientSocket.Connected)
            {
                Start();
            }

            byte[] bytes;
            try
            {
                bytes = serialize.Serialize(cmd);
            }
            catch (Exception ex)
            {

                throw new SerializeException("序列化出错", ex);
            }

            clientSocket.Send(bytes);
            ResponseCommand result = GetResponseCommand();
            return result;
        }

        private ResponseCommand GetResponseCommand()
        {
            byte[] buffer = new byte[1024];

            List<byte> list = new List<byte>();

            while (true)
            {

                int len = clientSocket.Receive(buffer);
                list.AddRange(buffer.Take(len));
                if (len < buffer.Length)
                {
                    break;
                }
            }

            if (list.Count > 0)
            {
                byte[] bytes = list.ToArray();

                if (bytes.Length == 0)
                {
                    return GetResponseCommand();
                }
                try
                {
                    ResponseCommand reCmd = serialize.Desrialize<ResponseCommand>(bytes);
                    return reCmd;
                }
                catch (Exception ex)
                {

                    throw new DesrializeException("反序列化出错", ex);
                }

            }
            else
            {
                return new ResponseCommand() { Success = true };
            }

        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            Console.WriteLine("调用客户端Close");
            if (clientSocket == null)
            {
                return;
            }
            clientSocket.Close();
            clientSocket.Dispose();
        }
    }
}
