using System;
using System.Collections.Generic;
using System.Net.Sockets;

using GeneralTool.General.Interfaces;
using GeneralTool.General.SocketLib.Interfaces;

using GeneralTool.General.SocketLib.Models;

namespace GeneralTool.General.SocketLib.Packages
{
    /// <summary>
    /// 不对数据做任何解析操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    // Token: 0x02000097 RID: 151
    public class NoPackage<T> : IPackage<T> where T : ReceiveState, new()
    {
        /// <inheritdoc />
        // Token: 0x1700014E RID: 334
        // (get) Token: 0x0600061B RID: 1563 RVA: 0x0001E678 File Offset: 0x0001C878
        // (set) Token: 0x0600061C RID: 1564 RVA: 0x0001E680 File Offset: 0x0001C880
        public T State { get; set; } = Activator.CreateInstance<T>();

        /// <inheritdoc />
        // Token: 0x0600061D RID: 1565 RVA: 0x0001E68C File Offset: 0x0001C88C
        public byte[] Package(byte[] buffer)
        {
            return buffer;
        }

        /// <summary>
        /// 日志
        /// </summary>
        // Token: 0x1700014F RID: 335
        // (get) Token: 0x0600061E RID: 1566 RVA: 0x0001E69F File Offset: 0x0001C89F
        // (set) Token: 0x0600061F RID: 1567 RVA: 0x0001E6A7 File Offset: 0x0001C8A7
        public ILog Log { get; set; }

        /// <inheritdoc />
        // Token: 0x06000620 RID: 1568 RVA: 0x0001E6B0 File Offset: 0x0001C8B0
        public void Subpackage(T state, Action<IEnumerable<byte>, Socket> completePackage)
        {
            completePackage(state.ListBytes.ToArray(), state.WorkSocket);
            List<byte> listBytes = state.ListBytes;
            listBytes?.Clear();
        }
    }
}
