using System.Diagnostics;
using System.IO;
using System.Text;

namespace GeneralTool.General.ProcessHelpers
{
    /// <summary>
    /// 进程帮助类
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="exePath">exe应用程序路径</param>
        /// <param name="result">输出结果</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static string Run(string exePath, out string result, string args = "")
        {
            result = "";
            if (!File.Exists(exePath))
                return "没有找到应用程序 : " + exePath;

            var process = new Process();
            var startInfo = new ProcessStartInfo()
            {
                FileName = exePath,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = Encoding.Default,
                StandardErrorEncoding = Encoding.Default,
            };
            process.EnableRaisingEvents = true;

            process.StartInfo = startInfo;
            process.Start();
            result = process.StandardOutput.ReadToEnd();
            process.Close();
            process.Dispose();
            return result;
        }
    }
}
