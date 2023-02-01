using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using GeneralTool.General.ExceptionHelper;
using GeneralTool.General.Models;
using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;

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

    }
}
