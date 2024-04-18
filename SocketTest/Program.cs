using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.IniHelpers;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.TaskLib;

using SocketTest.IniCommon;

namespace SocketTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //new IOCTest().Test();
            //var color = "Blue".ToDrawingColor();
            //var str = color.ToHexString();

            //var dir = "\\aa\\bb\\cc";
            //dir.CreateDirectory();

            //var client = new FixedHeadSocketClient(null, null);
            //client.Client.ReceiveBufferSize = 1024 * 1024 * 10;
            //client.ReadTimeOut = 18000;

            //while (true)
            //{
            //    client.Startup("127.0.0.1", 8877);
            //    client.Send(new ServerRequest()
            //    {
            //        Url = "Task/TestString"
            //    }, System.Threading.CancellationToken.None);
            //    System.Threading.Thread.Sleep(100);
            //}
            //client.Dispose();

            //var ini = new SimpleIniManager();
            //ini.SetValue("Window", "abc",33);
            //var a = ini.GetValue("Window", "abc");

            //var file = new MemoryStream();
            //SingleInstance<MemoryStream>.SetInstanceFunc(() => file);

            //var f = SingleInstance<MemoryStream>.GetInstance(() => file);

            //var model = SingleInstance<Model>.Instance;
            //model.X = 100;
            //var m2 = SingleInstance<Model>.Instance;

            //var node = new WindowNode(nameof(WindowNode));
            //var width = node.Width.Value;
            //node.Width.Value = 2;

            var c1 = new Class1();
            c1.C1();
            c1.C3();

            c1.C2();

            c1.Remove();

            Console.ReadKey();
        }
    }

    public class Model
    {
        public int X { get; set; }
    }

}
