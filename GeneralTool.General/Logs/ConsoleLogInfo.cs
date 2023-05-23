using System;
using System.Diagnostics;
using System.Threading;

using GeneralTool.General.Enums;
using GeneralTool.General.Models;

namespace GeneralTool.General.Logs
{
    /// <summary>
    /// </summary>
    public class ConsoleLogInfo : BaseLog
    {


        #region Public 方法

        /// <inheritdoc/>
        public override void Debug(string msg) => this.Log(msg, LogType.Debug);

        /// <inheritdoc/>
        public override void Error(string msg) => this.Log(msg, LogType.Error);

        /// <inheritdoc/>
        public override void Fail(string msg) => this.Log(msg, LogType.Fail);

        /// <inheritdoc/>
        public override void Info(string msg) => this.Log(msg, LogType.Info);

        /// <inheritdoc/>
        public override void Log(string msg, LogType logType = LogType.Info)
        {
            var result = new LogMessageInfo(msg, logType)
            {
                CurrentThreadId = Thread.CurrentThread.ManagedThreadId,
                CurrentTime = DateTime.Now,
            };

            var headInfo = "";
            if (this.ShowLogTypeInfo) headInfo = "[" + result.LogType + "]";
            if (this.ShowLogThreadId) headInfo += " " + result.CurrentThreadId + " ";
            if (this.ShowLogTime) headInfo += " " + result.CurrentTime + ":";

            msg = $"{headInfo}{result.Msg}";
            result.FullMsg = msg;

            Trace.WriteLine(msg);
            base.LogEventMethod(this, result);
        }

        /// <inheritdoc/>
        public override void Waring(string msg) => this.Log(msg, LogType.Waring);

        #endregion Public 方法
    }
}