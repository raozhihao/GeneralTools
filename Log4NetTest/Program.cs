using GeneralTool.General.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Log4NetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            watch.Start();
            var log = new LogLib("logTest");
            //log.MaxLength = 1024;
            log.LogEvent += Log_LogEvent;
            Parallel.For(0, 1000, i =>
            {
                log.Log("测试..." + i);
            });
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
            Console.ReadKey();
        }

        private static void Log_LogEvent(object sender, LogMessageInfo e)
        {

        }
    }
}
