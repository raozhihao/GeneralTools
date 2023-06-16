using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneralTool.CoreLibrary.WPFHelper;

namespace SocketApplication.Models
{
    public class SocketModel:BaseNotifyModel
    {
        private string ip = "127.0.0.1";
        public string Ip
        {
            get => this.ip;
            set=>this.RegisterProperty(ref this.ip, value);
        }

        private int port = 8877;
        public int Port
        {
            get => this.port;
            set => this.RegisterProperty(ref this.port, value);
        }

        private int bufferSize = 8192;
        public int BufferSize
        {
            get=> this.bufferSize;
            set => this.RegisterProperty(ref this.bufferSize, value);
        }
    }

   
}
