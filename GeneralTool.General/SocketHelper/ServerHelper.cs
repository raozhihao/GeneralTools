using GeneralTool.General.Models;
using GeneralTool.General.ReflectionHelper;
using System;

namespace GeneralTool.General.SocketHelper.Server
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
        private readonly ServerSocketBase socketBase;

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
            caches = new System.Collections.Concurrent.ConcurrentDictionary<string, ReflectionClass>();
            this.socketBase = new ServerSocketBase(host, port);
            this.socketBase.RecevieEvent += this.SocketBase_RecevieAction;
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
        /// <param name="backListenCount">
        /// 挂起连接队列的最大长度。
        /// </param>
        /// <returns>
        /// </returns>
        public bool Start(int backListenCount = 10)
        {
            this.socketBase.Start(backListenCount);
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
                        this.socketBase.Dispose();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                disposedValue = true;
            }
        }

        #endregion Protected 方法

        #region Private 方法

        private void SocketBase_RecevieAction(SocketReceiveArgs obj)
        {
            var list = obj.Buffer;
            var clientSocket = obj.ClinetSocket;
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
                obj.TrySend(bytes);
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
                obj.TrySend(bytes);
                return;
            }
            else
            {
                ResponseCommand reponseCmd = value.Invoke(cmd);
                try
                {
                    byte[] bytes = serialize.Serialize(reponseCmd);
                    obj.TrySend(bytes);
                }
                catch (Exception ex)
                {
                    reponseCmd.Success = false;
                    reponseCmd.Messages = "反序列化出现错误";
                    reponseCmd.ResultObject = ex;
                    byte[] bytes = serialize.Serialize(reponseCmd);
                    obj.TrySend(bytes);
                }
            }
        }

        #endregion Private 方法
    }
}