using System;
using System.Net.Sockets;

using GeneralTool.General.Models;
using GeneralTool.General.ReflectionHelper;


using GeneralTool.General.SocketLib;
using GeneralTool.General.SocketLib.Models;
using System.Net;
using System.Linq;
using GeneralTool.General.ExceptionHelper;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 服务帮助类
    /// </summary>
    public class ServerHelper : IDisposable
    {
        #region Private 字段

        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, ReflectionClass> caches;
        private bool disposedValue;
        private readonly SerializeHelpers serialize = new SerializeHelpers();
        private readonly SocketServer<FixedHeadRecevieState> socketBase;

        private readonly string host;
        private readonly int port;
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
        public ServerHelper(string host = "127.0.0.1", int port = 55155)
        {
            this.host = host;
            this.port = port;
            caches = new System.Collections.Concurrent.ConcurrentDictionary<string, ReflectionClass>();
            this.socketBase = SimpleServerBuilder.CreateFixedCommandPack(null);
            this.socketBase.ReceiveEvent += this.SocketBase_RecevieAction;
        }

        #endregion Public 构造函数

        #region Private 析构函数

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        /// <summary>
        /// </summary>
        ~ServerHelper()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        #endregion Private 析构函数

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
        /// 注册类型
        /// </summary>
        /// <typeparam name="TCallTypeInterface">
        /// 接口
        /// </typeparam>
        /// <typeparam name="TCallType">
        /// 实现 TCallTypeInterface 的实际类型
        /// </typeparam>
        /// <returns>
        /// </returns>
        public bool RegisterClass<TCallTypeInterface, TCallType>()
        {
            Type callType = typeof(TCallTypeInterface);

            string name = callType.Name;
            ReflectionClass reflection = new ReflectionClass(typeof(TCallType));
            if (caches.ContainsKey(name))
            {
                caches[name] = null;
                caches[name] = reflection;
                return true;
            }
            else
            {
                return caches.TryAdd(name, reflection);
            }
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <typeparam name="TCallTypeInterface">
        /// 接口
        /// </typeparam>
        /// <param name="instance">
        /// 实现 TInterface 的实际对象
        /// </param>
        /// <returns>
        /// </returns>
        public bool RegisterClass<TCallTypeInterface>(object instance)
        {
            Type callType = typeof(TCallTypeInterface);
            string name = callType.Name;
            ReflectionClass reflection = new ReflectionClass(instance);
            if (caches.ContainsKey(name))
            {
                caches[name] = null;
                caches[name] = reflection;
                return true;
            }
            else
            {
                return caches.TryAdd(name, reflection);
            }
        }

        /// <summary>
        /// 开启侦听(在调用此方法之前,请先调用 <see cref="RegisterClass{ TCallType}"/> 或 <see cref="RegisterClass(object)"/>)
        /// </summary>
        /// 挂起连接队列的最大长度。
        /// <returns>
        /// </returns>
        public bool Start()
        {
            this.socketBase.Startup(IPAddress.Parse(this.host), this.port);
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
                        this.socketBase.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.GetInnerExceptionMessage());
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        #endregion Protected 方法

        #region Private 方法

        private void SocketBase_RecevieAction(object sender, ReceiveArg package)
        {
            var list = package.PackBuffer;
            var clientSocket = package.Client as Socket;
            RequestCommand cmd = null;
            try
            {
                cmd = serialize.Desrialize<RequestCommand>(list.ToArray());
            }
            catch (Exception ex)
            {
                ResponseCommand reponseCmd = new ResponseCommand
                {
                    Success = false,
                    Messages = "反序列化出现错误",
                    ResultObject = ex
                };
                byte[] bytes = serialize.Serialize(reponseCmd);
                new SocketCommon().Send(clientSocket, bytes);
                //package.TrySend(bytes);
                return;
            }

            bool re = caches.TryGetValue(cmd.ClassName, out ReflectionClass value);
            if (!re)
            {
                ResponseCommand reponseCmd = new ResponseCommand
                {
                    Messages = "服务端未注册该接口的实现类",
                    Success = false
                };
                byte[] bytes = serialize.Serialize(reponseCmd);
                new SocketCommon().Send(clientSocket, bytes);
                return;
            }
            else
            {
                ResponseCommand reponseCmd = value.Invoke(cmd);
                try
                {
                    byte[] bytes = serialize.Serialize(reponseCmd);
                    new SocketCommon().Send(clientSocket, bytes);
                }
                catch (Exception ex)
                {
                    reponseCmd.Success = false;
                    reponseCmd.Messages = "反序列化出现错误";
                    reponseCmd.ResultObject = ex;
                    byte[] bytes = serialize.Serialize(reponseCmd);
                    new SocketCommon().Send(clientSocket, bytes);
                }
            }
        }

        #endregion Private 方法
    }
}