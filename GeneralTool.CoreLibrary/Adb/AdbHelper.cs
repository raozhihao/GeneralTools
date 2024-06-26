﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.TaskExtensions;

namespace GeneralTool.CoreLibrary.Adb
{
    /// <summary>
    /// Adb帮助类
    /// </summary>
    public class AdbHelper
    {
        private string adbPath;

        public AdbHelper()
        {

        }

        /// <summary>
        /// 设置adb路径
        /// </summary>
        /// <param name="adbExePath"></param>
        public void SetAdbPath(string adbExePath = "")
        {
            if (string.IsNullOrWhiteSpace(adbExePath))
                adbExePath = "adb";//尝试使用环境变量中的adb

            adbPath = adbExePath;
        }

        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="adbExePath">adb.exe文件路径</param>
        public AdbHelper(string adbExePath = "")
        {
            if (string.IsNullOrWhiteSpace(adbExePath))
                adbExePath = "adb";//尝试使用环境变量中的adb

            adbPath = adbExePath;
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="args">命令参数,例如 adb shell getprop ro.product.brand </param>
        /// <returns></returns>
        public string Command(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
                return "";

            if (args.StartsWith("adb "))
                args = args.Substring(3);

            if (args.Trim() == "shell")
                return "非正确的命令";

            Process cmd = new Process();

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo()
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
                _ = cmd.Start();
                string result = cmd.StandardOutput.ReadToEnd();
                cmd.Close();
                cmd.Dispose();
                cmd = null;
                return result.TrimEnd('\r', '\n');
            }
            catch (Exception ex)
            {
                return ex.GetInnerExceptionMessage();
            }
        }



        /// <summary>
        /// 关闭所有adb连接
        /// </summary>
        public static void TryKillAllAdbProcess()
        {
            while (true)
            {
                Process[] ps = Process.GetProcessesByName("adb");
                if (ps.Length == 0)
                    break;
                foreach (Process item in ps)
                {
                    try
                    {
                        item.Kill();
                    }
                    catch
                    {

                    }
                }
            }
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
                string result = Command("adb shell \"dumpsys battery | grep level\"");
                return string.IsNullOrWhiteSpace(result)
                    ? -1
                    : !result.Contains("level:") ? -1 : Convert.ToInt32(result.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
        }

        /// <summary>
        /// 发送按键命令
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string CommandKey(AdbKey key) => Command($"adb shell input keyevent {(int)key}");

        /// <summary>
        /// 解锁/点亮手机屏幕
        /// </summary>
        public void UnlockScreen()
        {
            _ = Command("adb shell input keyevent 224");
        }

        /// <summary>
        /// 锁定/休眠手机屏幕
        /// </summary>
        public void LockScreen()
        {
            _ = Command("adb shell input keyevent 223");
        }

        /// <summary>
        /// 获取当前屏幕并返回保存截图的物理路径
        /// </summary>
        /// <returns></returns>
        public string CaptureCurrentScreen(string savePath)
        {
            StringBuilder message = new StringBuilder();
            _ = message.AppendLine(Command("adb shell screencap -p /sdcard/screen.png"));
            _ = message.AppendLine(Command($"adb pull /sdcard/screen.png {savePath}\\screen.png"));
            return message.ToString();
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        public List<AdbDeviceData> AdbDeviceDatas
        {
            get
            {
                string result = Command("devices -l");
                string[] arr = result.Split(new string[2]
                   {
                        "\r\n",
                        "\n"
                   }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length == 1)
                {
                    return new List<AdbDeviceData>();
                }

                List<AdbDeviceData> list = new List<AdbDeviceData>();
                for (int i = 1; i < arr.Length; i++)
                {
                    list.Add(AdbDeviceData.CreateFromAdbData(arr[i]));
                }

                return list;
            }
        }

        /// <summary>
        /// 获取当前第一个设备
        /// </summary>
        public AdbDeviceData FirstDevice
        {
            get
            {
                return AdbDeviceDatas.FirstOrDefault();
            }
        }

        private string LoadSerial(string deviceSerial)
        {
            if (string.IsNullOrWhiteSpace(deviceSerial))
            {
                AdbDeviceData device = FirstDevice;
                return device == null ? throw new Exception("Cant find device") : device.Serial;
            }
            return deviceSerial;
        }

        /// <summary>
        /// 安装app
        /// </summary>
        /// <param name="appPath">app路径</param>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<string> InstallApp(string appPath, string deviceSerial = "")
        {
            Result<string> result = new Result<string>();
            try
            {
                deviceSerial = LoadSerial(deviceSerial);

                string reStr = Command($"adb -s {deviceSerial} install -t \"" + appPath + "\"");

                result.ResultBool = true;
                result.ResultItem = reStr;

                return result;
            }
            catch (Exception ex)
            {
                result.ResultBool = false;
                result.ResultItem = ex.GetInnerExceptionMessage();
                result.LastErroMsg = result.ResultItem;
                return result;
            }
        }

        /// <summary>
        /// 卸载app
        /// </summary>
        /// <param name="appPackageName">要卸载的包名</param>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<string> UninstallApp(string appPackageName, string deviceSerial = "")
        {
            Result<string> result = new Result<string>();
            try
            {
                deviceSerial = LoadSerial(deviceSerial);

                string reStr = Command($"adb -s {deviceSerial} uninstall \"" + appPackageName + "\"");

                result.ResultBool = true;
                result.ResultItem = reStr;

                return result;
            }
            catch (Exception ex)
            {
                result.ResultBool = false;
                result.ResultItem = ex.GetInnerExceptionMessage();
                result.LastErroMsg = result.ResultItem;
                return result;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">本地文件路径</param>
        /// <param name="remotePath">远程文件路径</param>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<string> PushFile(string filePath, string remotePath, string deviceSerial = "")
        {
            Result<string> result = new Result<string>();
            try
            {
                deviceSerial = LoadSerial(deviceSerial);

                string reStr = Command($"adb -s {deviceSerial} push " + filePath + " " + remotePath);

                result.ResultBool = true;
                result.ResultItem = reStr;
                return result;
            }
            catch (Exception ex)
            {
                string message = ex.GetInnerExceptionMessage();
                result.ResultBool = false;
                result.ResultItem = message;
                result.LastErroMsg = message;
                return result;
            }
        }

        /// <summary>
        /// 拉取文件
        /// </summary>
        /// <param name="localPath">本地保存路径</param>
        /// <param name="remotePath">远程拉取文件路径</param>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<string> PullFile(string localPath, string remotePath, string deviceSerial = "")
        {
            Result<string> result = new Result<string>();
            try
            {
                deviceSerial = LoadSerial(deviceSerial);

                string reStr = Command($"adb -s {deviceSerial} pull {remotePath} {localPath}");

                result.ResultBool = true;
                result.ResultItem = reStr;
                return result;
            }
            catch (Exception ex)
            {
                string message = ex.GetInnerExceptionMessage();
                result.ResultBool = false;
                result.ResultItem = message;
                result.LastErroMsg = message;
                return result;
            }
        }

        /// <summary>
        /// 获取当前屏幕UI
        /// </summary>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<XmlDocument> DumpScreen(string deviceSerial = "")
        {
            Result<XmlDocument> result = new Result<XmlDocument>();
            try
            {
                deviceSerial = LoadSerial(deviceSerial);
                string remotePath = "/sdcard/ui.xml";
                //adb获取
                string reStr = Command($"adb -s {deviceSerial} shell uiautomator dump {remotePath}");
                //读取文件
                string localTmp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ui.xml");
                Result<string> re = PullFile(localTmp, remotePath, deviceSerial);//拉到本地来
                if (!re.ResultBool)
                {
                    result.ResultBool = re.ResultBool;
                    result.LastErroMsg = re.LastErroMsg;
                    return result;
                }

                if (!File.Exists(localTmp))
                {
                    result.ResultBool = false;
                    result.LastErroMsg = "dump error:" + reStr;
                    return result;
                }

                string text = File.ReadAllText(localTmp);
                //获取从xml开头的字符串,因为可能会在小米机型上有错误
                int index = text.IndexOf("<?xml version='1.0'");
                if (index < 0)
                {
                    result.ResultBool = false;
                    result.LastErroMsg = "dump error:" + reStr;
                    return result;
                }
                text = text.Substring(index);

                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(text);
                    result.ResultItem = xmlDocument;
                    result.ResultBool = true;
                    return result;
                }
                catch (Exception ex)
                {
                    result.LastErroMsg = ex.GetInnerExceptionMessage();
                    result.ResultBool = false;
                    return result;
                }
                finally
                {
                    try
                    {
                        File.Delete(localTmp);
                        _ = Command("adb shell rm -rf /sdcard/ui.xml");
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                result.LastErroMsg = ex.GetInnerExceptionMessage();
                result.ResultBool = false;
                return result;
            }
        }

        /// <summary>
        /// 根据text来查找当前ui上的元素位置
        /// </summary>
        /// <param name="text"></param>
        /// <param name="timeout"></param>
        /// <param name="deviceSerial"></param>
        /// <returns></returns>
        public Result<Element> FindElemeteForText(string text, TimeSpan timeout = default, string deviceSerial = "")
        {
            return FindElement($"//node[@text='{text}']", timeout, deviceSerial);
        }

        /// <summary>
        /// 获取指定元素坐标
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="timeout"></param>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<Element> FindElement(string xpath, TimeSpan timeout = default, string deviceSerial = "")
        {
            Result<Element> result = new Result<Element>();
            Result<XmlDocument> resultXml = new Result<XmlDocument>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (timeout == TimeSpan.Zero || stopwatch.Elapsed < timeout)
            {
                resultXml = DumpScreen(deviceSerial);
                if (!resultXml.ResultBool)
                {
                    result.ResultBool = false;
                    result.LastErroMsg = resultXml.LastErroMsg;
                    continue;
                }

                XmlDocument xmlDocument = resultXml.ResultItem;
                if (xmlDocument != null)
                {
                    XmlNode xmlNode = xmlDocument.SelectSingleNode(xpath);
                    if (xmlNode != null)
                    {
                        string value = xmlNode.Attributes["bounds"].Value;
                        if (value != null)
                        {
                            int[] array = value.Replace("][", ",").Replace("[", "").Replace("]", "")
                                .Split(',')
                                .Select(int.Parse)
                                .ToArray();
                            Dictionary<string, string> dictionary = new Dictionary<string, string>();
                            foreach (XmlAttribute attribute in xmlNode.Attributes)
                            {
                                dictionary.Add(attribute.Name, attribute.Value);
                            }

                            Element cords = new Element((array[0] + array[2]) / 2, (array[1] + array[3]) / 2, dictionary);

                            result.ResultItem = cords;
                            result.ResultBool = true;
                            return result;
                        }
                    }
                }

                if (timeout == TimeSpan.Zero)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取当前UI界面上的所有元素坐标
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="timeout"></param>
        /// <param name="deviceSerial">设备序列号,没有则以当前查询到的第一个为主</param>
        /// <returns></returns>
        public Result<Element[]> FindElementsCords(string xpath, TimeSpan timeout = default, string deviceSerial = "")
        {
            Result<Element[]> result = new Result<Element[]>() { ResultBool = false };
            try
            {
                deviceSerial = LoadSerial(deviceSerial);
            }
            catch (Exception ex)
            {
                result.ResultBool = false;
                result.LastErroMsg = ex.GetInnerExceptionMessage();
                return result;
            }
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Result<XmlDocument> reXml = new Result<XmlDocument>();
            while (timeout == TimeSpan.Zero || stopwatch.Elapsed < timeout)
            {
                reXml = DumpScreen(deviceSerial);
                if (!reXml.ResultBool)
                {
                    result.ResultBool = false;
                    result.LastErroMsg = reXml.LastErroMsg;
                    continue;
                }
                XmlDocument xmlDocument = reXml.ResultItem;
                if (xmlDocument != null)
                {
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes(xpath);
                    if (xmlNodeList != null)
                    {
                        Element[] array = new Element[xmlNodeList.Count];
                        for (int i = 0; i < array.Length; i++)
                        {
                            string value = xmlNodeList[i].Attributes["bounds"].Value;
                            if (value == null)
                            {
                                continue;
                            }

                            int[] array2 = value.Replace("][", ",").Replace("[", "").Replace("]", "")
                                .Split(',')
                                .Select(int.Parse)
                                .ToArray();
                            Dictionary<string, string> dictionary = new Dictionary<string, string>();
                            foreach (XmlAttribute attribute in xmlNodeList[i].Attributes)
                            {
                                dictionary.Add(attribute.Name, attribute.Value);
                            }

                            Element cords = new Element((array2[0] + array2[2]) / 2, (array2[1] + array2[3]) / 2, dictionary);
                            array[i] = cords;
                        }

                        if (array.Length == 0)
                        {
                            result.ResultItem = null;
                            result.ResultBool = false;
                            result.LastErroMsg = "Cant find any element";
                            return result;
                        }

                        result.ResultBool = true;
                        result.ResultItem = array;
                        return result;
                    }
                }

                if (timeout == TimeSpan.Zero)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="deviceSerail"></param>
        /// <returns></returns>
        public Result<string> Click(int x, int y, string deviceSerail = "")
        {
            Result<string> result = new Result<string>();
            try
            {
                deviceSerail = LoadSerial(deviceSerail);
                result.ResultItem = Command($"adb -s {deviceSerail} shell input tap {x} {y}");
                result.ResultBool = true;
                return result;
            }
            catch (Exception ex)
            {
                result.ResultBool = false;
                result.LastErroMsg = ex.GetInnerExceptionMessage();
                return result;
            }
        }

        /// <summary>
        /// 获取当前屏幕
        /// </summary>
        /// <param name="deviceSerail"></param>
        /// <returns></returns>
        public Result<Image> GetScreen(string deviceSerail = "")
        {
            Result<Image> result = new Result<Image>() { ResultBool = false };
            try
            {
                deviceSerail = LoadSerial(deviceSerail);
                string remoPath = "/sdcard/screen.png";
                string localPath = "screen.png";
                //获取当前屏幕
                StringBuilder message = new StringBuilder();
                _ = message.AppendLine(Command($"adb -s {deviceSerail} shell screencap -p {remoPath}"));
                _ = message.AppendLine(Command($"adb -s {deviceSerail} pull {remoPath} {localPath}"));
                //读取
                Image image = Image.FromFile(localPath);
                result.ResultBool = true;
                result.ResultItem = image;
                return result;
            }
            catch (Exception ex)
            {
                result.LastErroMsg = ex.GetInnerExceptionMessage();
                return result;
            }
        }

        /// <summary>
        /// 滑动
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="deviceSerial"></param>
        /// <param name="time">滑动时间毫秒</param>
        /// <returns></returns>
        public Result<string> Swipe(int x1, int y1, int x2, int y2, string deviceSerial = "", int time = 500)
        {
            Result<string> result = new Result<string>() { ResultBool = false };
            try
            {
                deviceSerial = LoadSerial(deviceSerial);
                _ = Command($"adb shell  input swipe {x1} {y1} {x2} {y2}");
                result.ResultBool = true;
                return result;
            }
            catch (Exception ex)
            {
                result.LastErroMsg = ex.GetInnerExceptionMessage();
                result.ResultItem = ex.GetInnerExceptionMessage();
                return result;
            }
        }
    }
}
