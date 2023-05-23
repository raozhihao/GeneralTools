using System;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.Logs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseLog : ILog
    {
        /// <inheritdoc/>
        public bool ShowLogTypeInfo { get; set; } = true;
        /// <inheritdoc/>
        public bool ShowLogThreadId { get; set; }
        /// <inheritdoc/>
        public bool ShowLogTime { get; set; } = true;

        /// <inheritdoc/>
        public event EventHandler<LogMessageInfo> LogEvent;

        /// <inheritdoc/>
        public abstract void Debug(string msg);

        /// <inheritdoc/>
        public abstract void Error(string msg);

        /// <inheritdoc/>
        public abstract void Fail(string msg);

        /// <inheritdoc/>
        public abstract void Info(string msg);

        /// <inheritdoc/>
        public abstract void Log(string msg, LogType logType = LogType.Info);

        /// <inheritdoc/>
        public abstract void Waring(string msg);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        public virtual void Log(object msg, LogType logType = LogType.Info) => this.Log(msg, logType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Debug(object msg) => this.Log(msg, LogType.Debug);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Info(object msg) => this.Log(msg, LogType.Info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Error(object msg) => this.Log(msg, LogType.Error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Waring(object msg) => this.Log(msg, LogType.Waring);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Fail(object msg) => this.Log(msg, LogType.Fail);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        /// <param name="objects"></param>
        public virtual void Log(string msg, LogType logType = LogType.Info, params object[] objects) => this.Log(msg.Fomart(objects), logType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Debug(string msg, params object[] objects) => this.Log(msg.Fomart(objects), LogType.Debug);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Info(string msg, params object[] objects) => this.Log(msg.Fomart(objects), LogType.Info);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Waring(string msg, params object[] objects) => this.Log(msg.Fomart(objects), LogType.Waring);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Fail(string msg, params object[] objects) => this.Log(msg.Fomart(objects), LogType.Fail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void LogEventMethod(object sender, LogMessageInfo e)
        {
            this.LogEvent?.Invoke(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
