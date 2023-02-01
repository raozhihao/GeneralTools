using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using TaskLibs;

namespace ClientConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dto = new DemoTaskDto(null, null);
            var mystruct = dto.TestStruce(new MyStruce() { X = 111, Y = 222 });

            var myModel = dto.TestModel(new MyModel() { X = 10, MyProperty = "aaa" });

            dto.TestParameter("aa", 1, 3.2d, new Point(2, 3), System.IO.FileAccess.Read);

            var guid = dto.TestGuid();
            var str = dto.TestString();
        }
    }
}
