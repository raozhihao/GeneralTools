using GeneralTool.General.Enums;

namespace GeneralTool.General.Models
{
    /// <summary>
    /// 日志消息
    /// </summary>
    public struct LogMessage
    {
        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="currentPath">
        /// 路径
        /// </param>
        /// <param name="message">
        /// 消息
        /// </param>
        /// <param name="logType">
        /// 类型
        /// </param>
        public LogMessage(string currentPath, string message, LogType logType)
        {
            this.CurrentPath = currentPath;
            this.Message = message;
            this.LogType = logType;
        }

        #endregion Public 构造函数

        #region Public 属性

        /// <summary>
        /// 路径
        /// </summary>
        public string CurrentPath { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        #endregion Public 属性
    }
}