﻿using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// Socket扩展类
    /// </summary>
    public static class SocketExtensions
    {
        /// <summary>
        /// 确定当前Socket是否还处于连接状态
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static bool IsClientConnected(this Socket socket)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation c in tcpConnections)
            {
                TcpState stateOfConnection = c.State;

                if (c.LocalEndPoint.Equals(socket.LocalEndPoint) && c.RemoteEndPoint.Equals(socket.RemoteEndPoint))
                {
                    if (stateOfConnection == TcpState.Established)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 设置Socket为长连接
        /// </summary>
        /// <param name="socket"></param>
        public static void SetSocketKeepAlive(this Socket socket)
        {
            uint dummy = 0;
            byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)3000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//keep-alive间隔
            BitConverter.GetBytes((uint)500).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);// 尝试间隔

            socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
        }
    }
}