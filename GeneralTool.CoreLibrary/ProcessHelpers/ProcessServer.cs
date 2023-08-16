using System;
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

        private Process _process;
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="exePath">应用程序路径</param>
        /// <param name="args">参数</param>
        public void Run(string exePath, string workDir = null, string args = "")
        {
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
                _process = process;
            }
            catch (Exception ex)
            {
                ErroReceived(process, ex.GetInnerExceptionMessage());
                this.Close();
            }
        }

        /// <summary>
        /// 关闭后台服务
        /// </summary>
        public void Close()
        {
            if (_process == null) return;
            try
            {
                _process.Exited -= Process_Exited;
                _process.OutputDataReceived -= Process_OutputDataReceived;
                _process.ErrorDataReceived -= Process_ErrorDataReceived;
                if (_process.HasExited)
                    _process.Close();
                else
                    _process.Kill();

                _process.Dispose();
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(_process, ex.GetInnerExceptionMessage());
            }
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
            if (this._process.HasExited)
            {
                this.Close();
                return;
            }
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            Exited?.Invoke(sender, e);
        }
    }

}
