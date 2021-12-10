using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class SocketClass : ISocketClass
    {
        int index = 0;

        public event Action EventTest;

        public void Log(string msg)
        {
            Console.WriteLine("Hello:" + msg + " - " + index++);
            this.EventTest?.Invoke();
        }

      
    }
}
