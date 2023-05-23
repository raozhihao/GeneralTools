using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.TaskLib;

namespace ConsoleFram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new TaskTests.NoPackTask().Test();
        }
    }
}
