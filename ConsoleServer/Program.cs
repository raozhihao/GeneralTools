using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new GeneralTool.General.SocketHelper.Server.ServerHelper("127.0.0.1", 33455);
            server.RegisterClass<ITestLib>(new TestLib());
            server.Start();
            Console.ReadKey();
        }
    }
}
