using System;
using System.Diagnostics;

using GeneralTool.General.Enums;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;

namespace GeneralTool.General.Logs
{
    /// <summary>
    /// </summary>
    public class ConsoleLogInfo : ILog
    {
        #region Public 事件

        /// <inheritdoc/>
        public event EventHandler<LogMessageInfo> LogEvent;

        #endregion Public 事件

        #region Public 方法

        /// <inheritdoc/>
        public void Debug(string msg) => this.Log(msg, LogType.Debug);

        /// <inheritdoc/>
        public void Error(string msg) => this.Log(msg, LogType.Error);

        /// <inheritdoc/>
        public void Fail(string msg) => this.Log(msg, LogType.Fail);

        /// <inheritdoc/>
        public void Info(string msg) => this.Log(msg, LogType.Info);

        /// <inheritdoc/>
        public void Log(string msg, LogType logType = LogType.Info)
        {
            Trace.WriteLine(msg);
            //System.Diagnostics.Trace.WriteLine($"ConsoleLog: {msg} ,LogType: {logType}");
            this.LogEvent?.Invoke(this, new LogMessageInfo(msg, logType, ""));
        }

        /// <inheritdoc/>
        public void Waring(string msg) => this.Log(msg, LogType.Waring);

        #endregion Public 方法
    }
}