﻿using System;
using System.Collections.Generic;
using System.Reflection;

using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.TaskLib
{
    /// <summary>
    /// 服务站点接口
    /// </summary>
    public interface IServerStation
    {
        /// <summary>
        /// 请求的信息
        /// </summary>
        Dictionary<TaskKey, RequestAddressItem> RequestRoute { get; set; }

        #region Public 方法

        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="url">方法url</param>
        /// <param name="target">执行对象</param>
        /// <param name="m">执行方法</param>
        /// <param name="method">http请求方法</param>
        /// <returns></returns>
        bool AddRoute(string url, object target, MethodInfo m, HttpMethod method);
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