using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.ProcessHelpers
{
    /// <summary>
    /// 进程帮助类
    /// </summary>
    public static class ProcessHelper
    {
        #region Public 方法

        private static readonly AutoResetEvent reciveEvent = new AutoResetEvent(false);
        private static readonly List<string> reciveList = new List<string>();
        /// <summary>
        /// 接收消息事件
        /// </summary>
        public static event Action<string> ReciveEvent;
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="exePath">
        /// exe应用程序路径
        /// </param>
        /// <param name="args">
        /// 参数
        /// </param>
        /// <param name="timeOut"></param>
        /// <returns>
        /// </returns>
        public static string Run(string exePath, string args = "", string workDir = null, int timeOut = -1)
        {
            _ = reciveEvent.Reset();
            reciveList.Clear();
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            };

            if (!string.IsNullOrWhiteSpace(workDir))
                startInfo.WorkingDirectory = workDir;

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.StartInfo = startInfo;
            try
            {
                _ = process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                bool re = reciveEvent.WaitOne(timeOut);

                //超时停止,不再接收
                process.Exited -= Process_Exited;
                process.OutputDataReceived -= Process_OutputDataReceived;
                process.ErrorDataReceived -= Process_ErrorDataReceived;

            }
            catch (Exception ex)
            {
                reciveList.Add(ex.GetInnerExceptionMessage());
            }
            finally
            {
                if (process.HasExited)
                    process.Close();
                else
                    process.Kill();

                process.Dispose();
            }
            return string.Join(Environment.NewLine, reciveList);
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (sender is Process p)
            {

                reciveList.Add(e.Data);
                ReciveEvent?.Invoke(e.Data);

            }
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (sender is Process p)
            {

                reciveList.Add(e.Data);
                ReciveEvent?.Invoke(e.Data);

            }
        }

        private static void Process_Exited(object sender, EventArgs e)
        {
            _ = reciveEvent.Set();
        }

        #endregion Public 方法
    }
}