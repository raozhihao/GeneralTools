using GeneralTool.General.Attributes;
using GeneralTool.General.TaskLib;
using System;

namespace TaskTest.TaskLibs
{
    [Route("Test/")]
    class Test : BaseTaskInvoke
    {

        [Route(nameof(ConsoleTest), "测试")]
        public void ConsoleTest([WaterMark("消息")] string msg)
        {
            Console.WriteLine(msg);
        }

        [Route(nameof(Add), "测试")]
        public int Add(int x, int y)
        {
            return x + y;
        }

        //这个方法会被跳过
        [Route(nameof(Add), "测试")]
        public int Add(int x, int y, int z)
        {
            return x + y;
        }

        [Route(nameof(SayHello), "SayHello")]
        public string SayHello(string name = "John")
        {
            return "Hello" + name;
        }
    }
}
