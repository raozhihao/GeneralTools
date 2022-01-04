using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;

using GeneralTool.General.SocketLib;

namespace GeneralTool.General.SocketHelper
{
    /// <summary>
    /// 包
    /// </summary>
    public class SocketPackage
    {
        /// <summary>
        /// 包长度
        /// </summary>
        public int PackageLen { get; set; }

        /// <summary>
        /// 包主体
        /// </summary>
        public List<byte> Buffer { get; set; } = new List<byte>();

        /// <summary>
        /// 是否出错
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// 出错消息
        /// </summary>
        public Exception ErroMsg { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SocketCommon
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public Exception ErroException { get; private set; }
        private const int BufferSize = 65536;

        /// <summary>
        /// 读取数据长度
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public int ReadHeadSize(Socket socket)
        {
            var headBuffer = new byte[4];
            int len = this.Read(socket, headBuffer);
            if (len != 4)
            {
                return -1;
            }

            var headLen = BitConverter.ToInt32(headBuffer, 0);

            Trace.WriteLine($"{DateTime.Now} : 接收到长度数据   Recevier size : {len}");
            return headLen;
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="size">需要读取的数量</param>
        /// <returns></returns>
        public SocketPackage Read(Socket clientSocket, int size)
        {
            var package = new SocketPackage() { IsError = false };
            package.PackageLen = size;

            //计算需要多大的缓冲区才好
            var bufferSize = size > BufferSize ? BufferSize : size;
            LogMsg($"Read   this.bufferSize : {bufferSize}");

            //需要读取的次数,根据buffer来的
            var count = size / bufferSize;//需要循环取的次数
            var quite = size % bufferSize;//最后要读取的剩余量

            LogMsg($"Read  count : {count} , quite : {quite}");
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    if (!clientSocket.IsClientConnected())
                    {
                        this.ErroException = new Exception("Cient is not connected");
                        return new SocketPackage() { IsError = true, ErroMsg = this.ErroException };
                    }

                    var buffer = new byte[bufferSize];
                    int tmpLen = Read(clientSocket, buffer);
                    if (tmpLen != buffer.Length)
                    {
                        //失败
                        return new SocketPackage() { IsError = true, ErroMsg = this.ErroException };
                    }

                    //加入
                    package.Buffer.AddRange(buffer);
                    buffer = null;
                }
            }
            //查看剩余量是否有
            if (quite > 0)
            {
                var buffer = new byte[quite];
                int tmpLen = Read(clientSocket, buffer);
                if (tmpLen != buffer.Length)
                {
                    //失败
                    return new SocketPackage() { IsError = true, ErroMsg = this.ErroException };
                }

                //加入
                package.Buffer.AddRange(buffer);
                buffer = null;
            }

            LogMsg("当前包接收完成了,共接收了数据:" + package.Buffer.Count);
            //全部读取成功
            return package;
        }

        private int Read(Socket clientSocket, byte[] buffer)
        {
            var length = buffer.Length;
            var sumCount = 0;
            try
            {
                var tmpBuffer = new byte[length];
                while (true)
                {
                    if (!clientSocket.IsClientConnected())
                    {
                        return sumCount;
                    }

                    //每次接收的数据量不一定刚好,所以要不断的去接收
                    int len = clientSocket.Receive(tmpBuffer, tmpBuffer.Length, SocketFlags.None);

                    //将新读取到的数据复制到buffer中,例如上一次拷贝进去了10个,那么它最后一个下标就是总读取数-1
                    Array.Copy(tmpBuffer, 0, buffer, sumCount, len);
                    sumCount += len;//每次接收进行累加
                                    //继续读取

                    if (sumCount == length)
                    {
                        break;
                    }
                    else if (len == 0)
                    {
                        return sumCount;
                    }
                    else
                    {
                        //并没有接收完,继续接收,例如buffer.Length是100,而sumCount是50,则只需要接收下面的50就行
                        tmpBuffer = new byte[length - sumCount];
                        continue;
                    }
                }

            }
            catch (Exception ex)
            {
                this.ErroException = ex;
                return sumCount;
            }

            if (sumCount != length)
            {

            }

            return sumCount;
        }

        private void LogMsg(string msg)
        {
            Trace.WriteLine($"{DateTime.Now} : {msg}");
        }

        /// <summary>
        /// 发送数据,该方法会将数据长度发送在最前面
        /// </summary>
        /// <param name="Socket"></param>
        /// <param name="buffer"></param>
        /// <param name="progress"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public int Send(Socket Socket, byte[] buffer, Action<double> progress = null, Action<string> log = null)
        {
            //获取长度
            int len = buffer.Length;

            log?.Invoke("开始发送长度数据:" + len);
            var lenBuffer = BitConverter.GetBytes(len);
            //先发送长度
            var lenTmp = this.SendBuffer(Socket, lenBuffer);
            if (lenTmp == -1)
            {
                return -1;
            }

            log?.Invoke("长度数据发送完成,开始发送主体数据");

            //再发送主体
            lenTmp = this.SendBuffer(Socket, buffer, progress, log);
            log?.Invoke("主体数据发送完成,共发送:" + lenTmp);
            if (lenTmp == -1)
            {
                return -1;
            }
            return lenTmp;

        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="serverSocket"></param>
        /// <param name="bytes"></param>
        /// <param name="progress">进度</param>
        /// <param name="log"></param>
        /// <returns></returns>
        public int SendBuffer(Socket serverSocket, byte[] bytes, Action<double> progress = null, Action<string> log = null)
        {
            //分段传输
            //计算需要多大的缓冲区才好
            var size = bytes.Length;
            var bufferSize = size > BufferSize ? BufferSize : size;
            Trace.WriteLine($"Send  this.bufferSize : {bufferSize}");

            //需要读取的次数,根据buffer来的
            var count = size / bufferSize;//需要循环取的次数
            var quiteSize = size % bufferSize;//最后要读取的剩余量

            log?.Invoke($"Send  count : {count} , quite : {quiteSize}");

            var sum = 0;

            var sumCount = quiteSize > 0 ? count + 1 : count;
            log?.Invoke($"发送数据长度:{bufferSize},需要次数:{sumCount}");
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    //Console.WriteLine($"Index : {i}, skip : {i * bufferSize} ,take {bufferSize}, sum : {sum}");
                    //更新buffer
                    var buffer = bytes.Skip(i * bufferSize).Take(bufferSize).ToArray();
                    if (!serverSocket.IsClientConnected())
                    {
                        this.ErroException = new Exception("Cient is not connected");
                        return sum;
                    }

                    int tmpLen = SendBytes(serverSocket, buffer);
                    sum += tmpLen;
                    if (tmpLen != buffer.Length)
                    {
                        //失败
                        return sum;
                    }
                    buffer = null;
                    progress?.Invoke((i + 1) * 1.0 / sumCount);
                }
            }
            //查看剩余量是否有
            if (quiteSize > 0)
            {
                var buffer = bytes.Skip(sum).ToArray();

                log?.Invoke($"进入quite,发送长度:{buffer.Length}");
                int tmpLen = SendBytes(serverSocket, buffer);
                sum += tmpLen;
                log?.Invoke($"总计发送长度:{sum}");
                if (tmpLen != buffer.Length)
                {
                    //失败
                    return sum;
                }
                buffer = null;
                progress?.Invoke(1);
            }

            log?.Invoke("本次已发送完成");

            return sum;
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public int SendBytes(Socket socket, byte[] buffer)
        {
            var lenSize = buffer.Length;
            var sum = 0;//保存已发送字节总数
            var lenTmp = lenSize;
            try
            {
                var bufTmp = new byte[lenSize];
                //将数据拷贝进去
                Array.Copy(buffer, 0, bufTmp, sum, lenTmp);
                while (true)
                {

                    //当前已发送字节数
                    var len = socket.Send(bufTmp, bufTmp.Length, SocketFlags.None);

                    sum += len;
                    if (len == 0)
                    {
                        //没有发送成功
                        return sum;
                    }

                    //如果当前已发送总字节数比原定的要少
                    if (sum < lenSize)
                    {
                        Trace.WriteLine("Send  数据字节数未发够,继续发送");
                        //发送的数据不够,继续发送,例如需要发送10个字节,但上几次总共就发了5个,则还剩下5个未发
                        //则从下标为5的地方再拷贝原始数据到tmp中发送

                        var tmpCount = lenSize - sum;//未发送完成的
                        bufTmp = new byte[tmpCount];//剩下未发送完的
                        Array.Copy(buffer, sum, bufTmp, 0, tmpCount);
                    }
                    else if (sum == lenSize)
                    {
                        //一致,则退出
                        break;
                    }
                }

                return sum;
            }
            catch (Exception ex)
            {
                this.ErroException = ex;
                return sum;
            }
        }
    }

    /// <summary>
    /// Socket错误
    /// </summary>
    public class SocketClientErroArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="clientSocket"></param>
        public SocketClientErroArgs(Exception exception, Socket clientSocket)
        {
            Exception = exception;
            ClientSocket = clientSocket;
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Socket ClientSocket { get; set; }

    }
}
