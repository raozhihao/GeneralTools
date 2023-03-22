using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.General.Interfaces;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.TaskLib;

namespace SimpleDiagram
{
    internal class NoPackStation : ServerStationBase
    {
        private SocketServer<ReceiveState> socketServer;
        public NoPackStation(ILog log) : base(log)
        {
        }

        public override bool Close()
        {
           
        }

        public override bool Start(string ip, int port)
        {
            throw new NotImplementedException();
        }
    }
}
