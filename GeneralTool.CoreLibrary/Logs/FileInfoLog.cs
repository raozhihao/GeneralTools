using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.Logs
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public class FileInfoLog : BaseLog
    {
        #region Private 字段

        private static readonly object locker = new object();
        private readonly ConcurrentQueue<LogMessageInfo> lockDic = new ConcurrentQueue<LogMessageInfo>();

        private readonly string logPathDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private FileStream currentFileStream = null;
        private readonly string logName;
        //private Thread writeThread;
        //private CancellationTokenSource cancellationTokenSource;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// </summary>
        /// <param name="logName">
        /// </param>
        /// <param name="logBaseDir">日志存放目录,如果无,则存放在当前应用程序Logs文件夹下</param>
        public FileInfoLog(string logName, string logBaseDir = "")
        {
            if (string.IsNullOrWhiteSpace(logBaseDir))
            {
                var dir = Path.Combine(logPathDir, logName);
                Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(dir);
                logPathDir = new DirectoryInfo(dir).FullName;
            }
            else
            {
                this.logName = logName;
                logPathDir = new DirectoryInfo(logBaseDir).FullName;
                Microsoft.VisualBasic.FileIO.FileSystem.CreateDirectory(logPathDir);
            }

            //this.cancellationTokenSource = new CancellationTokenSource();
            //this.writeThread = new Thread(WriteLog) { IsBackground = true };
            //this.writeThread.Start();

        }

        #endregion Public 构造函数

        //#region Public 事件

        ///// <inheritdoc/>
        //public event EventHandler<LogMessageInfo> LogEvent;

        //#endregion Public 事件

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
            try
            {
                string fileName = Path.Combine(logPathDir, logName + DateTime.Now.ToString("yyyy-MM-dd_1") + ".log");
                lock (locker)
                {
                    FileInfo fileInfo = new FileInfo(fileName);
                    bool createNew = true;
                    if (fileInfo.Exists)
                    {
                        //查看是否已有日志
                        string[] files = Directory.GetFiles(logPathDir, "*.log");

                        if (files.Length > 0)
                        {
                            FileInfo file = new FileInfo(Path.Combine(logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + files.Length + ".log"));
                            if (file.Exists)
                            {
                                if (file.Length > MaxLength)
                                {
                                    fileName = Path.Combine(logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + (files.Length + 1) + ".log");
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
                                    fileName = Path.Combine(logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + (files.Length + 1) + ".log");
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
                    CurrentPath = fileName;
                    if (createNew)
                    {
                        currentFileStream?.Close();
                        currentFileStream?.Dispose();
                        currentFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    }
                    else if (currentFileStream == null)
                        currentFileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                }
                LogMessageInfo info = new LogMessageInfo(msg, logType, fileName) { CurrentTime = DateTime.Now, CurrentThreadId = Thread.CurrentThread.ManagedThreadId };
                //lockDic.Enqueue(info);
                WriteLog(info);

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        /// <inheritdoc/>
        public override void Waring(string msg) => Log(msg, LogType.Waring);


        /// <inheritdoc/>
        public override void Dispose()
        {
            DateTime time = DateTime.Now;
            while (!lockDic.IsEmpty)
            {
                Thread.Sleep(10);
                if (DateTime.Now - time > TimeSpan.FromMilliseconds(3))
                    break;
            }

            //this.cancellationTokenSource?.Cancel();
            //this.writeThread?.Join(1000);
            try
            {
                currentFileStream?.Close();
                currentFileStream?.Dispose();
            }
            catch
            {

            }
            //this.cancellationTokenSource?.Dispose();
            //this.cancellationTokenSource = new CancellationTokenSource();
        }

        #endregion Public 方法

        #region Private 方法

        //private void WriteLog()
        //{
        //    while (!this.cancellationTokenSource.IsCancellationRequested)
        //    {
        //        bool re = lockDic.TryDequeue(out LogMessageInfo result);
        //        if (re)
        //        {
        //            string headInfo = "";
        //            if (ShowLogTypeInfo) headInfo = "[" + result.LogType + "]";
        //            if (ShowLogThreadId) headInfo += " " + result.CurrentThreadId + " ";
        //            if (ShowLogTime) headInfo += " " + result.CurrentTime + ":";

        //            string msg = $"{headInfo}{result.Msg}";
        //            result.FullMsg = msg;

        //            byte[] data = Encoding.UTF8.GetBytes(msg + Environment.NewLine);
        //            currentFileStream.Write(data, 0, data.Length);
        //            currentFileStream.Flush();

        //            if (ConsoleLogEnable)
        //                Trace.WriteLine(msg);
        //            // base.LogEvent?.Invoke(this, result);

        //            base.LogEventMethod(this, result);
        //        }
        //        else
        //        {
        //            Thread.Sleep(5);
        //        }

        //    }

        //}

        private void WriteLog(LogMessageInfo result)
        {
            string headInfo = "";
            if (ShowLogTypeInfo) headInfo = "[" + result.LogType + "]";
            if (ShowLogThreadId) headInfo += " " + result.CurrentThreadId + " ";
            if (ShowLogTime) headInfo += " " + result.CurrentTime + ":";

            string msg = $"{headInfo}{result.Msg}";
            result.FullMsg = msg;

            byte[] data = Encoding.UTF8.GetBytes(msg + Environment.NewLine);
            currentFileStream.Write(data, 0, data.Length);
            currentFileStream.Flush();
            Array.Clear(data, 0, data.Length);

            if (ConsoleLogEnable)
                Trace.WriteLine(msg);

            try
            {
                base.LogEventMethod(this, result);
            }
            catch (Exception)
            {

            }

        }


        #endregion Private 方法


    }
}