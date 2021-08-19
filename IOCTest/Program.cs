using GeneralTool.General.IniHelpers;
using GeneralTool.General.Ioc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

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

            ioc.Inject<ILog, C2Log>();
            ioc.Inject<SubClass>();
            ioc.Start();
            Console.ReadKey();
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
