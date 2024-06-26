﻿using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace GeneralTool.CoreLibrary.SocketLib
{
    /// <summary>
    /// Socket扩展类
    /// </summary>
    public static class SocketExtensions
    {
        #region Public 方法

        /// <summary>
        /// 确定当前Socket是否还处于连接状态
        /// </summary>
        /// <param name="socket">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsClientConnected(this Socket socket,string localEndPoint, string remoteEndPoint)
        {
            if (socket == null || !socket.Connected)
                return false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation c in tcpConnections)
            {
                TcpState stateOfConnection = c.State;

                try
                {
                    if ((c.LocalEndPoint+"").Equals(localEndPoint) && (c.RemoteEndPoint+"").Equals(remoteEndPoint))
                    {

                        /* 项目“GeneralTool.CoreLibrary (net452)”的未合并的更改
                        在此之前:
                                                if (stateOfConnection == TcpState.Closed || stateOfConnection == TcpState.CloseWait)
                                                {
                                                    return false;
                                                }
                                                else
                                                {
                                                    return true;
                                                }
                                            }
                        在此之后:
                                                if (stateOfConnection != TcpState.Closed && stateOfConnection != TcpState.CloseWait;
                                            }
                        */
                        return stateOfConnection != TcpState.Closed && stateOfConnection != TcpState.CloseWait;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// 设置Socket为长连接
        /// </summary>
        /// <param name="socket">
        /// </param>
        public static void SetSocketKeepAlive(this Socket socket)
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//keep-alive间隔
            BitConverter.GetBytes((uint)500).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);// 尝试间隔

            _ = socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        }

        #endregion Public 方法
    }
}
