using GeneralTool.General.Ioc;
using System;

namespace IOCTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = SimpleIocSerivce.SimpleIocSerivceInstance;
            ioc.Inject<MainClass>();
            ioc.Inject<ILog, ConsoleLog>();
            ioc.Start();
            var main = ioc.Resolve<MainClass>();
            main.Info("main");
            Console.ReadKey();
        }
    }
}
