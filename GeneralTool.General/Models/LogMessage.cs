using GeneralTool.General.Enums;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPath">路径</param>
        /// <param name="message">消息</param>
        /// <param name="logType">类型</param>
        public LogMessage(string currentPath, string message, LogType logType)
        {
            this.CurrentPath = currentPath;
            this.Message = message;
            this.logType = logType;
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string CurrentPath { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType logType { get; set; }

    }
}
