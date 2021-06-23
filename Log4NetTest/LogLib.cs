using GeneralTool.General.Enums;
using GeneralTool.General.Models;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Reflection;

namespace Log4NetTest
{
    class LogLib : GeneralTool.General.Interfaces.ILog
    {
        public event EventHandler<LogMessageInfo> LogEvent;

        private static ILog log4Net;
        private string logName;
        public LogLib(string logName)
        {
            this.logName = logName;
            log4Net = this.GetLoggerByName(logName);
        }

        public void Debug(string msg)
        {
            this.Log(msg, LogType.Debug);
        }

        public void Error(string msg)
        {
            this.Log(msg, LogType.Error);
        }

        public void Fail(string msg)
        {
            this.Log(msg, LogType.Fail);
        }

        public void Info(string msg)
        {
            this.Log(msg, LogType.Info);
        }

        public void Log(string msg, LogType logType = LogType.Info)
        {
            switch (logType)
            {
                case LogType.Info:
                    log4Net.Info(msg);
                    break;
                case LogType.Debug:
                    log4Net.Debug(msg);
                    break;
                case LogType.Error:
                    log4Net.Error(msg);
                    break;
                case LogType.Waring:
                    log4Net.Warn(msg);
                    break;
                case LogType.Fail:
                    log4Net.Fatal(msg);
                    break;
            }

            this.LogEvent?.Invoke(this, new LogMessageInfo(msg, logType, ""));
        }

        public void Waring(string msg)
        {
            this.Log(msg, LogType.Waring);
        }

        public ILog GetLoggerByName(string name)
        {
            if (LogManager.Exists(name) == null)
            {
                // Pattern Layout defined
                PatternLayout patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "%date %thread %level %logger - %message%newline";
                patternLayout.ActivateOptions();

                // configurating the RollingFileAppender object
                RollingFileAppender appender = new RollingFileAppender();
                appender.Name = name;
                appender.AppendToFile = true;
                appender.File = $"Logs\\{name}.log";
                appender.StaticLogFileName = true;
                appender.PreserveLogFileNameExtension = true;
                appender.LockingModel = new FileAppender.MinimalLock();
                appender.Layout = patternLayout;
                appender.MaxSizeRollBackups = 512;
                appender.MaximumFileSize = "3MB";
                appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                appender.ActivateOptions();

                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

                var loger = hierarchy.GetLogger(name, hierarchy.LoggerFactory); //!!! 此处写法是重点，不容更改
                loger.Hierarchy = hierarchy;
                loger.AddAppender(appender);
                loger.Level = Level.All;

                BasicConfigurator.Configure();//!!! 此处写法是重点，不容更改

                var appname = Assembly.GetEntryAssembly().GetName().Name;
                var version = Assembly.GetEntryAssembly().GetName().Version;
                //  loger.Log(Level.Info, $"Log name {name} created for Application: {appname} Version: {version}", null);
            }
            var log = LogManager.GetLogger(name);
            return log;
        }
    }
}
