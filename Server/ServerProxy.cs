using System;

namespace Server
{
    class ServerProxy
    {
        public void Startup()
        {
            var server = new GeneralTool.General.SocketHelper.ServerHelper();
            var re = server.RegisterClass<ClassLibrary.ISocketClass>(new ClassLibrary.SocketClass());
            if (!re)
            {
                Console.WriteLine("Register error");
                return;
            }

            server.Start();

        }
    }
}
