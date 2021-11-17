using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.ExceptionHelper;
using System.Diagnostics;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// Socket服务端
    /// </summary>
    public class ServerSocketBase
    {
        #region Private 字段

        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, Socket> clients;
        private bool disposedValue;
        private Socket serverSocket;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">
        /// 路径
        /// </param>
        /// <param name="port">
        /// 端口
        /// </param>
        public ServerSocketBase(string host = "127.0.0.1", int port = 55155)
        {
            clients = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Net.Sockets.Socket>();
            serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            serverSocket.Blocking = true;
        }

        #endregion Public 构造函数

        #region Private 析构函数

        /// <summary>
        /// </summary>
        ~ServerSocketBase()
        {
            Dispose(disposing: false);
        }

        #endregion Private 析构函数

        #region Public 事件

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
        public event EventHandler<SocketPackage> RecevieEvent;

        /// <summary>
        /// 错误信息接收事件
        /// </summary>
        public event EventHandler<SocketClientErroArgs> ErroEvent;

        #endregion Public 事件

        #region Public 方法

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
        /// 开启侦听
        /// </summary>
        /// <param name="backListenCount">
        /// 挂起连接队列的最大长度。
        /// </param>
        /// <returns>
        /// </returns>
        public bool Start(int backListenCount = 10)
        {
            serverSocket.Listen(backListenCount);
            serverSocket.BeginAccept(AcceptMethod, serverSocket);
            serverSocket.SetSocketKeepAlive();
            return true;
        }

        #endregion Public 方法

        #region Protected 方法

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing">
        /// </param>
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

        #endregion Protected 方法

        #region Private 方法

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


            var common = new SocketCommon();
            List<byte> list = new List<byte>();
            while (true)
            {
                if (!clientSocket.IsClientConnected())
                {
                    CloseClient(clientSocket);

                    break;
                }


                var watch = new Stopwatch();
                watch.Start();
                //先读取一个头部
                var package = new SocketPackage();
                try
                {
                    Trace.WriteLine($"{DateTime.Now} : 准备接收 - {key}");

                    var len = common.ReadHeadSize(clientSocket);
                    if (len <= 0)
                    {
                        throw new Exception("数据长度不对,收到的数据长度为:" + len + "," + common.ErroException);
                    }

                    Trace.WriteLine($"{DateTime.Now} : 接收到长度数据   Recevier size : {len}");
                    //读取剩下数据
                    package = common.Read(clientSocket, len);
                    Trace.WriteLine($"{DateTime.Now} : 接收完成");
                    if (package.IsError)
                    {
                        this.ErroEvent?.Invoke(this, new SocketClientErroArgs(package.ErroMsg, clientSocket));
                        CloseClient(clientSocket);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //出现问题,关闭连接
                    Trace.WriteLine(ex.GetInnerExceptionMessage());

                    this.ErroEvent?.Invoke(this, new SocketClientErroArgs(package.ErroMsg, clientSocket));
                    CloseClient(clientSocket);
                    break;
                }

                this.RecevieEvent?.Invoke(clientSocket,package);

                Trace.WriteLine($"{DateTime.Now} : 发送事件");
                System.Threading.Thread.Sleep(10);
            }
        }

        private void CloseClient(Socket clientSocket)
        {
            var key = clientSocket.RemoteEndPoint.ToString();
            System.Diagnostics.Trace.WriteLine($"客户端 {key} 已下线");
            clients.TryRemove(key, out _);
            clientSocket.Close();
            clientSocket.Dispose();
            this.ClientDisconnectedEvent?.Invoke(clientSocket);
        }



        #endregion Private 方法
    }
}
