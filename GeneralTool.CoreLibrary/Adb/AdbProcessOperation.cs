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
            process.ErrorHandler += Process_ErrorHandler;
            process.ReceivedHandler += Process_ReceivedHandler;
            process.Exited += Process_Exited;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            isExited = true;
        }

        private void Process_ReceivedHandler(object sender, string e)
        {
            if (addDate)
                e = $"[{DateTime.Now.ToString(dateFoamrt)}]  " + e;
            _ = outPutMsg.AppendLine(e);
        }

        private void Process_ErrorHandler(object sender, string e)
        {
            if (!string.IsNullOrEmpty(e))
            {
                if (addDate)
                    e = $"[{DateTime.Now.ToString(dateFoamrt)}]  " + e;
                _ = outPutMsg.AppendLine(e);
            }

        }

        /// <summary>
        /// 获取adb执行结果(立即结束adb命令)
        /// </summary>
        /// <returns></returns>
        public string GetEndResult()
        {
            Dispose();
            return WaitResult();
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
            if (!isExited) Dispose();
            isExited = false;
            dateFoamrt = dateFomart;
            addDate = !string.IsNullOrWhiteSpace(dateFoamrt);

            if (adbCommand.StartsWith("adb "))
            {
                adbCommand = adbCommand.Substring(3);
            }

            if (adbCommand.Trim() == "shell")
            {
                throw new Exception("非正确的命令");
            }

            _ = outPutMsg.Clear();
            process.Run(this.adbPath, null, adbCommand);
        }

        /// <summary>
        /// 同步等待获取结果(只有adb命令结果返回全部完成此项才会退出)
        /// </summary>
        /// <returns></returns>
        public string WaitResult()
        {
            while (!isExited)
            {
                if (token.IsCancellationRequested)
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
            if (isExited) return;
            try
            {
                process.Close();
            }
            catch (Exception)
            {

            }
            isExited = true;
        }

    }
}
