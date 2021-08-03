using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralTool.General.Ioc;

namespace IocTest
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject<Ilog, ConsoleLog>();
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject(new Controller());
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject(new Test("Test"),nameof(Test.Con));
            SimpleIocSerivce.SimpleIocSerivceInstance.Inject<MainModel>();
            SimpleIocSerivce.SimpleIocSerivceInstance.Start();

            Console.ReadKey();
        }
    }
}
