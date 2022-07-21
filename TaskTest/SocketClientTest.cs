using GeneralTool.General.SocketHelper;
using System;
using System.Text;

namespace TaskTest
{
    class SocketClientTest
    {
        public void Test()
        {
            var clinet = new ClientSocketBase(port: 8878);
            clinet.Start();
            while (true)
            {
                try
                {
                    var result = clinet.Send(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                    Console.WriteLine($"Server Reveice:{Encoding.UTF8.GetString(result.ToArray())}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }
    }
}
