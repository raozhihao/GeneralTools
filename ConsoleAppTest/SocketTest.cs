
using GeneralTool.General.SocketHelper;
using System;
using System.Text;

namespace ConsoleAppTest
{
    public class SocketTest
    {
        public void Test()
        {
            var server = new ServerSocketBase(port: 8878);
            server.RecevieEvent += this.Server_RecevieAction1;
            server.ClientConnetedEvent += this.Server_ClientConnetedAction;
            server.ClientDisconnectedEvent += this.Server_ClientDisconnectedAction;
            server.Start();
        }

        private void Server_RecevieAction1(GeneralTool.General.Models.SocketReceiveArgs obj)
        {
            var msg = Encoding.UTF8.GetString(obj.Buffer.ToArray());
            //Console.WriteLine($"Revice:{Encoding.UTF8.GetString(obj.Buffer.ToArray())}");
            obj.TrySend(Encoding.UTF8.GetBytes($"{DateTime.Now}:已成功收到消息,字符串长度:{msg.Length}"));
        }

        private void Server_ClientDisconnectedAction(System.Net.Sockets.Socket obj)
        {
            Console.WriteLine("Client disconnected");
        }

        private void Server_ClientConnetedAction(System.Net.Sockets.Socket obj)
        {
            Console.WriteLine("Client connected");
        }

    }
}
