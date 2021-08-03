using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IocTest
{
   public interface Ilog
    {
        void Log(string msg);
    }

    public class ConsoleLog : Ilog, GeneralTool.General.Ioc.IInitInterface
    {
        public Controller Controller { get; set; }

        public void Init()
        {
            this.Controller.SendEvent += Controller_SendEvent;
        }

        public void Log(string msg)
        {
            Console.WriteLine("ConsoleLog : {0}",msg);
        }
        private void Controller_SendEvent(string obj)
        {
            Console.WriteLine("Controller : {0}",obj);
        }
    }

    public class MainModel:GeneralTool.General.Ioc.IInitInterface
    {
        public Ilog Log { get; set; }
        public Controller Controller { get; set; }

        public void Init()
        {
            this.Log.Log("Init");
            this.Controller.Send("Main Send");
        }
    }

    public class Test : GeneralTool.General.Ioc.IInitInterface
    {
        private string str;
        public Ilog Log { get; set; }
        public Test(string str)
        {
            this.str = str;
        }

        public void Init()
        {
            this.Log.Log(str);
        }

        public void Con()
        {
            Console.WriteLine(this.str);
        }
    }

    public class Controller
    {
        public event Action<string> SendEvent;
        public void Send(string msg)
        {
            this.SendEvent?.Invoke(msg);
        }
    }
}
