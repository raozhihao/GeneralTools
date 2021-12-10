using System;
using System.Collections.Generic;
using System.Net.Sockets;

using GeneralTool.General.Interfaces;
using GeneralTool.General.SocketLib.Models;

namespace GeneralTool.General.SocketLib.Interfaces
{
    /// <summary>
    /// 指示该如何解包
    /// </summary>
    public interface IPackage<T> where T : ReceiveState, new()
    {
        /// <summary>
        /// 要挂载的数据类型
        /// </summary>
        T State { get; set; }
        /// <summary>
        /// 指示该如何解包
        /// </summary>
        /// <param name="state">传递的挂载类型</param>
        /// <param name="completePackage">该委托指定将一个完整的数据包返回给服务端进行处理,如果不返回,请自行处理</param>
        void Subpackage(T state, Action<IEnumerable<byte>, Socket> completePackage);
        /// <summary>
        /// 指示该如何封包
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        byte[] Package(byte[] buffer);

        /// <summary>
        /// 日志
        /// </summary>
        ILog Log { get; set; }
    }
}
