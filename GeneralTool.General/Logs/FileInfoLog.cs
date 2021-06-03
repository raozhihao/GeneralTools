using GeneralTool.General.Enums;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using System;
using System.IO;
using System.Text;

namespace GeneralTool.General.Logs
{
    /// <summary>
    /// 文件日志
    /// </summary>
    public class FileInfoLog : ILog
    {
        private readonly string logPathDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        /// <summary>
        /// 单个Log最大字节数
        /// </summary>
        public long MaxLength { get; set; } = 1024 * 1024 * 3;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logName"></param>
        public FileInfoLog(string logName)
        {
            this.logPathDir = Directory.CreateDirectory(Path.Combine(logPathDir, logName)).FullName;
        }
        /// <inheritdoc/>
        public event EventHandler<LogMessageInfo> LogEvent;

        /// <inheritdoc/>
        public void Debug(string msg) => this.Log(msg, LogType.Debug);

        /// <inheritdoc/>
        public void Fail(string msg) => this.Log(msg, LogType.Fail);

        /// <inheritdoc/>
        public void Info(string msg) => this.Log(msg, LogType.Info);

        /// <inheritdoc/>
        public void Waring(string msg) => this.Log(msg, LogType.Waring);

        /// <inheritdoc/>
        public void Erro(string msg) => this.Log(msg, LogType.Erro);

        /// <inheritdoc/>
        public void Log(string msg, LogType logType = LogType.Info)
        {
            try
            {
                this.LogEvent?.Invoke(this, new LogMessageInfo(msg, logType));
                msg += Environment.NewLine;
                string fileName = Path.Combine(this.logPathDir, DateTime.Now.ToString("yyyy-MM-dd_1") + ".log");
                var fileInfo = new FileInfo(fileName);
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
                            }
                            else
                            {
                                fileName = file.FullName;
                            }
                        }
                        else
                        {
                            if (fileInfo.Length > MaxLength)
                            {
                                fileName = Path.Combine(this.logPathDir, DateTime.Now.ToString("yyyy-MM-dd_") + (files.Length + 1) + ".log");
                            }
                            else
                            {
                                fileName = fileInfo.FullName;
                            }
                        }
                    }


                    //写入日志
                    using (var fileStream = File.Open(fileName, FileMode.Append))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(msg);
                        fileStream.Write(bytes, 0, bytes.Length);
                    }
                }
                else
                {
                    using (FileStream fileStream2 = File.Create(fileName))
                    {
                        byte[] bytes2 = Encoding.UTF8.GetBytes(msg);
                        fileStream2.Write(bytes2, 0, bytes2.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
