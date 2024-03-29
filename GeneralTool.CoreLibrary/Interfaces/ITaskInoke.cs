﻿using System.Collections.Generic;

using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.TaskLib;

namespace GeneralTool.CoreLibrary.Interfaces
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
        Dictionary<TaskKey, DoTaskParameterItem> this[object obj]
        {
            get;
        }

        #endregion Public 索引器

        #region Public 方法

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="parameterItem"></param>
        /// <returns></returns>
        object DoInterface(DoTaskParameterItem parameterItem);

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns>
        /// </returns>
        Dictionary<TaskKey, DoTaskParameterItem> GetInterfaces();

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
        bool Open(string ip, int port, params object[] tartget);

        /// <summary>
        /// 不使用服务开启任务
        /// </summary>
        /// <param name="taskInokes">
        /// 任务列表
        /// </param>
        /// <returns>
        /// </returns>
        bool OpenWithoutServer(params object[] taskInokes);

        #endregion Public 方法
    }
}