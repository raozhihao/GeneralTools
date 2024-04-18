using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.SocketLib.Packages;

using static System.Windows.Forms.AxHost;

namespace GeneralTool.CoreLibrary.SocketLib
{
    #region New

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
        public Func<IPackage<T>> PackageFunc { get; set; }

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
        /// 接收区数据大小
        /// </summary>
        public int ReceiveBufferSize { get; set; } = 8192;

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="port"></param>
        public virtual void Startup(int port) => Startup(IPAddress.Any, port);
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
            Log = log;
        }

        /// <summary>
        /// 开始异步接收
        /// </summary>
        /// <param name="client"></param>
        protected virtual void BeginReceive(Socket client)
        {
            Log.Debug($"获取到 {client.RemoteEndPoint} 的连接");
            ReceiveState state = null;
            //开始异步接收数据
            if (PackageFunc == null)
            {
                PackageFunc = new Func<IPackage<T>>(() => new NoPackage<T>());
                state = PackageFunc().State;
            }
            else
            {
                state = PackageFunc().State;
                if (state == null)
                {
                    try
                    {
                        CloseClient(client.LocalEndPoint + "", client.RemoteEndPoint + "", new Exception("State不可为null"));
                    }
                    catch (Exception)
                    {

                    }
                    return;
                }
            }

            state.WorkSocket = client;
            state.BufferSize = ReceiveBufferSize;

            try
            {
                state.WorkRemoePoint = client.RemoteEndPoint.ToString();
                state.WorkLocalPoint = client.LocalEndPoint.ToString();
                _ = CurrentSockets.TryAdd(client.RemoteEndPoint.ToString(), client);
                Log.Debug($"开始异步读取 {client.RemoteEndPoint} 的数据");
                _ = client.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, ReceiveCallBack, state);
            }
            catch (Exception ex)
            {
                Log.Debug($"异步读取 {client.RemoteEndPoint} 失败:{ex}");
                try
                {
                    if (state == null)
                        CloseClient(client.LocalEndPoint + "", client.RemoteEndPoint + "", ex);
                    else
                        CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, ex);
                }
                catch (Exception)
                {

                }

            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            T state = ar.AsyncState as T;
            Socket client = state.WorkSocket;
            int read = 0;
            //获取已读取数据
            try
            {
                if (!client.IsClientConnected(state.WorkLocalPoint, state.WorkRemoePoint))
                {
                    Trace.WriteLine("EndReceive DisConnected");
                    CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, new Exception("已断开连接"));
                    return;
                }
                read = client.EndReceive(ar);
                if (read != 0)
                    Log.Debug($"读取到 {state.WorkRemoePoint} 的数据长度为 {read}");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EndReceive error");
                CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, ex);
                return;
            }
            if (!client.IsClientConnected(state.WorkLocalPoint, state.WorkRemoePoint))
            {
                Trace.WriteLine("EndReceive DisConnected");
                CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, new Exception("已断开连接"));
                return;
            }
            if (read > 0)
            {
                //写入包内
                state.ListBytes.AddRange(state.Buffer.Take(read));
                //分包处理
                try
                {
                    Log.Debug($"{state.WorkRemoePoint} 进行分包,分包程序:{PackageFunc}");
                    PackageFunc().Subpackage(state, ExecutePackage);
                }
                catch (Exception ex)
                {
                    CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, ex);
                    return;
                }
            }
            if (!client.IsClientConnected(state.WorkLocalPoint, state.WorkRemoePoint))
            {
                Trace.WriteLine("EndReceive SubPackage DisConnected");
                CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, new Exception("已断开连接"));
                return;
            }

           
            //继续接收
            try
            {
                Log.Debug($"{state.WorkRemoePoint} 继续接收");
                _ = client.BeginReceive(state.Buffer, 0, state.BufferSize, SocketFlags.None, ReceiveCallBack, state);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("BeginReceive DisConnected");
                CloseClient(state.WorkLocalPoint, state.WorkRemoePoint, ex);
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
            Log.Debug($"{client.RemoteEndPoint} 返回消息,长度:{packBuffer.Count()}");
            try
            {
                ReceiveEvent?.Invoke(this, new ReceiveArg(packBuffer, client));
            }
            catch (Exception e)
            {
                string msg = "处理包过程中未捕获的异常错误:" + e;
                Log.Fail(msg);
                ErrorEvent?.Invoke(this, new SocketErrorArg(client, new Exception(msg)));
                try
                {
                    CloseClient(client.LocalEndPoint + "", client.RemoteEndPoint + "", new Exception(msg));
                }
                catch (Exception)
                {

                }
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

            byte[] buffer = Encoding.UTF8.GetBytes(msg);

            return Send(buffer, socket);
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

                Log.Debug($"向 {socket.RemoteEndPoint} 发送消息,压包程序:{PackageFunc}");
                byte[] newBuffer = PackageFunc().Package(buffer);
                if (!socket.IsClientConnected(socket.LocalEndPoint + "", socket.RemoteEndPoint + ""))
                {
                    CloseClient(socket.LocalEndPoint + "", socket.RemoteEndPoint + "", new Exception("已断开连接"));
                    return false;
                }
                return SocketCommon.SendBytes(newBuffer, socket);
            }
            catch (Exception ex)
            {
                string msg = $"向 {socket.RemoteEndPoint} 发送消息失败:{ex}";
                Log.Fail(msg);
                ErrorEvent?.Invoke(this, new SocketErrorArg(socket, new Exception(msg)));
                try
                {
                    CloseClient(socket.LocalEndPoint + "", socket.RemoteEndPoint + "", new Exception(msg));
                }
                catch (Exception)
                {

                }
                return false;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void CloseClient(string localPoint, string remotePoint, Exception ex)
        {
            try
            {

                _ = CurrentSockets.TryRemove(remotePoint, out var client);
                Log.Debug($"关闭 {remotePoint} 的连接");
                if (client == null)
                    return;
                DisconnectEvent?.Invoke(this, new SocketErrorArg(client, ex));
                if (client.IsClientConnected(localPoint, remotePoint))
                {
                    client?.Close();
                    client?.Dispose();
                }


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
            Log.Debug($"关闭 {this} 的连接");
            IsConnected = false;
            if (Socket == null) return;
            try
            {
                Socket.Close();
                Socket.Dispose();
                foreach (KeyValuePair<string, Socket> item in CurrentSockets)
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

                CurrentSockets.Clear();
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
                    Close();
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

    #endregion
}
