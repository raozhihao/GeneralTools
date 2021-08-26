using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GeneralTool.General.ProcessHelpers
{
    /// <summary>
    /// 进程帮助类
    /// </summary>
    public static class ProcessHelper
    {
        #region Public 方法

        private static AutoResetEvent reciveEvent = new AutoResetEvent(false);
        private static List<string> reciveList = new List<string>();
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
        public static string Run(string exePath, string args = "", int timeOut = -1)
        {
            reciveEvent.Reset();
            reciveList.Clear();
            var process = new Process();
            var startInfo = new ProcessStartInfo()
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

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.StartInfo = startInfo;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                reciveEvent.WaitOne(timeOut);

                return string.Join(Environment.NewLine, reciveList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (process.HasExited)
                    process.Close();
                else
                    process.Kill();

                process.Dispose();
            }
        }

        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            reciveList.Add(e.Data);
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            reciveList.Add(e.Data);
        }

        private static void Process_Exited(object sender, EventArgs e)
        {
            reciveEvent.Set();
        }

        #endregion Public 方法
    }
}