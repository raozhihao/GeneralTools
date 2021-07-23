﻿using GeneralTool.General.Enums;
using GeneralTool.General.Logs;
using GeneralTool.General.TaskLib;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TaskTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //new SocketClientTest().Test();
             TestManager();
            //TestLog();
            Console.ReadKey();
        }

        private static int count;
        private static void TestLog()
        {
            var watch = new Stopwatch();
            watch.Start();
            var log = new ConsoleLogInfo();
            // log.MaxLength = 1024;
            // log.LogEvent += Log_LogEvent;
            Parallel.For(0, 1000, i =>
            {
                log.Log("测试..." + i);
            });
            watch.Stop();
            Console.WriteLine(watch.Elapsed);

            //for (int i = 0; i < 100000; i++)
            //{

            //    count++;
            //    var index = new Random().Next(0, 5);
            //    var time = DateTime.Now;
            //    switch (index)
            //    {
            //        case 0:
            //            log.Info($"{LogType.Info}");
            //            break;
            //        case 1:
            //            log.Debug($"{LogType.Debug}");
            //            break;
            //        case 2:
            //            log.Error($"{LogType.Error}");
            //            break;
            //        case 3:
            //            log.Waring($"{LogType.Waring}");
            //            break;
            //        case 4:
            //            log.Fail($"{LogType.Fail}");
            //            break;
            //    }
            //}
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
                case LogType.Error:
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
            //Thread.Sleep(300);
        }

        private static void TestManager()
        {
            var @base = new TaskManager(null,null,new HttpServerStation(null));
            var test = new TaskLibs.Test();
            @base.Open("127.0.0.1", 8820, test);

            var dic = @base.GetInterfaces();
            var faces = @base[test];

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