using System;

using ClassLibrary;

using GeneralTool.General.IPCHelper;

namespace Server
{
    class IPCServer
    {
        public void StartUp()
        {
            var helper = new IPCServerHelper<IPCModel>();
            helper.RegisterServer();
            Console.WriteLine("Registed");
            Console.ReadKey();
        }
    }
}
