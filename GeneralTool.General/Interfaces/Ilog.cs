﻿using System;

using GeneralTool.General.Enums;
using GeneralTool.General.Models;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        #region Public 事件

        /// <summary>
        /// 日志写入触发事件
        /// </summary>
        event EventHandler<LogMessageInfo> LogEvent;

        #endregion Public 事件

        #region Public 方法

        /// <summary>
        /// 写入调试信息
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        void Debug(string msg);

        /// <summary>
        /// 写入错误信息
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        void Error(string msg);

        /// <summary>
        /// 写入失败信息
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        void Fail(string msg);

        /// <summary>
        /// 写入展示信息
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        void Info(string msg);

        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <param name="logType">
        /// </param>
        /// <returns>
        /// </returns>
        void Log(string msg, LogType logType = LogType.Info);

        /// <summary>
        /// 写入警告信息
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <returns>
        /// </returns>
        void Waring(string msg);

        #endregion Public 方法
    }
}