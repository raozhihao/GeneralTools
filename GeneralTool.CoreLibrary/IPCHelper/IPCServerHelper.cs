#if NET452
using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

using GeneralTool.CoreLibrary.Extensions;

namespace GeneralTool.CoreLibrary.IPCHelper
{
    /// <summary>
    /// IPC服务端帮助类
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class IPCServerHelper<T> : IDisposable where T : MarshalByRefObject
    {
        #region Private 字段

        private IpcChannel channel;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName">
        /// 端口名称
        /// </param>
        public IPCServerHelper(string portName)
        {
            PortName = portName;
        }

        /// <summary>
        /// 构造函数，端口名称将默认以类型名称命名
        /// </summary>
        public IPCServerHelper()
        {
            PortName = typeof(T).Name;
        }

        #endregion Public 构造函数

        #region Private 析构函数

        /// <summary>
        /// </summary>
        ~IPCServerHelper()
        {
            _ = Stop();
        }

        #endregion Private 析构函数

        #region Public 属性
        /// <summary>
        /// 
        /// </summary>
        public string ErroMsg { get; private set; }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <summary>
        /// 是否已注册
        /// </summary>
        public bool IsRegisted { get; private set; }

        /// <summary>
        /// 获取或设备服务端口
        /// </summary>
        public string PortName { get; private set; }

        /// <summary>
        /// 获取或设备服务端URL
        /// </summary>
        public string RemoteUrl { get; private set; }

        #endregion Public 属性

        #region Public 方法

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _ = Stop();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <returns>
        /// </returns>
        public bool RegisterServer()
        {
            if (IsRegisted)
            {
                return true;
            }
            try
            {
                //使用二进制作为数据序列化方式，也可以使用SoapClientFormatterSink、SoapServerFormatterSink
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full };
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                Hashtable properties = new Hashtable
                {
                    //指定信道的端口
                    ["portName"] = PortName,
                    ["name"] = "ipc",
                    ["authorizedGroup"] = "Everyone",
                    ["typeFilterLevel"] = TypeFilterLevel.Full
                };

                channel = new IpcChannel(properties, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(channel, false);
                string remoUri = typeof(T).Name;
                WellKnownServiceTypeEntry wellKnown = new WellKnownServiceTypeEntry(typeof(T), remoUri, WellKnownObjectMode.Singleton);

                RemotingConfiguration.RegisterWellKnownServiceType(wellKnown);
                channel.StartListening(null);
                RemoteUrl = $"ipc://{PortName}/{remoUri}";
                ErroMsg = "";
                IsRegisted = true;
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.GetInnerExceptionMessage();
                IsRegisted = false;
                return false;
            }
        }

        /// <summary>
        /// 停止服务（其实没多大用）
        /// </summary>
        /// <returns>
        /// </returns>
        public bool Stop()
        {
            try
            {
                ChannelServices.UnregisterChannel(channel);
                channel?.StopListening(null);

                channel = null;
                ErroMsg = "";
                IsRegisted = false;
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.GetInnerExceptionMessage();
                IsRegisted = false;
                return false;
            }
        }

        #endregion Public 方法
    }
}
#endif