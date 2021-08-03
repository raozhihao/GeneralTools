using System;
using System.Reflection;

namespace GeneralTool.General.TaskLib
{
    /// <summary>
    /// 服务站点接口
    /// </summary>
    public interface IServerStation
    {
        #region Public 方法

        /// <summary>
        /// 设置路由
        /// </summary>
        /// <param name="url">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="m">
        /// </param>
        /// <returns>
        /// </returns>
        bool AddRoute(string url, object target, MethodInfo m);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        bool AddTypeConverter<T>(Func<string, object> converter);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool Close();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool RemoveRoute(string url);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool Start(string ip, int port);

        #endregion Public 方法
    }
}