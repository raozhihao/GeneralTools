using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using GeneralTool.CoreLibrary;

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
            this.RectControlView.AddBlock(new BlockInfoModel()
            {
                Bounds = new Rect(10, 10, 100, 50),
                //Content = "Content",
                Header = "Header" + this.RectControlView.Blocks.Count + 1,
                Key = Guid.NewGuid(),
                
            });
        }

      

        private void TestValidateMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RectControlView.InvalidateBlocks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetAllInfosMethod(object sender, RoutedEventArgs e)
        {
            var builder = new StringBuilder();
            builder.AppendLine("块信息如下:");
            foreach (var item in this.RectControlView.Blocks)
            {
                var value = item.Value;
                builder.AppendLine($"[{value.Header}]  -  [{value.Bounds}]");
            }

            MessageBox.Show(builder.ToString());
        }

        private void ZoomCheckedMethod(object sender, RoutedEventArgs e)
        {
           this.RectControlView.ShowOrHideZoom( this.ZoomCheckBox.IsChecked.Value?Visibility.Visible:Visibility.Collapsed);
        }

        private void TestChangeSizeToMaxMethod(object sender, RoutedEventArgs e)
        {
            this.RectControlView.ChangeCanvasSize(this.RectControlView.ActualWidth,this.RectControlView.ActualHeight);
        }
    }
}
