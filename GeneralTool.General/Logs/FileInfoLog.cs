using GeneralTool.General.Enums;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GeneralTool.General.Logs
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public class FileInfoLog : ILog
    {
        #region Private 字段

        private static readonly object locker = new object();
        private readonly ConcurrentQueue<LogMessageInfo> lockDic = new ConcurrentQueue<LogMessageInfo>();

        private readonly string logPathDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private FileStream currentFileStream = null;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="logName">
        /// </param>
        public FileInfoLog(string logName)
        {
            this.logPathDir = Directory.CreateDirectory(Path.Combine(logPathDir, logName)).FullName;
        }

        #endregion Public 构造函数

        #region Public 事件

        /// <inheritdoc/>
        public event EventHandler<LogMessageInfo> LogEvent;

        #endregion Public 事件

        #region Public 属性

        /// <summary>
        /// 是否将日志定向到控制台
        /// </summary>
        public bool ConsoleLogEnable { get; set; } = true;

        /// <summary>
        /// 当前日志路径
        /// </summary>
        public string CurrentPath { get; protected set; }

        /// <summary>
        /// 单个Log最大字节数
        /// </summary>
        public long MaxLength { get; set; } = 1024 * 1024 * 3;

        #endregion Public 属性

        #region Public 方法

        /// <inheritdoc/>
        public void Debug(string msg) => this.Log(msg, LogType.Debug);

        /// <inheritdoc/>
        public void Error(string msg) => this.Log(msg, LogType.Error);

        /// <inheritdoc/>
        public void Fail(string msg) => this.Log(msg, LogType.Fail);

        /// <inheritdoc/>
        public void Info(string msg) => this.Log(msg, LogType.Info);

        /// <inheritdoc/>
        public void Log(string msg, LogType logType = LogType.Info)
        {
            try
            {
                lock (locker)
                {
                    string fileName = Path.Combine(this.logPathDir, DateTime.Now.ToString("yyyy-MM-dd_1") + ".log");
                    var fileInfo = new FileInfo(fileName);
                    var createNew = true;
                    if (fileInfo.Exists)
                    {
                        //查看是否已有日志
                        var files = Directory.GetFiles(this.logPathDir, "*.log");

                        if (files.Length > 0)
                        {
                            var file = new FileInfo(Path.Combine(this.logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + files.Length + ".log"));
                            if (file.Exists)
                            {
                                if (file.Length > MaxLength)
                                {
                                    fileName = Path.Combine(this.logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + (files.Length + 1) + ".log");
                                    createNew = true;
                                }
                                else
                                {
                                    fileName = file.FullName;
                                    createNew = false;
                                }
                            }
                            else
                            {
                                if (fileInfo.Length > MaxLength)
                                {
                                    fileName = Path.Combine(this.logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + (files.Length + 1) + ".log");
                                    createNew = true;
                                }
                                else
                                {
                                    fileName = fileInfo.FullName;
                                    createNew = false;
                                }
                            }
                        }
                    }

                    if (createNew)
                    {
                        this.currentFileStream?.Close();
                        this.currentFileStream?.Dispose();
                        this.currentFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    }
                    else if (this.currentFileStream == null)
                        this.currentFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                    msg = $"{DateTime.Now} : {msg}";
                    var info = new LogMessageInfo(msg, logType, fileName);
                    this.lockDic.Enqueue(info);
                    WriteLog();
                    this.CurrentPath = fileName;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        /// <inheritdoc/>
        public void Waring(string msg) => this.Log(msg, LogType.Waring);

        #endregion Public 方法

        #region Private 方法

        private void WriteLog()
        {
            var re = this.lockDic.TryDequeue(out var result);
            if (re)
            {
                var data = Encoding.UTF8.GetBytes(result.Msg + Environment.NewLine);
                this.currentFileStream.Write(data, 0, data.Length);
                this.currentFileStream.Flush();

                if (this.ConsoleLogEnable)
                    Trace.WriteLine($"[{result.LogType}] - {DateTime.Now} : {result.Msg}");
                this.LogEvent?.Invoke(this, result);
            }
        }

        #endregion Private 方法
    }
}