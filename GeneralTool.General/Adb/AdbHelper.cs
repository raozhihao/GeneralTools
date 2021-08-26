using System;
using System.Diagnostics;
using System.Text;

namespace GeneralTool.General.Adb
{
    /// <summary>
    /// Adb帮助类
    /// </summary>
    public class AdbHelper
    {
        readonly string adbPath;

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="adbExePath">adb.exe文件路径</param>
        public AdbHelper(string adbExePath) => this.adbPath = adbExePath;

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="args">命令参数,例如 adb shell getprop ro.product.brand </param>
        /// <returns></returns>
        public string Command(string args)
        {
            if (args.StartsWith("adb "))
            {
                args = args.Substring(3);
            }
            if (args.Trim() == "shell")
                return "非正确的命令";

            var cmd = new Process();

            var startInfo = new ProcessStartInfo()
            {
                FileName = adbPath,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            cmd.EnableRaisingEvents = true;
            cmd.StartInfo = startInfo;
            cmd.Start();
            var result = cmd.StandardOutput.ReadToEnd();
            cmd.Close();
            cmd.Dispose();
            return result.TrimEnd(new char[] { '\r', '\n' });
        }

        /// <summary>
        /// 获取手机厂商名称
        /// </summary>
        public string BrandName
        {
            get
            {
                return Command("adb shell getprop ro.product.brand");
            }
        }

        /// <summary>
        /// 获取手机设备型号
        /// </summary>
        public string ModelName
        {
            get
            {
                return Command("adb shell getprop ro.product.model");
            }
        }

        /// <summary>
        /// 获取手机的市场名称
        /// </summary>
        public string MarketName
        {
            get
            {
                return Command("adb -d shell getprop ro.product.odm.marketname");
            }
        }
        /// <summary>
        /// 获取手机电量
        /// </summary>
        public int BatteryLevel
        {
            get
            {
                var result = Command("adb shell \"dumpsys battery | grep level\"");
                if (string.IsNullOrWhiteSpace(result))
                    return -1;
                if (!result.Contains("level:"))
                    return -1;
                return Convert.ToInt32(result.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
        }

        /// <summary>
        /// 解锁/点亮手机屏幕
        /// </summary>
        public void UnlockScreen()
        {
            Command("adb shell input keyevent 224");
        }

        /// <summary>
        /// 锁定/休眠手机屏幕
        /// </summary>
        public void LockScreen()
        {
            Command("adb shell input keyevent 223");
        }

        /// <summary>
        /// 获取当前屏幕并返回保存截图的物理路径
        /// </summary>
        /// <returns></returns>
        public string CaptureCurrentScreen(string savePath)
        {
            var message = new StringBuilder();
            message.AppendLine(Command("adb shell screencap -p /sdcard/screen.png"));
            message.AppendLine(Command($"adb pull /sdcard/screen.png {savePath}\\screen.png"));
            return message.ToString();
        }
    }
}
