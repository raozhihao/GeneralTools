using System;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {

            new Test2().Test();

            //var item = new RequestItem()
            //{
            //    ClassName = typeof(Program).Name,
            //    MethodName = nameof(Program.Main),
            //    Parameters = new List<object>() { new Meta.Axsi() { Joints = new double[] { 1, 2, 3 } }, new Meta.Ori() { ox = 1, oy = 2, oz = 3 } }
            //};

            //var me = GeneralTool.General.SerializeExtensions.Serialize(item);

            //var ite=GeneralTool.General.SerializeExtensions.Desrialize<RequestItem>(me);
            ////var server = new GeneralTool.General.SocketHelper.Server.ServerHelper();
            ////server.RegisterClass<Libs.IMethodClass,Libs.MethodClass>();
            ////server.Start();
            ////Console.ReadKey();
            //var server= new SuperSocket.SocketBase.AppServer();
            //server.Setup(7700);
            //server.NewSessionConnected += Server_NewSessionConnected;
            //server.NewRequestReceived += Server_NewRequestReceived;
            //var re= server.Start();
            //if(!re)
            //{
            //    Console.WriteLine("服务开启失败");
            //    return;
            //}
            //Console.ReadKey();
            //server.GetAllSessions().Foreach(s =>
            //{
            //    s.TrySend("Server message");
            //});
            Console.ReadKey();
        }

        private static void Server_NewRequestReceived(SuperSocket.SocketBase.AppSession session, SuperSocket.SocketBase.Protocol.StringRequestInfo requestInfo)
        {
            Console.WriteLine($"Received {session.RemoteEndPoint},{requestInfo.Key} {requestInfo.Body}");
        }

        private static void Server_NewSessionConnected(SuperSocket.SocketBase.AppSession session)
        {
            Console.WriteLine($"{session.RemoteEndPoint} connected");
        }
    }

    public class Meta
    {
        public struct Axsi
        {
            public double[] Joints { get; set; }
        }

        public struct Ori
        {
            public int ox { get; set; }
            public int oy { get; set; }
            public int oz { get; set; }
        }
    }

    public class RequestItem
    {
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public List<object> Parameters { get; set; }
    }
}
