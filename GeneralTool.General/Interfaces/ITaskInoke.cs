using GeneralTool.General.Models;
using System.Collections.Generic;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface ITaskInoke
    {
        /// <summary>
        /// 获取当前类型的所有任务集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Dictionary<string, DoTaskParameterItem> this[ITaskInoke obj]
        {
            get;
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="url">任务路由</param>
        /// <param name="parameterItem">任务项目</param>
        /// <returns></returns>
        object DoInterface(string url, DoTaskParameterItem parameterItem);

        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="tartget"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool Open(string ip, int port, params object[] tartget);

        /// <summary>
        /// 不使用服务开启任务
        /// </summary>
        /// <param name="taskInokes">任务列表</param>
        /// <returns></returns>
        bool OpenWithoutServer(params object[] taskInokes);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        Dictionary<string, DoTaskParameterItem> GetInterfaces();

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErroMsg { get; }
    }
}
