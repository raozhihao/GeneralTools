using System;
using System.Reflection;

using GeneralTool.CoreLibrary.Attributes;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Logs;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// </summary>
    public class Station
    {
        #region Private 字段

        private readonly ILog log;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="jsonConvert">
        /// </param>
        /// <param name="log">
        /// </param>
        /// <param name="serverStation">
        /// </param>
        public Station(IJsonConvert jsonConvert = null, ILog log = null, IServerStation serverStation = null)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();
            if (serverStation == null)
                serverStation = new ServerStation(jsonConvert, log);

            this.log = log;
            ServerStation = serverStation;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 获取或设置服务站点
        /// </summary>
        public IServerStation ServerStation
        {
            get;
            set;
        }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <param name="converter">
        /// </param>
        /// <returns>
        /// </returns>
        public bool AddParameterConverter<T>(Func<string, object> converter)
        {
            return ServerStation.AddTypeConverter<T>(converter);
        }

        /// <summary>
        /// </summary>
        /// <param name="target">
        /// </param>
        /// <returns>
        /// </returns>
        public bool AddStationObjectClass(object target)
        {
            RouteAttribute attributeByClass = target.GetAttributeByClass<RouteAttribute>();
            string rootPath = target.GetType().Name + "/";
            if (attributeByClass != null)
            {
                rootPath = attributeByClass.Url;
            }

            _ = target.GetType().GetMethods().Foreach(m =>
           {
               RouteAttribute route = m.GetCustomAttribute<RouteAttribute>();
               if (route != null)
                   _ = ServerStation.AddRoute(rootPath + route.Url, target, m, route.Method);
           });

            return true;
        }

        /// <summary>
        /// </summary>
        public void Close()
        {
            _ = ServerStation.Close();
            log.Debug("已关闭服务");
        }

        /// <summary>
        /// </summary>
        /// <param name="ip">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Start(string ip, int port)
        {
            return ServerStation.Start(ip, port);
        }

        #endregion Public 方法
    }
}