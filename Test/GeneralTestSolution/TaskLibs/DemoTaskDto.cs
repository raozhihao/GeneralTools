using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using GeneralTool.General.Attributes;
using GeneralTool.General.Interfaces;
using GeneralTool.General.TaskLib;

namespace TaskLibs
{
    public class DemoTaskDto: BaseClientTask
    {
        public DemoTaskDto( ILog log = null, IJsonConvert jsonConvert = null) : base("127.0.0.1",8878,nameof(DemoTask)+"/", log, jsonConvert)
        {
            //if (jsonConvert == null)
            //    jsonConvert = new NewsoftJsonConvert();
        }

        public string TestString()
        {
            return this.InvokeResult<string>(nameof(TestString));
        }

        public Guid TestGuid()
        {
            return this.InvokeResult<Guid>(nameof(TestGuid));
        }

        public void TestParameter(string str, int i, double a, Point p, FileAccess fileAccess)
        {
            this.InvokeResult(nameof(TestParameter),str,i,a,p,fileAccess);
        }

        public MyModel TestModel(MyModel model)
        {
           
            return this.InvokeResult<MyModel>(nameof(TestModel),model);
        }

        public MyStruce TestStruce(MyStruce struce)
        {
            return this.InvokeResult<MyStruce>(nameof(TestStruce), struce);
        }


    }
}
