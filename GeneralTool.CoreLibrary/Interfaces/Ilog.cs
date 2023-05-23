using System;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.Interfaces
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog : IDisposable
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        void Log(object msg, LogType logType = LogType.Info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void Debug(object msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void Info(object msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void Error(object msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void Waring(object msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        void Fail(object msg);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        /// <param name="objects"></param>
        void Log(string msg, LogType logType = LogType.Info, params object[] objects);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        void Debug(string msg, params object[] objects);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        void Info(string msg, params object[] objects);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        void Waring(string msg, params object[] objects);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        void Fail(string msg, params object[] objects);

        #endregion Public 方法

        #region 属性

        /// <summary>
        /// 显示日志类型
        /// </summary>
        bool ShowLogTypeInfo { get; set; }

        /// <summary>
        /// 显示日志Id
        /// </summary>
        bool ShowLogThreadId { get; set; }

        /// <summary>
        /// 显示日志时间
        /// </summary>
        bool ShowLogTime { get; set; }

        #endregion
    }
}