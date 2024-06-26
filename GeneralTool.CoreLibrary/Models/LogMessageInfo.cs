﻿using System;

using GeneralTool.CoreLibrary.Enums;

namespace GeneralTool.CoreLibrary.Models
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
            Msg = msg;
            LogType = logType;
            CurrentFileName = path;
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
        public LogType LogType { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentThreadId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CurrentTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FullMsg
        {
            get;  set;
        }

        #endregion Public 属性
    }
}