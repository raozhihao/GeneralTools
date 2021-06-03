using GeneralTool.General.Enums;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// Log日志消息
    /// </summary>
    public class LogMessageInfo
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
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        public LogMessageInfo(string msg, LogType logType)
        {
            this.Msg = msg;
            this.LogType = logType;
        }
    }
}
