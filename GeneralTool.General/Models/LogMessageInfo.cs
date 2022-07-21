using GeneralTool.General.Enums;
using System;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// Log日志消息
    /// </summary>
    [Serializable]
    public class LogMessageInfo : EventArgs
    {
        /// <summary>
        /// 日志消息
        /// </summary>
        public string Msg { get; internal set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; internal set; }

        /// <summary>
        /// 写入的路径
        /// </summary>
        public string CurrentFileName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        /// <param name="path"></param>
        public LogMessageInfo(string msg, LogType logType, string path)
        {
            this.Msg = msg;
            this.LogType = logType;
            this.CurrentFileName = path;
        }
    }
}
