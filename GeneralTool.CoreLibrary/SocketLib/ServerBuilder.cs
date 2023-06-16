using System;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Models;
using GeneralTool.CoreLibrary.SocketLib.Packages;

namespace GeneralTool.CoreLibrary.SocketLib
{
    /// <summary>
    /// 简单服务端创建
    /// </summary>
    public static class SimpleServerBuilder
    {
        /// <summary>
        /// 创建无任何解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketServer<ReceiveState> CreateNoPack(ILog log = null)
        {
            return new SocketServer<ReceiveState>(log);
        }

        /// <summary>
        /// 创建以\r\n为结尾解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketServer<ReceiveState> CreateCommandPack(ILog log = null)
        {
            SocketServer<ReceiveState> server = new SocketServer<ReceiveState>(log)
            {
                PackageFunc = new Func<IPackage<ReceiveState>>(() => new CommandLinePackage())
            };
            return server;
        }

        /// <summary>
        /// 创建自定义末尾解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketServer<ReceiveState> CreateCustomCommandPack(string cmd, ILog log = null)
        {
            SocketServer<ReceiveState> server = new SocketServer<ReceiveState>(log)
            {
                PackageFunc = new Func<IPackage<ReceiveState>>(() => new CustomCommandPackage(cmd))
            };
            return server;
        }

        /// <summary>
        /// 创建自定义末尾解析协议的服务端
        /// </summary>
        /// <returns></returns>
        public static SocketServer<ReceiveState> CreateCustomCommandPack(byte[] cmd, ILog log = null)
        {
            SocketServer<ReceiveState> server = new SocketServer<ReceiveState>(log)
            {
                PackageFunc = new Func<IPackage<ReceiveState>>(() => new CustomCommandPackage(cmd))
            };
            return server;
        }

        /// <summary>
        /// 创建将包长度放置包头解析协议的服务端
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static SocketServer<FixedHeadRecevieState> CreateFixedCommandPack(ILog log = null)
        {
            SocketServer<FixedHeadRecevieState> server = new SocketServer<FixedHeadRecevieState>(log)
            {
                PackageFunc = new Func<IPackage<FixedHeadRecevieState>>(() => new FixedHeadPackage())
            };
            return server;
        }

    }
}
