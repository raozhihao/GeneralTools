using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// Socket基础库
    /// </summary>
    public class ServerSocketBase
    {
        private Socket serverSocket;
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, Socket> clients;

        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event Action<Socket> ClientConnetedEvent;
        /// <summary>
        /// 客户端断开事件
        /// </summary>
        public event Action<Socket> ClientDisconnectedEvent;
        /// <summary>
        /// 客户端接收消息事件
        /// </summary>
        public event Action<SocketReceiveArgs> RecevieEvent;

        private bool disposedValue;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">路径</param>
        /// <param name="port">端口</param>
        public ServerSocketBase(string host = "127.0.0.1", int port = 55155)
        {
            clients = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Net.Sockets.Socket>();
            serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            serverSocket.Blocking = true;
        }


        /// <summary>
        /// 开启侦听
        /// </summary>
        /// <param name="backListenCount"> 挂起连接队列的最大长度。</param>
        /// <returns></returns>
        public bool Start(int backListenCount = 10)
        {
            serverSocket.Listen(backListenCount);
            serverSocket.BeginAccept(AcceptMethod, serverSocket);
            serverSocket.SetSocketKeepAlive();
            return true;
        }


        private void AcceptMethod(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as System.Net.Sockets.Socket;

            System.Net.Sockets.Socket clientSocket = null;
            try
            {
                clientSocket = socket.EndAccept(ar);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"客户端连接发生异常 : {ex.Message}");
                return;
            }

            this.ClientConnetedEvent?.Invoke(clientSocket);

            socket.BeginAccept(AcceptMethod, socket);

            string key = clientSocket.RemoteEndPoint.ToString();

            if (!clients.ContainsKey(key))
            {
                clients.TryAdd(key, clientSocket);
                System.Diagnostics.Trace.WriteLine($"新的客户端 {key} 已连接");
            }
            else
            {
                clients[key] = clientSocket;
                System.Diagnostics.Trace.WriteLine($"客户端 {key} 已重新连接");
            }


            List<byte> list = new List<byte>();
            while (true)
            {
                if (!clientSocket.IsClientConnected())
                {
                    System.Diagnostics.Trace.WriteLine($"客户端 {key} 已下线");
                    clients.TryRemove(key, out _);
                    clientSocket.Close();
                    clientSocket.Dispose();
                    this.ClientDisconnectedEvent?.Invoke(clientSocket);
                    break;
                }

                byte[] buffer = new byte[4096];
                list.Clear();
                if (!clientSocket.Connected)
                {
                    break;
                }
                while (clientSocket.Available > 0)
                {
                    int len = clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    list.AddRange(buffer.Take(len));
                    if (len < buffer.Length)
                    {
                        break;
                    }
                }

                if (list.Count > 0)
                {
                    this.RecevieEvent?.Invoke(new SocketReceiveArgs(clientSocket, list));
                }
                System.Threading.Thread.Sleep(10);
            }

        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    try
                    {
                        foreach (KeyValuePair<string, Socket> item in clients)
                        {
                            item.Value.Close();
                            item.Value.Dispose();
                        }

                        clients.Clear();
                        serverSocket.Close();
                        serverSocket.Dispose();

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }
                serverSocket = null;
                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        ~ServerSocketBase()
        {
            Dispose(disposing: false);
        }

    }
}
