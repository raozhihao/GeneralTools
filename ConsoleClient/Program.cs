using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new GeneralTool.General.SocketHelper.ClientHelper();
            helper.Init(new KeyValuePair<string, object>("127.0.0.1", 33455));
            GeneralTool.General.SocketHelper.Config.Register(helper);
            var ih = GeneralTool.General.SocketHelper.ClientFactory<ITestLib>.GetClientObj();
            ih.T1("aaa");
            var result = ih.T2();
        }
    }
}
