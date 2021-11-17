using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerProxy
    {
        public void Startup()
        {
            var server = new GeneralTool.General.SocketHelper.ServerHelper();
            var re= server.RegisterClass<ClassLibrary.ISocketClass>(new ClassLibrary.SocketClass());
            if (!re)
            {
                Console.WriteLine("Register error");
                return;
            }

            server.Start();

        }
    }
}
