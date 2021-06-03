using GeneralTool.General.Enums;
using GeneralTool.General.Logs;
using GeneralTool.General.TaskLib;
using System;
using System.Threading;

namespace TaskTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestManager();
           // TestLog();
        }

        private static int count;
        private static void TestLog()
        {
            var log = new FileInfoLog("logTest");
            log.LogEvent += Log_LogEvent;
            for (int i = 0; i < 100000; i++)
            {
                count++;
                var index = new Random().Next(0, 5);
                var time = DateTime.Now;
                switch (index)
                {
                    case 0:
                        log.Info($"{time} Log,{LogType.Info}");
                        break;
                    case 1:
                        log.Debug($"{time} Log,{LogType.Debug}");
                        break;
                    case 2:
                        log.Erro($"{time} Log,{LogType.Erro}");
                        break;
                    case 3:
                        log.Waring($"{time} Log,{LogType.Waring}");
                        break;
                    case 4:
                        log.Fail($"{time} Log,{LogType.Fail}");
                        break;
                }
            }
        }

        private static void Log_LogEvent(object sender, GeneralTool.General.Models.LogMessageInfo e)
        {
            if (count > 100)
            {
                count = 0;
                Console.Clear();
            }
            switch (e.LogType)
            {
                case LogType.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogType.Debug:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogType.Erro:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Waring:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.Fail:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }
            Console.WriteLine($"{sender} {e.Msg} {e.LogType}");
            Thread.Sleep(300);
        }

        private static void TestManager()
        {
            var @base = new TaskManager();
            var test = new TaskLibs.Test();
            @base.Open("127.0.0.1", 8820, test);

            var dic = @base.GetInterfaces();

            var item = dic["Test/Add"];
            item.Paramters[0].Value = 22;
            item.Paramters[1].Value = 33;
            var result = @base.DoInterface("Test/Add", item);
            Console.WriteLine($"result : {result}");

            item = dic["Test/SayHello"];
            result = @base.DoInterface("Test/SayHello", item);
            Console.WriteLine($"result : {result}");
            Console.ReadKey();
        }
    }
}
