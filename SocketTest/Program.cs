using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.TaskLib;

namespace SocketTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new FixedHeadSocketClient(null, null);
            client.Client.ReceiveBufferSize = 1024 * 1024 * 10;
            client.ReadTimeOut = 18000;

            

            while (true)
            {
                client.Startup("127.0.0.1", 8877);
                client.Send(new ServerRequest()
                {
                     Url= "Task/TestString"
                }, System.Threading.CancellationToken.None);
                System.Threading.Thread.Sleep(100);
            }
            client.Dispose();
        }
    }
}
