using System;
using System.Diagnostics;
using System.Threading;
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
        private readonly ExcuteCancelTokenSource tokenSource = new ExcuteCancelTokenSource();
        private void StartBtn_Click(object sender, EventArgs e)
        {
            tokenSource.Reset();
            thread = new Thread(LoopTestMethod) { IsBackground = true };
            thread.Start();
        }

        private void LoopTestMethod()
        {
            Trace.WriteLine("进入线程");
            while (!tokenSource.IsCancelNotify)
            {
                Thread.Sleep(10);
            }
            Trace.WriteLine("已退出线程");
        }

        private async void PauseBtn_Click(object sender, EventArgs e)
        {
            await tokenSource.Pause();
        }

        private async void ResumeBtn_Click(object sender, EventArgs e)
        {
            await tokenSource.Resume();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            thread.Join();
        }
    }
}
