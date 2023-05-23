using System;

namespace GeneralTool.CoreLibrary.Enums
{
    /// <summary>
    /// 日志类型
    /// </summary>
    [Flags]
    public enum LogType
    {
        /// <summary>
        /// Info
        /// </summary>
        Info,

        /// <summary>
        /// Debug
        /// </summary>
        Debug,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 警告
        /// </summary>
        Waring,

        /// <summary>
        /// 失败
        /// </summary>
        Fail
    }
}