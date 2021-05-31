﻿using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;

namespace GeneralTool.General.IPCHelper
{
    /// <summary>
    /// IPC服务端帮助类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IPCServerHelper<T> : IDisposable where T : MarshalByRefObject
    {
        /// <summary>
        /// 获取或设备服务端URL
        /// </summary>
        public string RemoteUrl { get; private set; }
        /// <summary>
        /// 获取或设备服务端口
        /// </summary>
        public string PortName { get; private set; }
        /// <summary>
        /// 获取错误信息
        /// </summary>

        public string ErroMsg { get; private set; }
        /// <summary>
        /// 是否已注册
        /// </summary>
        public bool IsRegisted { get; private set; }

        private IpcChannel channel;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName">端口名称</param>
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

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <returns></returns>
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
                    ["authorizedGroup"] = "Everyone"
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
                ErroMsg = ex.Message;
                IsRegisted = false;
                return false;
            }
        }

        /// <summary>
        /// 停止服务（其实没多大用）
        /// </summary>
        /// <returns></returns>
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
                ErroMsg = ex.Message;
                IsRegisted = false;
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        ~IPCServerHelper()
        {
            Stop();
        }
    }
}