using System;
using System.Diagnostics;
using System.Threading;

using GeneralTool.General.Extensions;

namespace GeneralTool.General.ProcessHelpers
{
    /// <summary>
    /// Process 帮助类,只支持子进程返回一条信息
    /// </summary>
    public class ProcessEngine
    {
        /// <summary>
        /// 读取返回信息的超时时间,默认为-1,永不超时
        /// </summary>
        public int ReadTimeOut { get; set; } = -1;

        /// <summary>
        /// 返回的错误信息
        /// </summary>
        public string ErroMsg { get; private set; }

        /// <summary>
        /// 进程退出事件
        /// </summary>
        public event Action ProcessExitEvent;


        private Process process;
        private AutoResetEvent resetEvent;
        private string reciveMsg;

        /// <summary>
        /// 启动程序
        /// </summary>
        /// <param name="exePath">应用程序路径</param>
        /// <param name="args">参数信息</param>
        /// <returns></returns>
        public bool Run(string exePath, string args = "")
        {
            this.process = new Process();
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
                this.process.BeginOutputReadLine();
                this.process.BeginErrorReadLine();
                this.reciveMsg = null;
                resetEvent = new AutoResetEvent(false);
                return true;
            }
            catch (Exception ex)
            {
                this.ErroMsg = "启动程序出现错误:" + ex.GetInnerExceptionMessage();
                return false;
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="line">数据</param>
        /// <returns></returns>
        public string WriteLine(string line)
        {
            resetEvent.Reset();
            this.reciveMsg = null;
            this.process.StandardInput.WriteLine(line);
            resetEvent.WaitOne(this.ReadTimeOut);
            var msg = this.reciveMsg;
            this.reciveMsg = null;
            return msg;
        }

        /// <summary>
        /// 写入消息,不获取返回值
        /// </summary>
        /// <param name="line"></param>
        public void WriteLineVoid(string line)
        {
            this.process.StandardInput.WriteLine(line);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                if (this.process.HasExited)
                    this.process.Close();
                else
                    this.process.Kill();

                this.process.Dispose();
            }
            catch (Exception ex)
            {
                this.ErroMsg = ex.GetInnerExceptionMessage();
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.reciveMsg = e.Data;
            resetEvent?.Set();
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.reciveMsg = e.Data;
            resetEvent?.Set();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            resetEvent?.Set();

            this.ProcessExitEvent?.Invoke();
        }

        /// <summary>
        /// 
        /// </summary>
        ~ProcessEngine()
        {
            this.Close();
        }
    }
}
