using System.Collections.Generic;

using GeneralTool.General.Models;
using GeneralTool.General.TaskLib;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// 任务接口
    /// </summary>
    public interface ITaskInoke
    {
        #region Public 属性

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErroMsg { get; }

        #endregion Public 属性

        #region Public 索引器

        /// <summary>
        /// 获取当前类型的所有任务集合
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        Dictionary<string, DoTaskParameterItem> this[BaseTaskInvoke obj]
        {
            get;
        }

        #endregion Public 索引器

        #region Public 方法

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="url">
        /// 任务路由
        /// </param>
        /// <param name="parameterItem">
        /// 任务项目
        /// </param>
        /// <returns>
        /// </returns>
        object DoInterface(string url, DoTaskParameterItem parameterItem);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns>
        /// </returns>
        Dictionary<string, DoTaskParameterItem> GetInterfaces();

        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="tartget">
        /// </param>
        /// <param name="ip">
        /// </param>
        /// <param name="port">
        /// </param>
        /// <returns>
        /// </returns>
        bool Open(string ip, int port, params BaseTaskInvoke[] tartget);

        /// <summary>
        /// 不使用服务开启任务
        /// </summary>
        /// <param name="taskInokes">
        /// 任务列表
        /// </param>
        /// <returns>
        /// </returns>
        bool OpenWithoutServer(params BaseTaskInvoke[] taskInokes);


        #endregion Public 方法
    }
}