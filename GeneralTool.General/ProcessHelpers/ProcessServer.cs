﻿using System;
using System.Diagnostics;

using GeneralTool.General.Extensions;

namespace GeneralTool.General.ProcessHelpers
{
    /// <summary>
    /// 主要用于Process的交互
    /// </summary>
    public class ProcessServer
    {
        /// <summary>
        /// 组件出现异常时
        /// </summary>
        public event EventHandler<string> ErrorHandler;
        /// <summary>
        /// 接收到消息时
        /// </summary>
        public event EventHandler<string> ReceivedHandler;
        /// <summary>
        /// 组件退出时
        /// </summary>
        public event EventHandler Exited;

        private Process _process;
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="exePath">应用程序路径</param>
        /// <param name="args">参数</param>
        public void Run(string exePath, string args = "")
        {
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
                this._process = process;
            }
            catch (Exception ex)
            {
                this.ErroReceived(process, ex.GetInnerExceptionMessage());
            }
        }

        /// <summary>
        /// 关闭后台服务
        /// </summary>
        public void Close()
        {
            try
            {
                if (this._process.HasExited)
                    this._process.Close();
                else
                    this._process.Kill();

                this._process.Dispose();
            }
            catch (Exception ex)
            {
                this.ErrorHandler?.Invoke(this._process, ex.GetInnerExceptionMessage());
            }
        }

        private void ErroReceived(object sender, string e)
        {
            this.ErrorHandler?.Invoke(sender, e);
        }
        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.ErrorHandler?.Invoke(sender, e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.ReceivedHandler?.Invoke(sender, e.Data);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            this.Exited?.Invoke(sender, e);
        }
    }

}
