using GeneralTool.General.Enums;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using System;

namespace GeneralTool.General.Logs
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleLogInfo : ILog
    {
        /// <inheritdoc/>
        public event EventHandler<LogMessageInfo> LogEvent;

        /// <inheritdoc/>
        public void Debug(string msg) => this.Log(msg, LogType.Debug);

        /// <inheritdoc/>
        public void Fail(string msg) => this.Log(msg, LogType.Fail);

        /// <inheritdoc/>
        public void Info(string msg) => this.Log(msg, LogType.Info);

        /// <inheritdoc/>
        public void Waring(string msg) => this.Log(msg, LogType.Waring);

        /// <inheritdoc/>
        public void Error(string msg) => this.Log(msg, LogType.Error);

        /// <inheritdoc/>
        public void Log(string msg, LogType logType = LogType.Info)
        {
            this.LogEvent?.Invoke(this, new LogMessageInfo(msg, logType));
            Console.WriteLine($"ConsoleLog: {msg} ,LogType: {logType}");
        }

    }
}
