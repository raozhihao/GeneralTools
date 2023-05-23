using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.TaskLib;

namespace ConsoleFram.TaskTests
{
    public class NoPackTask
    {
        public void Test()
        {
            var manager = new TaskManager(null, null, null);
            var task = new TestTask();
            var re = manager.Open("127.0.0.1", 8877, task);

            Console.ReadKey();
        }
    }
}
