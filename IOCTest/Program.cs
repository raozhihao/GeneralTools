using GeneralTool.General;
using GeneralTool.General.IniHelpers;
using GeneralTool.General.Ioc;
using GeneralTool.General.SerialPortEx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace IOCTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = SimpleIocSerivce.SimpleIocSerivceInstance;

            //ioc.Inject<MainClass>();
            //ioc.Inject<ILog, ConsoleLog>();
            //ioc.Start();
            //var main = ioc.Resolve<MainClass>();
            //main.Info("main");

            //ioc.Inject(Assembly.GetAssembly(typeof(Class1)), true);
            //

            //ioc.Inject<ILog, C2Log>();
            //ioc.Inject<SubClass>();
            //ioc.Start();


            // LoopRandom();

            // TestPool();

            TestSerialPort();
            Console.WriteLine("End");
            Console.ReadKey();
        }

        private static void TestSerialPort()
        {
            var serialControl = new SerialControl();
            serialControl.OnlineStateEvent += SerialControl_OnlineStateEvent;
            serialControl.PortName = "COM4";
            serialControl.BaudRate = 9600;
            serialControl.Parity = System.IO.Ports.Parity.None;
            serialControl.StopBits = System.IO.Ports.StopBits.One;
            serialControl.DataBits = 8;
            serialControl.ReadTimeout = 3000;
            serialControl.WriteTimeout = 3000;
            serialControl.Open(true);
            serialControl.Head = 0xee;
            serialControl.End = 0xff;

            while (true)
            {
                Console.ReadKey();
                serialControl.Close();
            }
        }

        private static void SerialControl_OnlineStateEvent(object sender, OnlineStateEventArgs e)
        {
            if (e.OnlineState == OnlineState.Unline)
            {
                var serial = sender as SerialControl;
                try
                {
                    serial.Open(true);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
                while (!serial.IsOpen)
                {
                    Trace.WriteLine("重试连接");
                    try
                    {
                        serial.Open(true);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.Message);
                    }
                    Thread.Sleep(1000);
                }
                Trace.WriteLine("重试连接成功");
            }
        }

        private static async void TestPool()
        {
            var pool = new ObjectPool<Student>(() => new Student(), 50);
            var list = new List<Task>();
            for (int i = 0; i < 100; i++)
            {
                var task = Task.Run(async () =>
                 {
                     var item = pool.Item;
                     //hread.Sleep(100);
                     await Task.Delay(10);
                     pool.Resolve(item);
                 });
                list.Add(task);
            }

            await Task.WhenAll(list.ToArray());
            pool.Dispose();
            //  var ij = pool.Item;
        }

        private static void LoopRandom()
        {
            var r = new Random();

            for (int i = 0; i < 100; i++)
            {

                Task.Run(() =>
                {
                    Console.WriteLine(RandomEx.Next(1, 10));
                    // Console.WriteLine(r.Next(1, 10));
                });

            }

        }
    }


    public class ThemeNode : Category
    {
        public ThemeNode(string sectionName) : base(sectionName)
        {
            this.DicString = new Node<string>(sectionName, "dic", "");
        }
        public Dictionary<int, string> Dics
        {
            get
            {
                return JsonConvert.DeserializeObject<Dictionary<int, string>>(this.DicString.Value);
            }
            set
            {
                if (value == null)
                    this.DicString.Value = "";
                else
                    this.DicString.Value = JsonConvert.SerializeObject(value);

            }
        }
        protected Node<string> DicString { get; set; }
    }

}
