﻿using System;
using System.Diagnostics;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.ProcessHelpers
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

        /// <summary>
        /// 获取当前进程是否已退出
        /// </summary>
        public bool IsExit { get; private set; }

        /// <summary>
        /// 外部程序对象
        /// </summary>
        public Process Process { get; private set; }

        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="exePath">应用程序路径</param>
        /// <param name="args">参数</param>
        public void Run(string exePath, string workDir, string args)
        {
            this.Run(exePath, args, workDir, true, ProcessWindowStyle.Hidden);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exePath"></param>
        /// <param name="args"></param>
        /// <param name="workDir"></param>
        /// <param name="createNoWindow"></param>
        /// <param name="windowStyle"></param>
        public void Run(string exePath, string args, string workDir, bool createNoWindow, ProcessWindowStyle windowStyle)
        {
            this.IsExit = false;
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = createNoWindow,
                WindowStyle = windowStyle,
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
                Process = process;
            }
            catch (Exception ex)
            {
                ErroReceived(process, ex.GetInnerExceptionMessage());
                Close();
                this.IsExit = true;
            }
        }

        /// <summary>
        /// 关闭后台服务
        /// </summary>
        public void Close()
        {
            if (Process == null) return;
            this.UnEvents();
            try
            {

                if (Process.HasExited)
                    Process.Close();
                else
                    Process.Kill();

                Process.Dispose();
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(Process, ex.GetInnerExceptionMessage());
            }
            finally
            {
                this.Process = null;
                this.IsExit = true;
            }
        }

        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            this.Process.StandardInput.Write(message);
        }


        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="message"></param>
        public void WriteLine(string message)
        {
            this.Process.StandardInput.WriteLine(message);
        }

        private void UnEvents()
        {
            if (Process == null) return;
            Process.Exited -= Process_Exited;
            Process.OutputDataReceived -= Process_OutputDataReceived;
            Process.ErrorDataReceived -= Process_ErrorDataReceived;
        }

        private void ErroReceived(object sender, string e)
        {
            ErrorHandler?.Invoke(sender, e);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ErrorHandler?.Invoke(sender, e.Data);
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ReceivedHandler?.Invoke(sender, e.Data);
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Exited?.Invoke(sender, e);
            this.UnEvents();
            this.Process?.Dispose();
            this.Process = null;
            this.IsExit = true;
        }
    }

}
