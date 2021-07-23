using System;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 服务站点接口
    /// </summary>
    public interface IServerStation
    {
        /// <summary>
        /// 设置路由
        /// </summary>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        bool AddRoute(string url, object target, MethodInfo m);
        bool AddTypeConverter<T>(Func<string, object> converter);
        bool Close();
        bool RemoveRoute(string url);
        bool Start(string ip, int port);
    }
}