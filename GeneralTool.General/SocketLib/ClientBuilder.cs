
using System;

using GeneralTool.General.Interfaces;
using GeneralTool.General.SocketLib.Interfaces;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.SocketLib.Packages;

namespace GeneralTool.General.SocketLib
{
    /// <summary>
    /// 简单客户端端创建
    /// </summary>
    public static class SimpleClientBuilder
    {
        /// <summary>
        /// 创建无任何解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketClient<ReceiveState> CreateNoSubPack(ILog log = null)
        {
            return new SocketClient<ReceiveState>(log);
        }

        /// <summary>
        /// 创建以\r\n为结尾解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketClient<ReceiveState> CreateCommandSubPack(ILog log = null)
        {
            var server = new SocketClient<ReceiveState>(log)
            {
                Package = new Func<IPackage<ReceiveState>>(() => new CommandLinePackage())
            };
            return server;
        }

        /// <summary>
        /// 创建自定义末尾解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketClient<ReceiveState> CreateCustomCommandSubPack(string cmd, ILog log = null)
        {
            var server = new SocketClient<ReceiveState>(log)
            {
                Package = new Func<IPackage<ReceiveState>>(() => new CustomCommandPackage(cmd))
            };
            return server;
        }

        /// <summary>
        /// 创建自定义末尾解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketClient<ReceiveState> CreateCustomCommandSubPack(byte[] cmd, ILog log = null)
        {
            var server = new SocketClient<ReceiveState>(log)
            {
                Package = new Func<IPackage<ReceiveState>>(() => new CustomCommandPackage(cmd))
            };
            return server;
        }

        /// <summary>
        /// 创建将包长度放置包头解析协议的服务端
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static SocketClient<FixedHeadRecevieState> CreateFixedCommandSubPack(ILog log = null)
        {
            var server = new SocketClient<FixedHeadRecevieState>(log)
            {
                Package = new Func<Interfaces.IPackage<FixedHeadRecevieState>>(() => new FixedHeadPackage())
            };
            return server;
        }


    }
}
