using System;

namespace SubConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            LoopArgs();
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("None");
            //    return;
            //}
            //switch (args[0])
            //{
            //    case "1":
            //        Add(1, 2);
            //        break;
            //    case "2":
            //        Test();
            //        break;
            //}
        }

        private static void LoopArgs()
        {
            while (true)
            {
                var cmd = Console.ReadLine();
                if (cmd == "Test")
                {
                    Test();
                }
                else if (cmd.Contains("Add"))
                {
                    var r = new Random();
                    Add(r.Next(1, 1000), r.Next(3000, 5000));
                }
                else if (cmd.Contains("void"))
                {

                }
                else if (cmd.Contains("no"))
                {
                }
                else
                {
                    Console.WriteLine("没有找到对应的命令");
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("End");
                }
            }
        }

        private static void Add(int v1, int v2)
        {
            Console.WriteLine(v1 + v2);
        }

        private static void Test()
        {
            Console.WriteLine("Test");
        }
    }
}
