using GeneralTool.General.Models;
using System;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 客户端代理基类
    /// </summary>
    public abstract class ClientProxy : IDisposable
    {
        #region Private 字段

        private bool disposedValue;

        #endregion Private 字段

        #region Private 析构函数

        // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        /// <summary>
        /// </summary>
        ~ClientProxy()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        #endregion Private 析构函数

        #region Public 事件

        /// <summary>
        /// 错误通知事件(注意,如果注册了此事件,则在远程方法出现错误时将会返回默认值,不会在方法内部抛出异常,而且如果方法具有返回值,则返回值为其默认值,引用类型返回null,值类型返回其默认空值,例如int返回0,请自行补捉因返回值而抛出的后续异常)
        /// </summary>
        public abstract event Action<ProxyErroModel> ErroMsg;

        #endregion Public 事件

        #region Public 方法

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion Public 方法

        #region Protected 方法

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        protected abstract void CloseClinetProxy();

        /// <summary>
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
                }

                // TODO: 释放未托管的资源(未托管的对象)并替代终结器
                // TODO: 将大型字段设置为 null
                CloseClinetProxy();
                disposedValue = true;
            }
        }

        #endregion Protected 方法
    }
}