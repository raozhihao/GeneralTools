using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

using GeneralTool.CoreLibrary.Interfaces;
using GeneralTool.CoreLibrary.SocketLib.Interfaces;

using GeneralTool.CoreLibrary.SocketLib.Models;

namespace GeneralTool.CoreLibrary.SocketLib.Packages
{
    /// <summary>
    /// 自定义后缀解包程序
    /// </summary>
    // Token: 0x02000095 RID: 149
    public class CustomCommandPackage : IPackage<ReceiveState>
    {
        /// <inheritdoc />
        // Token: 0x17000149 RID: 329
        // (get) Token: 0x06000609 RID: 1545 RVA: 0x0001E193 File Offset: 0x0001C393
        // (set) Token: 0x06000 RID: 1546 RVA: 0x0001E19B File Offset: 0x0001C39B
        public ReceiveState State { get; set; } = new ReceiveState();

        /// <summary>
        /// 日志
        /// </summary>
        // Token: 0x1700014A RID: 330
        // (get) Token: 0x0600060B RID: 1547 RVA: 0x0001E1A4 File Offset: 0x0001C3A4
        // (set) Token: 0x0600060C RID: 1548 RVA: 0x0001E1AC File Offset: 0x0001C3AC
        public ILog Log { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buffer">要查询的末尾字节数组</param>
        // Token: 0x0600060D RID: 1549 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
        public CustomCommandPackage(byte[] buffer)
        {
            this.buffer = buffer;
            ILog log = Log;
            log?.Debug(string.Format("处理程序 {0} : buffer: {1}", this, Encoding.UTF8.GetString(buffer)));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="commandLine">要查询的末尾字符串</param>
        // Token: 0x0600060E RID: 1550 RVA: 0x0001E207 File Offset: 0x0001C407
        public CustomCommandPackage(string commandLine) : this(Encoding.UTF8.GetBytes(commandLine))
        {
            ILog log = Log;
            log?.Debug(string.Format("处理程序 {0} : commandLine: {1}", this, commandLine));
        }

        /// <inheritdoc />
        // Token: 0x0600060F RID: 1551 RVA: 0x0001E23C File Offset: 0x0001C43C
        public void Subpackage(ReceiveState state, Action<IEnumerable<byte>, Socket> completePackage)
        {
            bool flag = state.ListBytes.Count == 0;
            if (!flag)
            {
                bool flag2 = buffer == null || buffer.Length == 0;
                if (flag2)
                {
                    string msg = "CustomCommandSubPackage中不可设置buffer为空";
                    throw new Exception(msg);
                }
                List<byte> list = state.ListBytes;
                bool flag3 = list.Count < 2;
                if (flag3)
                {
                    ILog log = Log;
                    log?.Debug("缓冲区长度小于2,不够解包");
                }
                else
                {
                    List<byte> bigTmp = list;
                    int PreIndex = 0;
                    int PreCount = 0;
                    for (; ; )
                    {
                        int index = list.IndexOf(buffer[0], PreIndex);
                        bool flag4 = index == -1;
                        if (flag4)
                        {
                            break;
                        }
                        int endIndex = GetEndIndex(list, buffer, index);
                        bool flag5 = endIndex == -1;
                        if (flag5)
                        {
                            int count = index + 1;
                            PreIndex = count;
                            ILog log2 = Log;
                            log2?.Debug("缓冲区中未找全buffer,不够解包");
                        }
                        else
                        {
                            int count = endIndex;
                            IEnumerable<byte> tmp = list.Skip(PreCount).Take(count - PreCount - buffer.Length);
                            PreCount = count;
                            PreIndex = count;
                            bigTmp = list.Skip(count).ToList<byte>();
                            completePackage(tmp, state.WorkSocket);
                            ILog log3 = Log;
                            log3?.Debug(string.Format("完整包长度:{0},剩余包长度:{1}", tmp.Count<byte>(), bigTmp.Count));
                        }
                    }
                    ILog log4 = Log;
                    log4?.Debug("缓冲区中未找到指定buffer开始下标,不够解包");
                    state.ListBytes = bigTmp;
                }
            }
        }

        // Token: 0x06000610 RID: 1552 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
        private int GetEndIndex(List<byte> big, byte[] small, int index)
        {
            int i = 0;
            while (i < small.Length)
            {
                bool flag = index == big.Count;
                int result;
                if (flag)
                {
                    result = -1;
                }
                else
                {
                    bool flag2 = big[index] != small[i];
                    if (!flag2)
                    {
                        index++;
                        i++;
                        continue;
                    }
                    result = -1;
                }
                return result;
            }
            return index;
        }

        /// <inheritdoc />
        // Token: 0x06000611 RID: 1553 RVA: 0x0001E42C File Offset: 0x0001C62C
        public byte[] Package(byte[] bytes)
        {
            ILog log = Log;
            log?.Debug("开始压包");
            int len = bytes.Length;
            byte[] newBuffer = new byte[bytes.Length + buffer.Length];
            bytes.CopyTo(newBuffer, 0);
            Array.Copy(buffer, 0, newBuffer, len, buffer.Length);
            ILog log2 = Log;
            log2?.Debug("压包完成");
            return newBuffer;
        }

        // Token: 0x0400022A RID: 554
        private readonly byte[] buffer;
    }
}
