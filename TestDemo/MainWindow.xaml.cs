using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TestDemo
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

        private void TestAddMethod(object sender, RoutedEventArgs e)
        {
            RectControlView.AddBlock(new BlockInfoModel()
            {
                Bounds = new Rect(10, 10, 100, 50),
                //Content = "Content",
                Header = "Header" + RectControlView.Blocks.Count + 1,
                Key = Guid.NewGuid(),

            });
        }

        private void TestValidateMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                RectControlView.InvalidateBlocks();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
        }

        private void GetAllInfosMethod(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            _ = builder.AppendLine("块信息如下:");
            foreach (KeyValuePair<Guid, BlockInfoModel> item in RectControlView.Blocks)
            {
                BlockInfoModel value = item.Value;
                _ = builder.AppendLine($"[{value.Header}]  -  [{value.Bounds}]");
            }

            _ = MessageBox.Show(builder.ToString());
        }

        private void ZoomCheckedMethod(object sender, RoutedEventArgs e)
        {
            RectControlView.ShowOrHideZoom(ZoomCheckBox.IsChecked.Value ? Visibility.Visible : Visibility.Collapsed);
        }

        private void TestChangeSizeToMaxMethod(object sender, RoutedEventArgs e)
        {
            RectControlView.ChangeCanvasSize(RectControlView.ActualWidth, RectControlView.ActualHeight);
        }
    }
}
