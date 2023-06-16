using System;
using System.Diagnostics;
using System.Threading;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.Logs
{
    /// <summary>
    /// </summary>
    public class ConsoleLogInfo : BaseLog
    {

        #region Public 方法

        /// <inheritdoc/>
        public override void Debug(string msg) => Log(msg, LogType.Debug);

        /// <inheritdoc/>
        public override void Error(string msg) => Log(msg, LogType.Error);

        /// <inheritdoc/>
        public override void Fail(string msg) => Log(msg, LogType.Fail);

        /// <inheritdoc/>
        public override void Info(string msg) => Log(msg, LogType.Info);

        /// <inheritdoc/>
        public override void Log(string msg, LogType logType = LogType.Info)
        {
            LogMessageInfo result = new LogMessageInfo(msg, logType)
            {
                CurrentThreadId = Thread.CurrentThread.ManagedThreadId,
                CurrentTime = DateTime.Now,
            };

            string headInfo = "";
            if (ShowLogTypeInfo) headInfo = "[" + result.LogType + "]";
            if (ShowLogThreadId) headInfo += " " + result.CurrentThreadId + " ";
            if (ShowLogTime) headInfo += " " + result.CurrentTime + ":";

            msg = $"{headInfo}{result.Msg}";
            result.FullMsg = msg;

            Trace.WriteLine(msg);
            base.LogEventMethod(this, result);
        }

        /// <inheritdoc/>
        public override void Waring(string msg) => Log(msg, LogType.Waring);

        #endregion Public 方法
    }
}