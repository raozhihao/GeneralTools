using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GeneralTool.General.SocketLib;
using GeneralTool.General.SocketLib.Models;

namespace ClientFrm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SocketClient<FixedHeadRecevieState> client;
        private void menuConnect_Click(object sender, EventArgs e)
        {
            this.client = SimpleClientBuilder.CreateFixedCommandSubPack();
            this.client.ReceiveEvent += Client_ReceiveEvent;
            this.client.ErrorEvent += Client_ErrorEvent;
            this.client.Startup(Convert.ToInt32(this.txtPort.Text));
            this.btnSend.Enabled = true;
        }

        private void Client_ErrorEvent(object sender, SocketErrorArg e)
        {
            throw new NotImplementedException();
        }

        private void Client_ReceiveEvent(object sender, ReceiveArg e)
        {
            Console.WriteLine(Encoding.UTF8.GetString(e.PackBuffer.ToArray()));
        }

        private void loopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var count = 1;
                while (true)
                {
                    var c = SimpleClientBuilder.CreateFixedCommandSubPack();
                    c.ErrorEvent += this.Client_ErrorEvent;
                    c.ReceiveEvent += this.Client_ReceiveEvent;
                    //c.DisconnectEvent += Client_DisconnectEvent;
                    c.Startup(8877);

                    var msg = "{\"Url\":\"WatchInterface/Test\",\"Paramters\":null}";
                    //  Console.WriteLine(msg);
                    //  var buffer = Encoding.UTF8.GetBytes(msg);
                    //  c.Send(buffer, c.Socket);
                    //  count++;
                    //  if (!autoReset.WaitOne())
                    //  {
                    //      Console.WriteLine("error");
                    //  }

                    //  c.ErrorEvent -= this.Client_ErrorEvent;
                    //  c.ReceiveEvent -= this.Client_ReceiveEvent;
                    //  c.DisconnectEvent -= Client_DisconnectEvent;
                    //  c.Close();
                    ////  Thread.Sleep(new Random().Next(10,100));
                    ///

                    //var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //socket.Connect("127.0.0.1", 22155);
                    //socket.Send(Encoding.UTF8.GetBytes(msg));
                    //count++;
                    //var buffer = new byte[1024];
                    //var len = socket.Receive(buffer, SocketFlags.None);
                    //if (len > 0)
                    //{
                    //    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, len));
                    //}
                    //else
                    //{

                    //}
                    //socket.Close();
                    //socket = null;
                }

            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.client.Send(this.txtSend.Text, this.client.Socket);
        }
    }
}
