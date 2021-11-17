using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeneralTool.General.SocketHelper;
using Microsoft.Win32;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Log(object message)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Logs.Inlines.Add(DateTime.Now + " : " + message + Environment.NewLine);
            }));
        }

        TcpClient client;
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            this.Disconnect.IsEnabled = true;
            this.Connect.IsEnabled = false;
            if (this.client == null || !this.client.Connected)
            {
                try
                {
                    this.client = new TcpClient("192.168.12.102", 8899);
                }
                catch (Exception ex)
                {
                    this.Log(ex);
                    this.Connect.IsEnabled = true;
                    this.Disconnect.IsEnabled = false;
                }
            }
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            this.Disconnect.IsEnabled = true;
            this.Connect.IsEnabled = true;
            if (this.client == null)
            {
                return;
            }

            try
            {
                this.client.Close();
            }
            catch (Exception ex)
            {
                this.Log(ex);
            }
        }

        SocketCommon common = new SocketCommon();
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (this.client == null)
            {
                return;
            }
            if (!this.client.Connected)
            {
                return;
            }

            var msg = this.Msg.Text;
            var buffer = Encoding.UTF8.GetBytes(msg);

            //发送数据
            var len = common.Send(this.client.Client, buffer);
            if (len == -1)
            {
                this.Log(common.ErroException);
                return;
            }
            this.Log("OK send");
        }

        private void Rand_Click(object sender, RoutedEventArgs e)
        {
            if (this.client == null)
            {
                return;
            }
            if (!this.client.Connected)
            {
                return;
            }

            Task.Run(() =>
            {
                int index = 1;
                while (true)
                {
                    var num = GeneralTool.General.RandomEx.Next(10000, int.MaxValue);
                    var msg = $"{index}  ==  {num}";
                    this.Log(msg);
                    var buffer = Encoding.UTF8.GetBytes(msg);

                    //发送数据
                    var len = common.Send(this.client.Client, buffer);
                    if (len == -1)
                    {
                        this.Log(common.ErroException);
                        return;
                    }
                    index++;
                    this.Log("OK send");
                    //Thread.Sleep(300);
                }
            });
        }

        private void Large_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog();
            if (!open.ShowDialog().Value)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    var re = SendFile(open.FileName);
                    if (!re)
                    {
                        return;
                    }


                    var reuslt = MessageBox.Show("loop?", "tips", MessageBoxButton.OKCancel);
                    if (reuslt == MessageBoxResult.OK)
                    {

                        while (true)
                        {
                            re = SendFile(open.FileName);
                            if (!re)
                            {
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Log(ex);
                }
            });

        }

        private bool SendFile(string fileName)
        {
            var buffer = File.ReadAllBytes(fileName);
            this.Log("开始发送");
            var len = common.Send(this.client.Client, buffer, ProgressCallBack, Log);
            int count = buffer.Length;
            buffer = null;
            this.Log("发送完成");
            if (len < count)
            {
                this.Log("Server error:" + common.ErroException);
                return false;
            }

            return true;
        }

        private void ProgressCallBack(double obj)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.progress.Value = obj;
            }));

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var client = GeneralTool.General.SocketHelper.ClientFactory<ClassLibrary.ISocketClass>.GetClientObj();
            client.Log("I am client");
        }
    }
}
