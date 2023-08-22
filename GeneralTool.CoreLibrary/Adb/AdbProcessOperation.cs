using System;
using System.Text;
using System.Threading;

using GeneralTool.CoreLibrary.ProcessHelpers;

namespace GeneralTool.CoreLibrary.Adb
{
    /// <summary>
    /// Adb操作块
    /// </summary>
    public class AdbProcessOperation : IDisposable
    {
        private readonly ProcessServer process = new ProcessServer();
        private readonly StringBuilder outPutMsg = new StringBuilder();
        private bool isExited;
        private string dateFoamrt;
        private bool addDate;
        private string adbPath;
        private CancellationToken token;
        /// <summary>
        /// 
        /// </summary>
        public AdbProcessOperation()
        {
            this.process.ErrorHandler += Process_ErrorHandler;
            this.process.ReceivedHandler += Process_ReceivedHandler;
            this.process.Exited += Process_Exited;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            this.isExited = true;
        }

        private void Process_ReceivedHandler(object sender, string e)
        {
            if (this.addDate)
                e = $"[{DateTime.Now.ToString(this.dateFoamrt)}]  " + e;
            this.outPutMsg.AppendLine(e);
        }

        private void Process_ErrorHandler(object sender, string e)
        {
            if (!string.IsNullOrEmpty(e))
            {
                if (this.addDate)
                    e = $"[{DateTime.Now.ToString(this.dateFoamrt)}]  " + e;
                this.outPutMsg.AppendLine(e);
            }

        }

        /// <summary>
        /// 获取adb执行结果(立即结束adb命令)
        /// </summary>
        /// <returns></returns>
        public string GetEndResult()
        {
            this.Dispose();
            return this.WaitResult();
        }

        /// <summary>
        /// 执行adb命令
        /// </summary>
        /// <param name="adbCommand">adb命令</param>
        /// <param name="dateFomart">是否在返回前面加上日期,日期的格式化文本,例如 yyyy-MM-dd HH:mm:ss</param>
        /// <exception cref="Exception"></exception>
        public void Run(string adbPath, string adbCommand, string dateFomart = "", CancellationToken token = default)
        {
            this.adbPath = adbPath;
            this.token = token;
            if (string.IsNullOrWhiteSpace(this.adbPath))
                this.adbPath = "adb";
            if (!isExited) this.Dispose();
            this.isExited = false;
            this.dateFoamrt = dateFomart;
            this.addDate = !string.IsNullOrWhiteSpace(this.dateFoamrt);

            if (adbCommand.StartsWith("adb "))
            {
                adbCommand = adbCommand.Substring(3);
            }

            if (adbCommand.Trim() == "shell")
            {
                throw new Exception("非正确的命令");
            }

            this.outPutMsg.Clear();
            this.process.Run(this.adbPath, null, adbCommand);
        }

        /// <summary>
        /// 同步等待获取结果(只有adb命令结果返回全部完成此项才会退出)
        /// </summary>
        /// <returns></returns>
        public string WaitResult()
        {
            while (!this.isExited)
            {
                if (this.token.IsCancellationRequested)
                    break;
                Thread.Sleep(1);
            }
            return outPutMsg.ToString();
        }

        /// <summary>
        /// 释放资源(这也会立即停止)
        /// </summary>
        public void Dispose()
        {
            if (this.isExited) return;
            try
            {
                this.process.Close();
            }
            catch (Exception)
            {

            }
            this.isExited = true;
        }

    }
}
