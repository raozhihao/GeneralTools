using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class TestSocketServer
    {
        public void Test()
        {
            var server = new GeneralTool.General.SocketHelper.Server.ServerHelper();
            server.RegisterClass<TestMethodClass>();
            server.SerializeType = GeneralTool.General.SocketHelper.SerializeType.Json;
            server.Start();
            Console.WriteLine("Server start on port : 55155");
            Console.ReadKey();
        }
    }

    class TestMethodClass
    {
        public string GetResult()
        {
            return Guid.NewGuid().ToString();
        }

        public int GetInt()
        {
            return new Random().Next(1000);
        }

        public string Say(string name)
        {
            return $"Hello {name}";
        }

        public void Test()
        {
            Console.WriteLine("Test");
        }

        public IEnumerable<int> GetRangs(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return i;
            }
        }

        public GeneralTool.General.Models.ResponseCommand GetResponseCommand()
        {
            return new GeneralTool.General.Models.ResponseCommand();
        }

        public GeneralTool.General.Models.ResponseCommand GetResponseCommand(Info info)
        {
            Console.WriteLine(info.Text);
            return new GeneralTool.General.Models.ResponseCommand();
        }


    }

    public class Info
    {
        public int Index { get; set; }
        public string Text { get; set; }
    }
}
