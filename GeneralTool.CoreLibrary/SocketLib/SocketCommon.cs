using System;
using System.Net.Sockets;
using System.Text;

namespace GeneralTool.CoreLibrary.SocketLib
{
    /// <summary>
    /// 
    /// </summary>
    public static class SocketCommon
    {
        /// <summary>
        /// 获取由字符串返回的数据缓冲区,该缓冲区将数据长度一并写入头部
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] GetBytesAndHeader(string msg)
        {
            //获取数据字节数组
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            return GetBytesAndHeader(buffer);
        }

        /// <summary>
        /// 获取由字符串返回的数据缓冲区,该缓冲区将数据长度一并写入头部
        /// </summary>
        /// <returns></returns>
        public static byte[] GetBytesAndHeader(byte[] buffer)
        {
            int blen = buffer.Length;
            //组织数据长度写入包头
            byte[] bitLen = GetHeadBytes(blen);
            //2.组织数据
            byte[] sendBytes = new byte[blen + 4];
            bitLen.CopyTo(sendBytes, 0);
            Array.Copy(buffer, 0, sendBytes, 4, buffer.Length);
            return sendBytes;
        }

        /// <summary>
        /// 获取头部数据
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] GetHeadBytes(int len)
        {
            return BitConverter.GetBytes(len);
        }

        /// <summary>
        /// 发送字符串数据,且在头部伴有数据长度
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool SendMessage(string msg, Socket client)
        {
            return SendBytes(GetBytesAndHeader(msg), client);
        }

        /// <summary>
        /// 发送字节数组数据
        /// </summary>
        /// <param name="sendBytes"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool SendBytes(byte[] sendBytes, Socket client)
        {
            //循环发送
            //设定已发送长度
            int sendedLen = 0;
            //设定需要发送长度
            int needSendLen = sendBytes.Length;
            //当需要发送长度大于0时需要一直发送
            while (needSendLen > 0)
            {
                //获取到已发送长度
                //在此处将循环发送,偏移量就是已发送的长度,该值将会一直叠加
                //同时需要发送长度会一直减少,直至减为0
                int len = client.Send(sendBytes, sendedLen, needSendLen, SocketFlags.None);
                if (len == 0)
                {
                    //未发送成功,则继续发送
                    continue;
                }
                else if (len == -1)
                {
                    //发送失败,则返回
                    return false;
                }

                //重置已发送长度
                needSendLen -= len;
                sendedLen += len;
                //如果不够,将继续循环发送
            }
            return true;
        }

        /// <summary>
        /// 根据头部数据长度来接收数据
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static bool ReceiveBytesForHead(Socket client, out byte[] buffer)
        {
            buffer = default;
            //3.得到包头
            byte[] headBuffer = new byte[4];
            bool reBool = ReceiveBytes(headBuffer, client);
            if (!reBool)
            {
                //没有接收成功
                return reBool;
            }
            //4.解包数据
            //得到接收长度
            int bufferSize = BitConverter.ToInt32(headBuffer, 0);
            Console.WriteLine("接收到数据长度为:" + bufferSize);
            buffer = new byte[bufferSize];
            reBool = ReceiveBytes(buffer, client);
            return reBool;
        }

        /// <summary>
        /// 接收数据到字节数组中
        /// </summary>
        /// <param name="bitLen"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static bool ReceiveBytes(byte[] bitLen, Socket client)
        {
            //循环接收
            //设定已接收长度
            int reLen = 0;
            //设定需要接收长度
            int needLen = bitLen.Length;
            //循环接收
            while (needLen > 0)
            {
                int len = client.Receive(bitLen, reLen, needLen, SocketFlags.None);
                if (len == 0)
                {
                    //未接收完成,继续接收
                    continue;
                }
                else if (len == -1)
                {
                    //接收失败,返回
                    return false;
                }

                //重置变量
                needLen -= len;
                reLen += len;
            }
            return true;
        }
    }
}
