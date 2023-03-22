using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.Attributes;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.TaskLib;

namespace ServerTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var station = new GenServerStation<ReceiveState>(null, null, null);
            var manager = new TaskManager(null, null, station);

            manager.Open("127.0.0.1", 8877, new TaskTest());
            var interfaces = manager.GetInterfaces();

            Console.ReadKey();
        }
    }

    [Route(nameof(TaskTest) + "/")]
    public class TaskTest : BaseTaskInvoke
    {
        [Route(nameof(TaskTest), SortIndex = 1)]
        public string GuidStr()
        {
            return Guid.NewGuid().ToString();
        }

        [Route(nameof(ATest), SortIndex = 0)]
        public void ATest()
        {

        }

        public void Test()
        {

        }
    }
}
