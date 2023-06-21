using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;

using GeneralTool.CoreLibrary.SocketLib.Models;

namespace GeneralTool.CoreLibrary.SocketLib.Packages
{
    /// <summary>
    /// 包含有头部长度的解包程序
    /// </summary>
    // Token: 0x02000096 RID: 150
    public class FixedHeadPackage : IPackage<FixedHeadRecevieState>
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        // Token: 0x1700014B RID: 331
        // (get) Token: 0x06000612 RID: 1554 RVA: 0x0001E4A3 File Offset: 0x0001C6A3
        // (set) Token: 0x06000613 RID: 1555 RVA: 0x0001E4AB File Offset: 0x0001C6AB
        public string Id { get; set; }

        /// <summary>
        /// 包含有头部长度的接收状态包
        /// </summary>
        // Token: 0x1700014C RID: 332
        // (get) Token: 0x06000614 RID: 1556 RVA: 0x0001E4B4 File Offset: 0x0001C6B4
        // (set) Token: 0x06000615 RID: 1557 RVA: 0x0001E4BC File Offset: 0x0001C6BC
        public FixedHeadRecevieState State { get; set; } = new FixedHeadRecevieState();

        /// <summary>
        /// 日志
        /// </summary>
        // Token: 0x1700014D RID: 333
        // (get) Token: 0x06000616 RID: 1558 RVA: 0x0001E4C5 File Offset: 0x0001C6C5
        // (set) Token: 0x06000617 RID: 1559 RVA: 0x0001E4CD File Offset: 0x0001C6CD
        public ILog Log { get; set; }

        /// <summary>
        ///
        /// </summary>
        // Token: 0x06000618 RID: 1560 RVA: 0x0001E4D8 File Offset: 0x0001C6D8
        public FixedHeadPackage()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <inheritdoc />
        // Token: 0x06000619 RID: 1561 RVA: 0x0001E514 File Offset: 0x0001C714
        public void Subpackage(FixedHeadRecevieState state, Action<IEnumerable<byte>, Socket> completePackage)
        {
            byte[] buffer = state.ListBytes.ToArray();
            bool flag = buffer.Length < 4;
            if (flag)
            {
                ILog log = Log;
                log?.Debug("缓冲区长度小于4,不够解包");
            }
            else
            {
                bool flag2 = state.ReceiveNeedLen == 0;
                if (flag2)
                {
                    int headLen = BitConverter.ToInt32(buffer, 0);
                    int curLen = buffer.Length - 4;
                    bool flag3 = curLen < headLen;
                    if (flag3)
                    {
                        ILog log2 = Log;
                        log2?.Debug(string.Format("已获取主体长度为:{0},目前解析到的主体长度为:{1},不够解包", headLen, curLen));
                    }
                    else
                    {
                        IEnumerable<byte> packBuffer = buffer.Skip(4);
                        bool flag4 = packBuffer.Count<byte>() == headLen;
                        if (flag4)
                        {
                            completePackage(packBuffer, state.WorkSocket);
                            state.ListBytes.Clear();
                            state.ReceiveNeedLen = 0;
                        }
                        else
                        {
                            IEnumerable<byte> bodyBuffer = packBuffer.Take(headLen);
                            completePackage(bodyBuffer, state.WorkSocket);
                            state.ListBytes = packBuffer.Skip(headLen).ToList<byte>();
                            state.ReceiveNeedLen = 0;
                            ILog log3 = Log;
                            log3?.Debug(string.Format("当前剩余包长度:{0}", state.ListBytes.Count));
                            Subpackage(state, completePackage);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        // Token: 0x0600061A RID: 1562 RVA: 0x0001E660 File Offset: 0x0001C860
        public byte[] Package(byte[] bytes)
        {
            return SocketCommon.GetBytesAndHeader(bytes);
        }
    }
}
