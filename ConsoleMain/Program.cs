using GeneralTool.General.ProcessHelpers;
using System;

namespace ConsoleMain
{
    class Program
    {
        static void Main(string[] args)
        {
            var exe = @"C:\Code\GitCode\GeneralTools\SubConsole\bin\Debug\SubConsole.exe";
            //var exe = @"C:\Code\DemoSolution\App_Debug\MainWindowLib.exe";
            //var exe = "cmd.exe";

            var process = new ProcessEngine();
            process.Run(exe);
            process.ProcessExitEvent += Process_ProcessExit;
            while (true)
            {
                string result = "";
                var line = Console.ReadLine();
                if (line == "q")
                {
                    return;
                }
                else
                   // result = ProcessHelper.Run(exe, line, 30);
                    result = process.WriteLine(line);

                Console.WriteLine("获取到返回:" + result);
            }

        }

        private static void Process_ProcessExit()
        {
            Console.WriteLine("子进程已退出");
        }
    }
}
