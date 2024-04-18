using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary;

namespace SocketTest
{
    internal class Class1
    {
        public void C1()
        {
            MessageManager.Instance.RegisterMessage<string>(nameof(C1), this.GetMessage);
        }

        public void C3()
        {
            MessageManager.Instance.RegisterMessage<string>(nameof(C3), this.GetMessage);
            MessageManager.Instance.RegisterMessage<object>("ccc", this.GetMessageObj);

            var a1 = MessageManager.Instance.GetAction<string>("ccc");
        }

        private void GetMessageObj(object obj)
        {
            Console.WriteLine("接收到obj消息:" + obj);
        }

        internal void C2()
        {
            MessageManager.Instance.SendMessage("C1", "c1的消息");
            MessageManager.Instance.SendMessage("C3", "c3的消息");
            MessageManager.Instance.SendMessage("ccc", new EntryPointNotFoundException());
        }


        private void GetMessage(string obj)
        {
            Console.WriteLine("接收到消息:" + obj);
        }

        public void Remove()
        {
            MessageManager.Instance.UnRegisterMessage("C1");
            MessageManager.Instance.UnRegisterMessage("C2");
            MessageManager.Instance.UnRegisterMessage("C3");
        }

    }
}
