using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static GeneralTool.General.TaskLib.SocketServer;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// Socket客户端
    /// </summary>
    public class SocketClient : SocketMast
    {
        /// <summary>
        /// 客户端连接事件
        /// </summary>
        public event RecLink LinkEvent = null;

        /// <summary>
        /// 客户端是否连接中
        /// </summary>
        public bool IsLink
        {
            get
            {
                bool flag = base.Socket.Poll(10, SelectMode.SelectRead);
                return !flag;
            }
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SocketClient(ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            this.Log = log;
        }

        /// <summary>
        /// 关闭服务端
        /// </summary>
        public override void Close()
        {
            try
            {
                bool isOpen = base.IsOpen;
                if (isOpen)
                {
                    this._isOpen = false;
                    this._socket.Close();
                    this._recMsgThread.Abort();
                    base.OnCloseLinkEvent(this._socket);
                    this._recMsgThread = null;
                    this.Log.Debug("已成功关闭客户端连接");
                }
            }
            catch (Exception ex)
            {
                this.Log.Fail("关闭客户端连接出现问题:" + ex.Message);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">发送字符串</param>
        public override void Send(string msg)
        {
            bool flag = !base.IsAutoSize;
            if (flag)
            {
                this._socket.Send(base.GetSendBytes(msg), base.DataPageLength, SocketFlags.None);
            }
            else
            {
                this._socket.Send(base.GetSendBytes(msg));
            }
            Log.Debug("发送消息成功");
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public override void Send(byte[] msg)
        {
            bool flag = !base.IsAutoSize;
            if (flag)
            {
                this._socket.Send(base.GetForntBytes(msg), base.DataPageLength, SocketFlags.None);
            }
            else
            {
                this._socket.Send(msg);
            }
            Log.Debug("发送消息成功");
        }

        /// <summary>
        /// 连接
        /// </summary>
        public override void Connect()
        {
            bool flag = !base.IsInit;
            if (flag)
            {
                Log.Fail("Socket 没有被初始化或者初始化设置失败");
                throw new Exception("Socket 没有被初始化或者初始化设置失败");
            }
            try
            {
                IPAddress ipaddress = IPAddress.Parse(base.Ip);
                this._socket = new Socket(ipaddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remoteEP = new IPEndPoint(ipaddress, base.Port);
                bool isOpen = base.IsOpen;
                if (isOpen)
                {
                    this.Close();
                }
                bool flag2 = base.ReciveBuffSize != -1;
                if (flag2)
                {
                    this._socket.ReceiveBufferSize = base.ReciveBuffSize;
                }
                this._socket.Connect(remoteEP);
                RecLink linkEvent = this.LinkEvent;
                if (linkEvent != null)
                {
                    linkEvent(this._socket);
                }
                this._recMsgThread = new Thread(new ThreadStart(this.RecMsg));
                this._recMsgThread.IsBackground = true;
                this._recMsgThread.Start();
                this._isOpen = true;
            }
            catch (Exception ex)
            {
                Log.Fail("连接服务端失败:" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        protected void RecMsg()
        {
            List<byte> list = new List<byte>();
            for (; ; )
            {
                bool flag = !this._socket.Connected;
                if (flag)
                {
                    Log.Error("客户端接收消息失败:未连接");
                    break;
                }
                try
                {
                    list.Clear();
                    bool isReciverForAll = this.IsReciverForAll;
                    if (isReciverForAll)
                    {
                        while (true)
                        {
                            byte[] array = new byte[base.DataPageLength];
                            var num = this._socket.Receive(array);
                            list.AddRange(array.Take(num));
                            Array.Clear(array, 0, array.Length);
                            array = null;
                            if (num < array.Length || this._socket.Available <= 0)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        byte[] array = new byte[base.DataPageLength];
                        this._socket.Receive(array, base.DataPageLength, SocketFlags.None);
                        list.AddRange(array);
                    }
                    bool isCheckLink = base.IsCheckLink;
                    if (isCheckLink)
                    {
                        bool flag4 = this._socket.Poll(10, SelectMode.SelectRead);
                        if (flag4)
                        {
                            Log.Fail("连接已被断开");
                            throw new Exception("连接已被断开!");
                        }
                    }
                }
                catch (ThreadAbortException ex)
                {
                    return;
                }
                catch (Exception ex2)
                {
                    Log.Fail("接收消息出错:" + ex2.Message);
                    return;
                }
                this.DealMsg(list.ToArray(), this._socket);
            }
            this.Close();
        }

        private Thread _recMsgThread = null;
    }
}
