using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary
{
    [Serializable]
    public class IPCModel:MarshalByRefObject
    {
        public event Action<object> OperationEvent;

        public void Loop(int num)
        {
            for (int i = 0; i < num; i++)
            {
                this.OperationEvent?.Invoke(i);
                Console.WriteLine(i);
                Thread.Sleep(2000);
            }
        }
    }
}
