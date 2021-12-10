using System;

using GeneralTool.General.Enums;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// Log日志消息
    /// </summary>
    [Serializable]
    public class LogMessageInfo : EventArgs
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="msg">
        /// </param>
        /// <param name="logType">
        /// </param>
        /// <param name="path">
        /// </param>
        public LogMessageInfo(string msg, LogType logType, string path = "")
        {
            this.Msg = msg;
            this.LogType = logType;
            this.CurrentFileName = path;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 写入的路径
        /// </summary>
        public string CurrentFileName { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; internal set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Msg { get; internal set; }

        #endregion Public 属性
    }
}