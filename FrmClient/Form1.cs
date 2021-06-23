using SuperSocket.ClientEngine;
using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace FrmClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var client = new AsyncTcpSession();
            client.Closed += Client_Closed;
            client.DataReceived += this.Client_DataReceived;
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7700));

        }

        private void Client_DataReceived(object sender, DataEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Data, e.Offset, e.Length);
            this.WriteLog(message);
        }

        void WriteLog(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    WriteLog(message);
                }));
                return;
            }
            this.richTextBox1.AppendText(message + Environment.NewLine);
        }

        private void Client_Closed(object sender, EventArgs e)
        {

        }
    }
}
