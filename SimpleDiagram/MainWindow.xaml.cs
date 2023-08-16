using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

using SimpleDiagram.Blocks;
using SimpleDiagram.BlockVIewModels;
using SimpleDiagram.Common;

namespace SimpleDiagram
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ExcuteCancelTokenSource tokenSource;
        private readonly TaskExecuteController taskExecute = new TaskExecuteController(null);
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;



        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Init();
        }

        private void Init()
        {
            var start = new StartBlock
            {
                Padding = new Thickness(15, 5, 15, 5),
                Header = "开始块",
                CanRepeatToCanvas = false,
                Background = System.Windows.Media.Brushes.DarkCyan,
                IsStart = true,

                DataContext = new StartViewModel()
            };
            Canvas.SetLeft(start, 20);
            Canvas.SetTop(start, 20);
            this.dc.Children.Clear();
            this.dc.AddItem(start, false);
            start.ApplyTemplate();
            this.dc.AddTempelte(start);
            start.Apply();
        }


        private void RunMethod(object sender, RoutedEventArgs e)
        {
            //以下是从当前设计器界面上获取
            var collection = this.dc.Children.OfType<BlockItem>();
            var start = collection.FirstOrDefault(b => b.IsStart);
            this.dc.IsEnabled = false;
            var current = start.DataContext as BaseBlockViewModel;
            new LayoutHelper(null).LoadBlocks(this.dc.Children, start);

            ////以下方法是从数据库中获取第一个开始块,并递归创建其树
            //var current = this.TaskExecute.GetStartBlockModel(this.prevScript.ScriptId);
            this.Run(current);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        public async void Run(BaseBlockViewModel current)
        {
            this.tokenSource = new ExcuteCancelTokenSource();

            try
            {
                _ = await this.taskExecute.Start(current, "脚本执行", this.tokenSource);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetInnerExceptionMessage());
            }
            finally
            {
                this.dc.IsEnabled = true;
            }

            if (this.tokenSource != null)
                this.tokenSource.Canceld = true;
        }

        private void dc_CopyEvent(object sender, GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models.BlockCopyArgs e)
        {
            if (e.DragItem != null && e.DestBlock != null && e.DragItem.DragItem != null && e.DragItem.CanRepeatToCanvas)
            {
                var source = e.DragItem.DragItem as BaseBlock;
                var sink = e.DestBlock as BaseBlock;

                //清除连接线相关属性,否则会出现循环引用
                source.BlockViewModel.SourceBlockModels.Clear();
                source.BlockViewModel.SinkBlockModels.Clear();
                source.BlockViewModel.NextModel = null;

                sink.BlockViewModel = source.BlockViewModel.Copy();
                sink.BlockViewModel.BlockId = sink.ID.ToString();

                //复制完成后不打开
                e.OnCreateToCanvas = false;
            }
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public T Copy<T>(T item)
        {
            if (item == null) return default;

            var type = item.GetType();
            var properties = type.GetProperties();
            var copyObj = Activator.CreateInstance(type);
            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                if (property.GetMethod != null && property.SetMethod != null)
                {
                    var value = property.GetMethod.Invoke(item, null);
                    property.SetValue(copyObj, value, null);
                }
            }

            return (T)copyObj;
        }

        private void BackMethod(object sender, RoutedEventArgs e)
        {
            this.dc.Back();
        }

        private void NextMethod(object sender, RoutedEventArgs e)
        {
            this.dc.Next();
        }
    }
}
