using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using GeneralTool.General.SocketHelper;
using GeneralTool.General.SocketLib;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.SocketLib.Packages;
using GeneralTool.General;
using GeneralTool.General.Logs;
using System.Drawing;

namespace Server
{
    class Program
    {
        static SocketServer<FixedHeadRecevieState> server;
        static int loopNum = 1;
        static FileInfoLog log = new FileInfoLog("Server");
        public int ur { get; set; }
        static void Main(string[] args)
        {
            var picPath = @"C:\Users\raozh\Pictures\Camera\1.bmp";


            //var server = new ServerHelper();
            //var re= server.RegisterClass<ClassLibrary.ISocketClass>(new ClassLibrary.SocketClass());
            //server.Start();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            server = SimpleServerBuilder.CreateFixedCommandPack(log);
            server.ReceiveEvent += Server_ReceiveEvent;
            server.ClientConnctedEvent += Server_ClientConnctedEvent;
            server.ErrorEvent += Server_ErrorEvent;
            server.DisconnectEvent += Server_DisconnectEvent;
            server.Startup(22155);
            Console.WriteLine("Server Startup");
            Console.ReadKey();
            server.Dispose();
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error(e.ExceptionObject + "");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.ExceptionObject);
        }

        private static void Server_DisconnectEvent(object sender, SocketErrorArg e)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Client {e.Client.RemoteEndPoint} Disconnect");
        }

        private static void Server_ErrorEvent(object sender, SocketErrorArg e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Client {e.Client.RemoteEndPoint} Error : {e.Exception?.Message}");
        }

        private static void Server_ClientConnctedEvent(object sender, SocketArg e)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"Client {e.Client.RemoteEndPoint} Connect");
        }

        private static void Server_ReceiveEvent(object sender, ReceiveArg e)
        {
            if (loopNum % 30 == 0)
                Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Receive : {Encoding.UTF8.GetString(e.PackBuffer.ToArray())}");
            var msg = $"Server send : {Guid.NewGuid()}   ---   {loopNum++}";
            var sendMsg = Encoding.UTF8.GetBytes(msg);
            server.Send(sendMsg, e.Client);
            Console.WriteLine($"Send.. {msg}");
        }
    }

    public class TaskResult
    {
        public int MyProperty { get; set; }
    }
}
