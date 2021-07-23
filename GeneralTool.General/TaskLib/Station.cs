using GeneralTool.General.Attributes;
using GeneralTool.General.Interfaces;
using GeneralTool.General.LinqExtensions;
using GeneralTool.General.Logs;
using System;
using System.Linq;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 
    /// </summary>
    public class Station
    {
        /// <summary>
        /// 获取或设置服务站点
        /// </summary>
        public IServerStation ServerStation
        {
            get;
            set;
        }


        private readonly ILog log;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonConvert"></param>
        /// <param name="log"></param>
        /// <param name="serverStation"></param>
        public Station(IJsonConvert jsonConvert = null, ILog log = null,IServerStation serverStation=null)
        {
            if (log == null)
                log = new ConsoleLogInfo();
            if (jsonConvert == null)
                jsonConvert = new BaseJsonCovert();
            if(serverStation==null)
                serverStation = new ServerStation(jsonConvert, log);

            this.log = log;
            this.ServerStation = serverStation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool AddStationObjectClass(object target)
        {
            RouteAttribute attributeByClass = target.GetAttributeByClass<RouteAttribute>();
            if (attributeByClass == null)
            {
                this.log.Debug($"类型 {target} 没有添加特性 {nameof(RouteAttribute)} ,跳过不执行");
                return false;
            }

            string rootPath = attributeByClass.Url;
            MethodInfo[] methods = target.GetType().GetMethods();

            var ms = from m in methods
                     where m.GetCustomAttribute<RouteAttribute>() != null
                     select m;

            ms.Foreach(m =>
           {
               var route = m.GetCustomAttribute<RouteAttribute>();

               ServerStation.AddRoute(rootPath + route.Url, target, m);
           });

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool Start(string ip, int port)
        {
            return ServerStation.Start(ip, port);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public bool AddParameterConverter<T>(Func<string, object> converter)
        {
            return ServerStation.AddTypeConverter<T>(converter);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            ServerStation.Close();
            log.Debug("已关闭服务");
        }
    }
}
