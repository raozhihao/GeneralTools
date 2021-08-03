using GeneralTool.General.Interfaces;
using GeneralTool.General.Logs;
using GeneralTool.General.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 站点服务基类
    /// </summary>
    public abstract class ServerStationBase : IServerStation
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="log">
        /// </param>
        public ServerStationBase(ILog log)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            this.Log = log;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 获取或设置日志
        /// </summary>
        public ILog Log { get; set; }

        #endregion Public 属性

        #region Protected 属性

        /// <summary>
        /// 当前请求的参数转换器集合
        /// </summary>
        protected Dictionary<string, ParamterConvertItem> ParamterConverters { get; set; } = new Dictionary<string, ParamterConvertItem>();

        /// <summary>
        /// 当前请求的路由集合
        /// </summary>
        protected Dictionary<string, RequestAddressItem> RequestRoute { get; set; } = new Dictionary<string, RequestAddressItem>();

        #endregion Protected 属性

        #region Public 方法

        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="m">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual bool AddRoute(string url, object target, MethodInfo m)
        {
            bool flag = this.RequestRoute.ContainsKey(url);
            bool result;
            if (flag)
            {
                this.Log.Error($"类型: {target} 方法: {url} 已存在,不添加");
                result = false;
            }
            else
            {
                this.RequestRoute.Add(url, new RequestAddressItem
                {
                    Url = url,
                    MethodInfo = m,
                    Target = target
                });
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 添加转换器
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="converter">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual bool AddTypeConverter<T>(Func<string, object> converter)
        {
            Type typeFromHandle = typeof(T);
            bool flag = this.ParamterConverters.ContainsKey(typeFromHandle.FullName);
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                this.ParamterConverters.Add(typeFromHandle.FullName, new ParamterConvertItem
                {
                    Type = typeFromHandle,
                    Converter = converter
                });
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        /// <returns>
        /// </returns>
        public abstract bool Close();

        /// <summary>
        /// 移除路由
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <returns>
        /// </returns>
        public bool RemoveRoute(string url)
        {
            bool flag = !this.RequestRoute.ContainsKey(url);
            return !flag && this.RequestRoute.Remove(url);
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="ip">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <returns>
        /// </returns>
        public abstract bool Start(string ip, int port);

        #endregion Public 方法
    }
}