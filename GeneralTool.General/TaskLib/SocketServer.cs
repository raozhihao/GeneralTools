using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 
    /// </summary>
    internal class SocketServer : SocketMast, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        public delegate void RecLink(Socket socket);
        /// <summary>
        /// 
        /// </summary>
        public event RecLink RecLinkSocket = null;

        /// <summary>
        /// 
        /// </summary>
        public int ClientNum
        {
            get
            {
                return this._clientNum;
            }
            set
            {
                this._clientNum = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Start()
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
                this._listenningThread = new Thread(new ThreadStart(this.ListenningLink));
                this._listenningThread.IsBackground = true;
                this._listenningThread.Start();
                this._isOpen = true;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected void AddClientSocket(Socket client)
        {
            bool flag = !this._linkPool.Keys.Contains(client.RemoteEndPoint.ToString()) && base.IsOpen;
            if (flag)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(this.CommunicationToClient));
                thread.IsBackground = true;
                thread.Start(client);
                this._linkPool.Add(client.RemoteEndPoint.ToString(), new SocketLinkObject(client, thread));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
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
                byte[] array = new byte[base.DataPageLength];
                for (; ; )
                {
                    list.Clear();
                    bool isReciverForAll = this.IsReciverForAll;
                    if (isReciverForAll)
                    {
                        int num = socket.Receive(array);
                        while (socket.Available > 0 || num > 0)
                        {
                            bool flag2 = num == 0;
                            if (flag2)
                            {
                                num = socket.Receive(array);
                            }
                            bool flag3 = num == array.Length;
                            if (flag3)
                            {
                                list.AddRange(array);
                            }
                            else
                            {
                                for (int i = 0; i < num; i++)
                                {
                                    list.Add(array[i]);
                                }
                            }
                            num = 0;
                        }
                    }
                    else
                    {
                        socket.Receive(array, base.DataPageLength, SocketFlags.None);
                        list.AddRange(array);
                    }
                    bool isCheckLink = base.IsCheckLink;
                    if (isCheckLink)
                    {
                        bool flag4 = socket.Poll(10, SelectMode.SelectRead);
                        if (flag4)
                        {
                            break;
                        }
                    }
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

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="msg"></param>
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
        /// 
        /// </summary>
        /// <param name="msg"></param>
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
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        public void Send(string msg, Socket client)
        {
            byte[] sendBytes = base.GetSendBytes(msg);
            try
            {
                client.Send(sendBytes, SocketFlags.None);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }


        private Thread _listenningThread = null;


        private int _clientNum = 5;


        private bool disposedValue = false;
    }
}
