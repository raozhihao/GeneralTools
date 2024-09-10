using System.Threading.Tasks;
using System.Windows;
using System;

using GeneralTool.CoreLibrary.TaskLib;

using MahApps.Metro.Controls;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.Extensions;

namespace TaskServerUI
{
    /// <summary>
    /// TaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TaskWindow : MetroWindow
    {
        public TaskWindow()
        {
            InitializeComponent();

            var socketStation = new FixedHeadSocketStation(null, null);
            var httpStation = new HttpServerStation();
            taskManager = new TaskManager(null, null, socketStation);
            var ip = "127.0.0.1";
            taskManager.AddServerStation(httpStation, ip, 8899);
            taskManager.AddServerStation(socketStation, ip, 8877);
            ITestTask task = new TestTask();
            var re = taskManager.OpenStations(task);
            _ = taskManager.GetInterfaces();
            //_ = taskManager.DoInterface<string>("Task/TestString", new object[] { "aaa" });
            var models = taskManager.TaskModels;
            TaskTab.ItemsSource = models;
        }

        private readonly TaskManager taskManager;

        private DoTaskParameterItem SelectedTaskInfo
        {
            get
            {
                var model = TaskTab.SelectedItem as TaskModel;

                DoTaskParameterItem taskinfo = model.SelectedItem?.DoTaskParameterItem;
                return taskinfo;
            }
        }
        private async void RunMethod(object sender, RoutedEventArgs e)
        {
            DoTaskParameterItem selected = SelectedTaskInfo;
            if (selected == null)
            {
                _ = MessageBox.Show("没有选择一个接口");
                return;
            }

            try
            {

                object result = await Task.Run(() =>
                {
                    try
                    {
                        return taskManager.DoInterface(selected);
                    }
                    catch (TaskCanceledException)
                    {
                        return "接口中止执行";
                    }
                });

                if (result != null)
                    OutTxt.Text = $"执行结束接口: [{SelectedTaskInfo.Method.Name}] {Environment.NewLine}执行结果: [{taskManager.JsonCovert.SerializeObject(result)}]";
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                OutTxt.Text = ($"执行结束接口: [{SelectedTaskInfo.Method.Name}] 出错,错误信息:{ex.GetInnerExceptionMessage()} ");
            }
            finally
            {

            }
        }
    }
}
