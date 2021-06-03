using GeneralTool.General.Enums;
using GeneralTool.General.Models;
using System;

namespace GeneralTool.General.Interfaces
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 日志写入触发事件
        /// </summary>
        event EventHandler<LogMessageInfo> LogEvent;

        /// <summary>
        /// 写入展示信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        void Info(string msg);

        /// <summary>
        /// 写入调试信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        void Debug(string msg);

        /// <summary>
        /// 写入警告信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        void Waring(string msg);

        /// <summary>
        /// 写入失败信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        void Fail(string msg);

        /// <summary>
        /// 写入错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        void Error(string msg);

        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        void Log(string msg, LogType logType = LogType.Info);

    }
}
