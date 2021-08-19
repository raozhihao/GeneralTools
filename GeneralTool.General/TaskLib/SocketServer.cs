using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using GeneralTool.General.SocketHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// </summary>
    public class SocketServer : SocketMast, IDisposable
    {
        #region Private 字段

        private Thread _listenningThread = null;
        private bool disposedValue = false;
        private readonly ILog log;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="log">
        /// </param>
        public SocketServer(ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            this.log = log;
        }

        #endregion Public 构造函数

        #region Public 委托

        /// <summary>
        /// </summary>
        /// <param name="socket">
        /// </param>
        public delegate void RecLink(Socket socket);

        #endregion Public 委托

        #region Public 事件

        /// <summary>
        /// </summary>
        public event RecLink RecLinkSocket = null;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// </summary>
        public int ClientNum { get; set; } = 5;

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// </summary>
        public override void Close()
        {
            try
            {
                bool isOpen = base.IsOpen;
                if (isOpen)
                {
                    this._socket.Close();
                    this._listenningThread.Abort();
                    foreach (KeyValuePair<string, SocketLinkObject> keyValuePair in this._linkPool)
                    {
                        keyValuePair.Value.LinkSocket.Close();
                        bool isAlive = keyValuePair.Value.LinkThread.IsAlive;
                        if (isAlive)
                        {
                            keyValuePair.Value.LinkThread.Abort();
                        }
                    }
                    this._linkPool.Clear();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                this._isOpen = false;
            }
        }

        /// <summary>
        /// </summary>
        public override void Connect()
        {
            bool flag = !base.IsInit;
            if (flag)
            {
                throw new Exception("Socket 没有被初始化或者初始化设置失败");
            }
            try
            {
                IPAddress ipaddress = IPAddress.Parse(base.Ip);
                this._socket = new Socket(ipaddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEP = new IPEndPoint(ipaddress, base.Port);
                this._socket.Bind(localEP);
                bool isOpen = base.IsOpen;
                if (isOpen)
                {
                    this.Close();
                }
                this._socket.Listen(this.ClientNum);
                this._socket.SetSocketKeepAlive();
                this._listenningThread = new Thread(new ThreadStart(this.ListenningLink))
                {
                    IsBackground = true
                };
                this._listenningThread.Start();
                this._isOpen = true;
            }
            catch (Exception ex)
            {
                log.Fail($"开启sokcet服务失败:{ex}");
                throw ex;
            }
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public override void Send(string msg)
        {
            byte[] sendBytes = base.GetSendBytes(msg);
            string text = "";
            foreach (KeyValuePair<string, SocketLinkObject> keyValuePair in this._linkPool)
            {
                bool flag = keyValuePair.Value.LinkSocket != null && !keyValuePair.Value.LinkSocket.Poll(10, SelectMode.SelectRead);
                if (flag)
                {
                    keyValuePair.Value.LinkSocket.Send(sendBytes, SocketFlags.None);
                }
                else
                {
                    text = keyValuePair.Key;
                }
            }
            bool flag2 = text != "";
            if (flag2)
            {
                this._linkPool.Remove(text);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public override void Send(byte[] msg)
        {
            bool flag = !base.IsAutoSize;
            if (flag)
            {
                msg = base.GetForntBytes(msg);
            }
            string text = "";
            foreach (KeyValuePair<string, SocketLinkObject> keyValuePair in this._linkPool)
            {
                bool flag2 = keyValuePair.Value.LinkSocket != null && !keyValuePair.Value.LinkSocket.Poll(10, SelectMode.SelectRead);
                if (flag2)
                {
                    keyValuePair.Value.LinkSocket.Send(msg, SocketFlags.None);
                }
                else
                {
                    text = keyValuePair.Key;
                }
            }
            bool flag3 = text != "";
            if (flag3)
            {
                this._linkPool.Remove(text);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <param name="client">
        /// </param>
        public void Send(string msg, Socket client)
        {
            byte[] sendBytes = base.GetSendBytes(msg);
            try
            {
                client.Send(sendBytes, SocketFlags.None);
            }
            catch (Exception ex)
            {
                log.Error($"向客户端 [{client.RemoteEndPoint}] 发送消息失败:{ex.Message}");
            }
        }

        #endregion Public 方法

        #region Protected 方法

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected void AddClientSocket(Socket client)
        {
            bool flag = !this._linkPool.Keys.Contains(client.RemoteEndPoint.ToString()) && base.IsOpen;
            if (flag)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(this.CommunicationToClient))
                {
                    IsBackground = true
                };
                thread.Start(client);
                this._linkPool.Add(client.RemoteEndPoint.ToString(), new SocketLinkObject(client, thread));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="client">
        /// </param>
        protected void DelClientSocket(Socket client)
        {
            try
            {
                string text = client.RemoteEndPoint.ToString();
                bool flag = this._linkPool.Keys.Contains(text);
                if (flag)
                {
                    this._linkPool.Remove(text);
                    base.OnCloseLinkEvent(client);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            bool flag = !this.disposedValue;
            if (flag)
            {
                if (disposing)
                {
                }
                this.disposedValue = true;
            }
        }

        #endregion Protected 方法

        #region Private 方法

        private void CommunicationToClient(object client)
        {
            Socket socket = client as Socket;

            try
            {
                bool flag = base.ReciveBuffSize != -1;
                if (flag)
                {
                    socket.ReceiveBufferSize = base.ReciveBuffSize;
                }
                List<byte> list = new List<byte>();

                for (; ; )
                {
                    if (!socket.IsClientConnected())
                    {
                        break;
                    }
                    list.Clear();
                    bool isReciverForAll = this.IsReciverForAll;
                    if (isReciverForAll)
                    {
                        while (true)
                        {
                            byte[] array = new byte[base.DataPageLength];
                            var num = socket.Receive(array);
                            list.AddRange(array.Take(num));
                            Array.Clear(array, 0, array.Length);

                            if (num <= array.Length && socket.Available == 0)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        byte[] array = new byte[base.DataPageLength];
                        socket.Receive(array, base.DataPageLength, SocketFlags.None);
                        list.AddRange(array);
                    }
                    //bool isCheckLink = base.IsCheckLink;
                    //if (isCheckLink)
                    //{
                    //    bool flag4 = socket.Poll(10, SelectMode.SelectRead);
                    //    if (flag4)
                    //    {
                    //        break;
                    //    }
                    //}
                    this.DealMsg(list.ToArray(), socket);
                }
                throw new Exception("连接已被断开!");
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception)
            {
                this.DelClientSocket(socket);
            }
        }

        private void ListenningLink()
        {
            try
            {
                for (; ; )
                {
                    Socket socket = this._socket.Accept();
                    bool flag = this.RecLinkSocket != null;
                    if (flag)
                    {
                        this.RecLinkSocket(socket);
                    }
                    this.AddClientSocket(socket);
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                log.Error($"监听客户端连接出现错误:{ex.Message}");
            }
        }

        #endregion Private 方法
    }
}