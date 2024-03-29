﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using GeneralTool.General.Enums;
using GeneralTool.General.Models;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// </summary>
    public abstract class SocketMast
    {
        #region Public 字段

        /// <summary>
        /// </summary>
        public bool IsDelMsgAsyn = false;

        /// <summary>
        /// 是否接收所有消息
        /// </summary>
        public bool IsReciverForAll = true;

        #endregion Public 字段

        #region Protected 字段

        /// <summary>
        /// </summary>
        protected bool _isOpen = false;

        /// <summary>
        /// </summary>
        protected Dictionary<string, SocketLinkObject> _linkPool = new Dictionary<string, SocketLinkObject>();

        /// <summary>
        /// </summary>
        protected Socket _socket = null;

        #endregion Protected 字段

        #region Private 字段

        // Token: 0x0400001B RID: 27
        private int _dataPageLength = 4096;

        // Token: 0x04000018 RID: 24
        private string _ip = string.Empty;

        // Token: 0x04000017 RID: 23
        private bool _isAutoSize = true;

        // Token: 0x0400001A RID: 26
        private bool _isInit = false;

        // Token: 0x04000019 RID: 25
        private int _port = 7878;

        // Token: 0x0400001E RID: 30
        private RecEncodingType encodingType = RecEncodingType.UTF8;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        public SocketMast()
        {
        }

        #endregion Public 构造函数

        #region Public 委托

        /// <summary>
        /// </summary>
        /// <param name="socket">
        /// </param>
        public delegate void ClsedLink(Socket socket);

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        public delegate void RecDataFrom(RecDataObject obj);

        #endregion Public 委托

        #region Public 事件

        /// <summary>
        /// 连接关闭事件
        /// </summary>
        public event ClsedLink CloseLinkEvent = null;

        /// <summary>
        /// 消息返回事件
        /// </summary>
        public event RecDataFrom RecDataEvent = null;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int DataPageLength
        {
            get
            {
                return this._dataPageLength;
            }
            set
            {
                bool flag = this._socket != null;
                if (flag)
                {
                    this._socket.ReceiveBufferSize = value;
                    this._dataPageLength = value;
                }
            }
        }

        /// <summary>
        /// 编码格式
        /// </summary>
        public RecEncodingType EncodingType
        {
            get
            {
                return this.encodingType;
            }
            set
            {
                this.encodingType = value;
            }
        }

        /// <summary>
        /// Ip
        /// </summary>
        public string Ip
        {
            get
            {
                return this._ip;
            }
            set
            {
                this._ip = value;
            }
        }

        /// <summary>
        /// 是否自动计算大小
        /// </summary>
        public bool IsAutoSize
        {
            get
            {
                return this._isAutoSize;
            }
            set
            {
                this._isAutoSize = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsCheckLink { get; set; } = false;

        /// <summary>
        /// </summary>
        public bool IsInit
        {
            get
            {
                return this._isInit;
            }
        }

        /// <summary>
        /// 是否已开启
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return this._isOpen;
            }
        }

        /// <summary>
        /// 连接池
        /// </summary>
        public Dictionary<string, SocketLinkObject> LinkPool
        {
            get
            {
                return this._linkPool;
            }
        }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get
            {
                return this._port;
            }
            set
            {
                this._port = value;
            }
        }

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public int ReciveBuffSize { get; set; } = -1;

        /// <summary>
        /// </summary>
        public Socket Socket
        {
            get
            {
                return this._socket;
            }
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// </summary>
        /// <param name="broadcastMsg">
        /// </param>
        /// <returns>
        /// </returns>
        public static Thread Broadcast(string broadcastMsg)
        {
            Thread thread = new Thread(delegate (object _)
            {
                Socket socket = null;
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Broadcast, 9095);
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                    byte[] bytes = Encoding.UTF8.GetBytes(_.ToString());
                    for (; ; )
                    {
                        socket.SendTo(bytes, remoteEP);
                        Thread.Sleep(1500);
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    socket.Dispose();
                    socket = null;
                }
            });
            thread.Start(broadcastMsg);
            return thread;
        }

        /// <summary>
        /// </summary>
        /// <param name="pos">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetLocalIP(int pos = 1)
        {
            string result;
            try
            {
                string hostName = Dns.GetHostName();
                //IPHostEntry hostByName = Dns.GetHostByName(hostName);
                IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());//hostByName.AddressList;
                                                                                  //string[] array = new string[addressList.Length];
                                                                                  //for (int i = 0; i < addressList.Length; i++)
                                                                                  //{
                                                                                  //    array[i] = addressList[i].ToString();
                                                                                  //}

                result = addressList[pos].ToString();// Dns.Resolve(Dns.GetHostName()).AddressList[pos].ToString();
            }
            catch (Exception)
            {
                result = "";
            }
            return result;
        }

        /// <summary>
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// </summary>
        /// <param name="sendData">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] GetForntBytes(byte[] sendData)
        {
            byte[] array = new byte[this.DataPageLength];
            Array.Copy(sendData, array, sendData.Length);
            return array;
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] GetSendBytes(string msg)
        {
            byte[] bytes;
            switch (this.EncodingType)
            {
                case RecEncodingType.UTF8:
                    bytes = Encoding.UTF8.GetBytes(msg);
                    break;

                case RecEncodingType.ASCII:
                    bytes = Encoding.ASCII.GetBytes(msg);
                    break;

                case RecEncodingType.UTF7:
                    bytes = Encoding.UTF7.GetBytes(msg);
                    break;

                case RecEncodingType.Unicode:
                    bytes = Encoding.Unicode.GetBytes(msg);
                    break;

                case RecEncodingType.UTF32:
                    bytes = Encoding.UTF32.GetBytes(msg);
                    break;

                default:
                    bytes = Encoding.Default.GetBytes(msg);
                    break;
            }
            bool flag = !this._isAutoSize;
            byte[] result;
            if (flag)
            {
                result = this.GetForntBytes(bytes);
            }
            else
            {
                result = bytes;
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="Ip">
        /// </param>
        /// <param name="Port">
        /// </param>
        /// <returns>
        /// </returns>
        public bool InitSocket(IP Ip, int Port)
        {
            bool flag = Port < 1000 || Port > 9999 || !this.IsFrontIp(Ip);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                this.Ip = Ip;
                this.Port = Port;
                this._isInit = true;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="Ip">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsFrontIp(string Ip)
        {
            string[] array = Ip.Split(new char[]
            {
                '.'
            });
            bool flag = array.Length != 4;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                foreach (string s in array)
                {
                    int num = int.Parse(s);
                    bool flag2 = num < 0 || num > 255;
                    if (flag2)
                    {
                        return false;
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="sock">
        /// </param>
        public void OnCloseLinkEvent(Socket sock)
        {
            this.CloseLinkEvent?.Invoke(sock);
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        public void OnRecDataEvent(RecDataObject obj)
        {
            bool flag = !this.IsDelMsgAsyn;
            if (flag)
            {
                lock (this)
                {
                    this.RecDataEvent?.Invoke(obj);
                }
            }
            else
            {
                this.RecDataEvent?.Invoke(obj);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public abstract void Send(string msg);

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        public abstract void Send(byte[] msg);

        #endregion Public 方法

        #region Internal 方法

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <param name="client">
        /// </param>
        internal virtual void DealMsg(byte[] msg, Socket client)
        {
            string @string;
            switch (this.EncodingType)
            {
                case RecEncodingType.UTF8:
                    @string = Encoding.UTF8.GetString(msg);
                    break;

                case RecEncodingType.ASCII:
                    @string = Encoding.ASCII.GetString(msg);
                    break;

                case RecEncodingType.UTF7:
                    @string = Encoding.UTF7.GetString(msg);
                    break;

                case RecEncodingType.Unicode:
                    @string = Encoding.Unicode.GetString(msg);
                    break;

                case RecEncodingType.UTF32:
                    @string = Encoding.UTF32.GetString(msg);
                    break;

                default:
                    @string = Encoding.Default.GetString(msg);
                    break;
            }
            @string.Trim(new char[1]);
            this.SendDataTo(new RecDataObject(client, msg, @string));
        }

        #endregion Internal 方法

        #region Protected Internal 方法

        /// <summary>
        /// </summary>
        /// <param name="recObj">
        /// </param>
        protected internal void SendDataTo(RecDataObject recObj)
        {
            new Thread(new ParameterizedThreadStart(this.DataTo))
            {
                IsBackground = true
            }.Start(recObj);
        }

        #endregion Protected Internal 方法

        #region Protected 方法

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        protected void DataTo(object obj)
        {
            this.OnRecDataEvent(obj as RecDataObject);
        }

        #endregion Protected 方法
    }
}