using System;
using System.Diagnostics;
using System.IO;

namespace GeneralTool.General.IPCHelper
{
    /// <summary>
    /// IPC客户端连接帮助类
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class IPCClientHelper<T> where T : MarshalByRefObject
    {
        #region Private 字段

        private T item;

        private string serverPath;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName">
        /// </param>
        public IPCClientHelper(string portName)
        {
            PortName = portName;
        }

        /// <summary>
        /// 构造函数，使用默认的端口名称
        /// </summary>
        public IPCClientHelper()
        {
            PortName = typeof(T).Name;
        }

        #endregion Public 构造函数

        #region Private 析构函数

        /// <summary>
        /// </summary>
        ~IPCClientHelper()
        {
            CloseServerProcess();
        }

        #endregion Private 析构函数

        #region Public 属性

        /// <summary>
        /// 连接时的错误信息
        /// </summary>
        public string ErroMsg { get; private set; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// 获取或设置端口名称
        /// </summary>
        public string PortName { get; set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 关闭由后台进程Host的IPC服务端应用程序
        /// </summary>
        public void CloseServerProcess()
        {
            try
            {
                ProcessExit();
                ErroMsg = "";
                IsConnected = false;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
            }
        }

        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <returns>
        /// </returns>
        public T GetInstance()
        {
            if (IsConnected)
            {
                return item;
            }
            try
            {
                string remoUri = $"ipc://{PortName}/{typeof(T).Name}";

                item = (T)Activator.GetObject(typeof(T), remoUri);

                ErroMsg = "";

                IsConnected = true;
                return item;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                IsConnected = false;
                return null;
            }
        }

        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <param name="portName">
        /// 服务端名称
        /// </param>
        /// <param name="serverPath">
        /// 后台服务进程路径
        /// </param>
        /// <returns>
        /// </returns>
        public T GetInstance(string portName, string serverPath)
        {
            PortName = portName;
            return GetInstance(serverPath);
        }

        /// <summary>
        /// 获取服务对象
        /// </summary>
        /// <param name="serverPath">
        /// 后台服务进程路径
        /// </param>
        /// <returns>
        /// </returns>
        public T GetInstance(string serverPath)
        {
            if (string.IsNullOrEmpty(serverPath))
            {
                return GetInstance();
            }

            bool exist = File.Exists(serverPath);
            if (!exist)
            {
                ErroMsg = "未找到服务端程序";
                return null;
            }

            bool re = RunServerProcess(serverPath);
            if (!re)
            {
                return null;
            }
            this.serverPath = serverPath;
            return GetInstance();
        }

        /// <summary>
        /// 在后台启动指定的应用程序
        /// </summary>
        /// <param name="serverPath">
        /// 应用程序路径
        /// </param>
        /// <returns>
        /// </returns>
        public bool RunServerProcess(string serverPath)
        {
            //Check application is running...
            string exeName = Path.GetFileNameWithoutExtension(serverPath);
            Process[] processs = Process.GetProcessesByName(exeName);
            foreach (Process p in processs)
            {
                p.Kill();
            }
            Process CmdProcess = new Process();
            try
            {
                CmdProcess.StartInfo.FileName = serverPath;

                CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口
                CmdProcess.StartInfo.UseShellExecute = false;       //不启用shell启动进程
                CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入
                CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出
                CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出

                CmdProcess.StartInfo.Arguments = Process.GetCurrentProcess().Id.ToString();
                CmdProcess.Start();//执行
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }

            return true;
        }

        #endregion Public 方法

        #region Private 方法

        private void ProcessExit()
        {
            if (string.IsNullOrWhiteSpace(serverPath))
            {
                return;
            }

            Process[] ps = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(serverPath));
            foreach (Process item in ps)
            {
                item.Kill();
            }
        }

        #endregion Private 方法
    }
}