using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GeneralTool.General;
using GeneralTool.General.Interfaces;
using GeneralTool.General.Models;
using GeneralTool.General.SocketLib.Models;
using GeneralTool.General.SocketLib.Packages;
using GeneralTool.General.TaskLib;

using static System.Collections.Specialized.BitVector32;

namespace PropertyGridTest
{

    public class ReInfo
    {
        public string Url { get; set; }
        public object[] Datas { get; set; }
    }

    [Serializable]
    public struct MyStruct
    {
        public int MyProperty { get; set; }
        public int ab { get; set; }
        public Point Point { get; set; }
    }

    /// <summary>
    /// LogTest.xaml 的交互逻辑
    /// </summary>
    public partial class LogTest : Window
    {
        public LogTest()
        {
            InitializeComponent();

            this.SerializeTest();
            this.DataContext = this;

            this.StationTest();
        }

        private void SerializeTest()
        {
            // var s = new MyStruct() { Point = new Point(1, 2), ab = 1, MyProperty = 2 };
            //var buffer= s.Serialize();
            // s = buffer.Desrialize<MyStruct>();

            var s = new ReInfo() { Datas = new object[] { new Point(1, 2) } };
            var buffer = s.Serialize();
            s = buffer.Desrialize<ReInfo>();
        }

        private void StationTest()
        {
            var manager = new TaskManager(null, null);
            var httpStation = new HttpServerStation(null);
            //var genStation = new GenServerStation<FixedHeadRecevieState>(null, null, () => new FixedHeadPackag());
            var genStation= new ServerStation(null, null);
            manager.AddServerStation(httpStation,"127.0.0.1",8877);
            manager.AddServerStation(genStation, "127.0.0.1",8897);

            var task = new StationTask();
            manager.OpenStations(task);

            manager.GetInterfaces();
        }

        private int index = 0;
        public ObservableCollection<LogMessageInfo> Logs { get; set; } = new ObservableCollection<LogMessageInfo>();
        private void AddMethod(object sender, RoutedEventArgs e)
        {
            Logs.Add(new LogMessageInfo(index + "", GeneralTool.General.Enums.LogType.Info));
            index++;
        }

        private void RemoveMethod(object sender, RoutedEventArgs e)
        {
            Logs.RemoveAt(Logs.Count - 1);
        }

        private void AddRangeMethod(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                Logs.Add(new LogMessageInfo(i + "", GeneralTool.General.Enums.LogType.Info));
            }
        }

        private void RemoveRangeMethod(object sender, RoutedEventArgs e)
        {
            //for (int i = 1000-1; i > 100; i--)
            //{
            //    Logs.RemoveAt(i);
            //}

            this.Logs.Clear();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    this.Logs.Add(new LogMessageInfo(Guid.NewGuid().ToString(), GeneralTool.General.Enums.LogType.Info));
                }
            });
           
        }
    }
}
