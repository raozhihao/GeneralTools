using System;

namespace IOCTest
{
    public class MainClass
    {
        public ILog Log { get; set; }
        public MainClass(ILog Log)
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


}
