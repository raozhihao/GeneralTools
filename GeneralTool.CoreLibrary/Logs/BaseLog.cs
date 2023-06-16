using System;
using System.IO;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Enums;
using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.Models;

namespace GeneralTool.CoreLibrary.Logs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseLog : ILog
    {
        /// <inheritdoc/>
        public bool ShowLogTypeInfo { get; set; } = true;
        /// <inheritdoc/>
        public bool ShowLogThreadId { get; set; }
        /// <inheritdoc/>
        public bool ShowLogTime { get; set; } = true;

        /// <inheritdoc/>
        public event EventHandler<LogMessageInfo> LogEvent;

        /// <inheritdoc/>
        public abstract void Debug(string msg);

        /// <inheritdoc/>
        public abstract void Error(string msg);

        /// <inheritdoc/>
        public abstract void Fail(string msg);

        /// <inheritdoc/>
        public abstract void Info(string msg);

        /// <inheritdoc/>
        public abstract void Log(string msg, LogType logType = LogType.Info);

        /// <inheritdoc/>
        public abstract void Waring(string msg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        public virtual void Log(object msg, LogType logType = LogType.Info) => Log(msg, logType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Debug(object msg) => Log(msg, LogType.Debug);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Info(object msg) => Log(msg, LogType.Info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Error(object msg) => Log(msg, LogType.Error);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Waring(object msg) => Log(msg, LogType.Waring);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Fail(object msg) => Log(msg, LogType.Fail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="logType"></param>
        /// <param name="objects"></param>
        public virtual void Log(string msg, LogType logType = LogType.Info, params object[] objects) => Log(msg.Fomart(objects), logType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Debug(string msg, params object[] objects) => Log(msg.Fomart(objects), LogType.Debug);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Info(string msg, params object[] objects) => Log(msg.Fomart(objects), LogType.Info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Waring(string msg, params object[] objects) => Log(msg.Fomart(objects), LogType.Waring);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objects"></param>
        public virtual void Fail(string msg, params object[] objects) => Log(msg.Fomart(objects), LogType.Fail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void LogEventMethod(object sender, LogMessageInfo e)
        {
            LogEvent?.Invoke(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }

        /// <summary>
        /// 清除Log日志
        /// </summary>
        /// <param name="days"></param>
        /// <param name="rootDir"></param>
        public static void ClearLogs(int days, string rootDir = null)
        {
            //清除本地LOG缓存 ,不是当天的全给清了
            //找到本地日志文件夹

            DirectoryInfo logDir = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"));
            if (!string.IsNullOrWhiteSpace(rootDir))
                logDir = new DirectoryInfo(rootDir);

            _ = Task.Run(() =>
           {
               //循环其中的文件夹
               foreach (DirectoryInfo directory in logDir.EnumerateDirectories())
               {
                   FileInfo[] files = directory.GetFiles("*.log");
                   foreach (FileInfo item in files)
                   {
                       try
                       {
                           if (DateTime.Now - item.CreationTime >= TimeSpan.FromDays(days))
                               item.Delete();
                       }
                       catch
                       {

                       }
                   }
               }
           });
        }
    }
}
