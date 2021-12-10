using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using GeneralTool.General.IPCHelper;

namespace Client
{
    [Serializable]
    class IPCClient
    {
        public void StartUp()
        {
            var helper = new IPCClientHelper<IPCModel>();
            var model = helper.GetInstance();
            model.Loop(10);
            Console.ReadKey();
        }

        private void Model_OperationEvent(object obj)
        {
            Console.WriteLine($"client : {obj}");
        }
    }
}
