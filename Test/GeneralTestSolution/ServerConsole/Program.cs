using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.TaskLib;

using TaskLibs;

namespace ServerConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var genStation = new FixedHeadSocketStation(null, null);
            var manager=new TaskManager(null,null,genStation);
            var task = new DemoTask();
            manager.Open("127.0.0.1", 8878, task);
            manager.GetInterfaces();
            Console.ReadKey();
        }
    }
}
