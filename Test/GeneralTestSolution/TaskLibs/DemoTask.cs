using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using GeneralTool.General.Attributes;
using GeneralTool.General.TaskLib;

namespace TaskLibs
{
    [Route(nameof(DemoTask)+"/")]
    public class DemoTask:BaseTaskInvoke
    {
        [Route(nameof(TestString))]
        public string TestString()
        {
            return Guid.NewGuid().ToString();
        }

        [Route(nameof(TestGuid))]
        public Guid TestGuid()
        {
            return Guid.NewGuid();
        }

        [Route(nameof(TestParameter))]
        public void TestParameter(string str,int i,double a,Point p,FileAccess fileAccess)
        {

        }

        [Route(nameof(TestModel))]
        public MyModel TestModel(MyModel model)
        {
            model.Y = 111;
            return model;
        }

        [Route(nameof(TestStruce))]
        public MyStruce TestStruce(MyStruce struce)
        {
            return struce;
        }
    }

    public struct MyStruce
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MyModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string MyProperty { get; set; }
    }
}
