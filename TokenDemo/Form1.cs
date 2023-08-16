using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using GeneralTool.CoreLibrary.Models;

namespace TokenDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private Thread thread;
        private ExcuteCancelTokenSource tokenSource = new ExcuteCancelTokenSource();
        private void StartBtn_Click(object sender, EventArgs e)
        {
            this.tokenSource.Reset();
            this.thread = new Thread(LoopTestMethod) { IsBackground = true };
            this.thread.Start();
        }

        private void LoopTestMethod()
        {
            Trace.WriteLine("进入线程");
            while (!this.tokenSource.IsCancelNotify)
            {
                Thread.Sleep(10);
            }
            Trace.WriteLine("已退出线程");
        }

        private async void PauseBtn_Click(object sender, EventArgs e)
        {
            await this.tokenSource.Pause();
        }

        private async void ResumeBtn_Click(object sender, EventArgs e)
        {
            await this.tokenSource.Resume();
        }

        private  void CancelBtn_Click(object sender, EventArgs e)
        {
            this.tokenSource.Cancel();
            this.thread.Join();
        }
    }
}
