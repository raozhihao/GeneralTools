using System;

namespace IOCTest
{
    public class MainClass
    {
        public ILog Log { get; set; }
        public MainClass(string a, ILog Log, int b)
        {
            this.Log.Info("Ctor");
        }

        public void Info(string msg)
        {
            this.Log.Info(msg);
        }
    }

    public interface ILog
    {
        void Info(string msg);
    }

    public class ConsoleLog : ILog
    {
        public ConsoleLog(MainClass mainClass)
        {

        }
        public void Info(string msg)
        {
            Console.WriteLine("Console {0}", msg);
        }
    }

    public class BaseClass
    {
        public ILog Log { get; set; }

        public BaseClass(ILog log)
        {
            if (log == null)
                this.Log = new DefaultLog();
            this.Log = log;
        }

        public BaseClass()
        {
            if (this.Log == null)
                this.Log = new DefaultLog();
        }
    }

    public class SubClass : BaseClass
    {
        public SubClass()
        {
            this.Log.Info("ss");
        }
    }

    public class DefaultLog : ILog
    {
        public void Info(string msg)
        {
            Console.WriteLine("Default Log");
        }
    }


    public class C2Log : ILog
    {
        public void Info(string msg)
        {
            Console.WriteLine("C2 Log");
        }
    }

}
