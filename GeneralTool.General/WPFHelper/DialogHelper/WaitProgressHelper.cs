using System;
using System.Windows;
using System.Windows.Controls;

namespace GeneralTool.General.WPFHelper.DialogHelper
{
    /// <summary>
    /// 等待帮助类，使用此类可以出现一个等待(注意：此等待为同步，异步代码请自行编写)
    /// </summary>
    public class WaitProgressHelper
    {
        #region Private 字段

        private readonly WaitViewModel vm;
        private Grid mainGrid;
        private Window parentWindow;

        #endregion Private 字段

        #region Public 构造函数

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="title">
        /// 标题
        /// </param>
        /// <param name="caption">
        /// 内容
        /// </param>
        public WaitProgressHelper(string title = "", string caption = "")
        {
            vm = new WaitViewModel
            {
                Title = title,
                Caption = caption
            };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="viewModel">
        /// 对等待框进行设定
        /// </param>
        public WaitProgressHelper(WaitViewModel viewModel)
        {
            if (viewModel != null)
            {
                vm = viewModel;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public WaitProgressHelper()
        {
            vm = new WaitViewModel();
        }

        #endregion Public 构造函数

        #region Public 方法

        /// <summary>
        /// 关闭等待
        /// </summary>
        public void CloseDialog()
        {
            if (mainGrid == null)
            {
                return;
            }

            parentWindow.Dispatcher.Invoke(() =>
            {
                EnableControls(true);
                vm.ProgressValue = 0;
                mainGrid.Children.RemoveAt(0);
                UIElement ui = mainGrid.Children[0];
                mainGrid.Children.Clear();

                mainGrid = null;
                parentWindow.Content = null;
                parentWindow.Content = ui;
            });
        }

        /// <summary>
        /// 显示等待
        /// </summary>
        /// <param name="window">
        /// 需要显示的等待窗体
        /// </param>
        /// <returns>
        /// </returns>
        public WaitViewModel ShowWaitDialog(Window window)
        {
            if (window == null)
            {
                return null;
            }

            window.Dispatcher.BeginInvoke(new Action(() =>
            {
                WaitView layer = new WaitView
                {
                    DataContext = vm
                }; //遮罩层
                UIElement content = window.Content as UIElement;//原有的content
                window.Content = null;

                int zindex = Panel.GetZIndex(content);
                Panel.SetZIndex(layer, zindex + 1);

                mainGrid = new Grid();
                mainGrid.Children.Add(layer);
                mainGrid.Children.Add(content);
                window.Content = mainGrid;
                window.UpdateLayout();
                parentWindow = window;
                EnableControls(false);
            }));

            return vm;
        }

        #endregion Public 方法

        #region Private 方法

        private void EnableControls(bool enable)
        {
            (parentWindow.Content as UIElement).IsEnabled = enable;
        }

        #endregion Private 方法
    }
}