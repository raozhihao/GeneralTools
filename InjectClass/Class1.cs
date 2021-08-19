using GeneralTool.General.Attributes;
using System;

namespace InjectClass
{
    [InjectType]
    public class Class1
    {
        public Class1(Ilog clas)
        {
            clas.Info("Cls");
        }
    }

    [InjectType]
    public class Class2
    {
        public Class3 class3 { get; set; }
        public Class1 Class1 { get; set; }
        public Class2()
        {

        }

        public Class2(string s)
        {

        }
    }
    [InjectType]
    public abstract class class5
    {
    }
    public interface Ilog
    {
        void Info(string msg);
    }
    [InjectType]
    public class Log : Ilog
    {
        public void Info(string msg)
        {
            Console.WriteLine("Log {0}", msg);
        }
    }
    public class Class3
    {
    }
}
