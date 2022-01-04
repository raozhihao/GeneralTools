

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.SocketLib.Interfaces;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.SocketLib.Packages;

namespace GeneralTool.General.SocketLib
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseSocket<T> : IDisposable where T : ReceiveState, new()
    {
        private bool disposedValue;

        /// <summary>
        /// 指示的解包程序,如果为null,则不进行解包,直接原样接收原样发送
        /// </summary>
        public Func<IPackage<T>> Package { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract Socket Socket { get; protected set; }

        /// <summary>
        /// 是否连接
        /// </summary>
        public abstract bool IsConnected { get; protected set; }

        /// <summary>
        /// 客户端消息接收事件
        /// </summary>
        public event EventHandler<ReceiveArg> ReceiveEvent;
        /// <summary>
        /// 客户端错误
        /// </summary>
        public event EventHandler<SocketErrorArg> ErrorEvent;
        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        public event EventHandler<SocketErrorArg> DisconnectEvent;

        /// <summary>
        /// 已连接的Socket
        /// </summary>
        public ConcurrentDictionary<string, Socket> CurrentSockets = new ConcurrentDictionary<string, Socket>();

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="port"></param>
        public virtual void Startup(int port) => this.Startup(IPAddress.Any, port);
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public abstract void Startup(IPAddress address, int port);
        /// <summary>
        /// 日志
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public BaseSocket(ILog log = null)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            this.Log = log;
        }

        /// <summary>
        /// 开始异步接收
        /// </summary>
        /// <param name="client"></param>
        protected virtual void BeginReceive(Socket client)
        {
            this.Log.Debug($"获取到 {client.RemoteEndPoint} 的连接");
            ReceiveState state = null;
            //开始异步接收数据
            if (this.Package == null)
            {
                this.Package = new Func<IPackage<T>>(() => new NoPackage<T>());
                state = this.Package().State;
            }
            else
            {
                state = this.Package().State;
                if (state == null)
                {
                    this.CloseClient(client, new Exception("State不可为null"));
                    return;
                }
            }

            state.WorkSocket = client;
            try
            {
                this.CurrentSockets.TryAdd(client.RemoteEndPoint.ToString(), client);
                this.Log.Debug($"开始异步读取 {client.RemoteEndPoint} 的数据");
                client.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, ReceiveCallBack, state);
            }
            catch (Exception ex)
            {
                this.Log.Debug($"异步读取 {client.RemoteEndPoint} 失败:{ex}");
                this.CloseClient(client, ex);
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {

            var state = ar.AsyncState as T;
            var client = state.WorkSocket;
            int read = 0;
            //获取已读取数据
            try
            {
                read = client.EndReceive(ar);
                this.Log.Debug($"读取到 {client.RemoteEndPoint} 的数据长度为 {read}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EndReceive error");
                CloseClient(client, ex);
                return;
            }
            if (!client.IsClientConnected())
            {
                Trace.WriteLine("EndReceive DisConnected");
                CloseClient(client, new Exception("已断开连接"));
                return;
            }
            if (read > 0)
            {
                //写入包内
                state.ListBytes.AddRange(state.Buffer.Take(read));
                //分包处理
                try
                {
                    this.Log.Debug($"{client.RemoteEndPoint} 进行分包,分包程序:{this.Package}");
                    this.Package().Subpackage(state, this.ExecutePackage);
                }
                catch (Exception ex)
                {
                    CloseClient(client, ex);
                    return;
                }
            }
            if (!client.IsClientConnected())
            {
                Trace.WriteLine("EndReceive SubPackage DisConnected");
                CloseClient(client, new Exception("已断开连接"));
                return;
            }

            this.Log.Debug($"{client.RemoteEndPoint} 继续接收");
            //继续接收
            try
            {
                client.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, ReceiveCallBack, state);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("BeginReceive DisConnected");
                CloseClient(client, ex);
                return;
            }
        }

        /// <summary>
        /// 处理包
        /// </summary>
        /// <param name="packBuffer"></param>
        /// <param name="client"></param>
        protected virtual void ExecutePackage(IEnumerable<byte> packBuffer, Socket client)
        {
            this.Log.Debug($"{client.RemoteEndPoint} 返回消息,长度:{packBuffer.Count()}");
            try
            {
                this.ReceiveEvent?.Invoke(this, new ReceiveArg(packBuffer, client));
            }
            catch (Exception e)
            {
                var msg = "处理包过程中未捕获的异常错误:" + e;
                this.Log.Fail(msg);
                this.ErrorEvent?.Invoke(this, new SocketErrorArg(client, new Exception(msg)));
            }
        }


        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual bool Send(string msg, Socket socket)
        {

            var buffer = Encoding.UTF8.GetBytes(msg);

            return this.Send(buffer, socket);
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="socket"></param>
        /// <returns></returns>
        public virtual bool Send(byte[] buffer, Socket socket)
        {
            if (buffer == null || buffer.Length == 0)
                return false;
            try
            {

                this.Log.Debug($"向 {socket.RemoteEndPoint} 发送消息,压包程序:{this.Package}");
                var newBuffer = this.Package().Package(buffer);
                if (!socket.IsClientConnected())
                {
                    CloseClient(socket, new Exception("已断开连接"));
                    return false;
                }
                return SocketCommon.SendBytes(newBuffer, socket);
            }
            catch (Exception ex)
            {
                var msg = $"向 {socket.RemoteEndPoint} 发送消息失败:{ex}";
                this.Log.Fail(msg);
                this.ErrorEvent?.Invoke(this, new SocketErrorArg(socket, new Exception(msg)));
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ex"></param>
        protected virtual void CloseClient(Socket client, Exception ex)
        {
            try
            {
                if (client == null)
                    return;
                this.CurrentSockets.TryRemove(client.RemoteEndPoint.ToString(), out _);
                this.Log.Debug($"关闭 {client.RemoteEndPoint} 的连接");

                this.DisconnectEvent?.Invoke(this, new SocketErrorArg(client, ex));
                client.Close();
                client.Dispose();

                client = null;
            }
            catch
            {

            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            this.Log.Debug($"关闭 {this} 的连接");
            this.IsConnected = false;
            if (this.Socket == null) return;
            try
            {
                this.Socket.Close();
                this.Socket.Dispose();
                foreach (var item in this.CurrentSockets)
                {
                    try
                    {
                        item.Value.Disconnect(false);
                        item.Value.Close();
                        item.Value.Dispose();
                    }
                    catch
                    {

                    }
                }

                this.CurrentSockets.Clear();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)
                    this.Close();
                }

                // TODO: 释放未托管的资源(未托管的对象)并重写终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        /// <summary>
        /// 
        /// </summary>
        ~BaseSocket()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
