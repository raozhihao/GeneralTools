using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.SocketHelper;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServerProxy().Startup();
           // Test();
            //var server = new ServerSocketBase("192.168.12.102", 8899);
            //server.ClientConnetedEvent += Server_ClientConnetedEvent;
            //server.ClientDisconnectedEvent += Server_ClientDisconnectedEvent;
            //server.RecevieEvent += Server_RecevieEvent;
            //server.Start();

            Console.WriteLine("Server start...");
            Console.ReadKey();
        }


        public static int SendBytes( byte[] buffer)
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
                    var len = GeneralTool.General.RandomEx.Next(1,lenSize);

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
                return sum;
            }
        }
        private static void Test()
        {
            var buffer = new byte[100];
            for (byte i = 0; i < 100; i++)
            {
                buffer[i] = i;
            }

            var tmpBuffer = new byte[100];
            SendBytes(buffer);
        }

        private static void Server_RecevieEvent(object sender,SocketPackage obj)
        {
            var package = obj;
            var len = package.PackageLen;
            var buffer = package.Buffer;
            File.WriteAllBytes("1", buffer.ToArray());
          
            Console.WriteLine("Recevier ok");
        }

        private static void Server_ClientDisconnectedEvent(System.Net.Sockets.Socket obj)
        {
            Console.WriteLine($"Client disconnected");
        }

        private static void Server_ClientConnetedEvent(System.Net.Sockets.Socket obj)
        {
            Console.WriteLine($"Client connected");
        }
    }
}
