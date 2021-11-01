using GeneralTool.General.WPFHelper;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.IVC.CutPanelVisibleChanged += IVC_CutPanelVisibleChanged;
            this.IVC.ImageMouseMoveEvent += IVC_ImageMouseMoveEvent;
        }

        private void IVC_ImageMouseMoveEvent(GeneralTool.General.Models.ImageMouseEventArgs obj)
        {
            this.Txt.Text = obj.CurrentPixelPoint + "";
        }

        private void IVC_CutPanelVisibleChanged(object sender, GeneralTool.General.Models.ImageCutRectEventArgs e)
        {
            this.Txt.Text = e.CutPixelRect + "";
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var open = new OpenFileDialog();
            if (open.ShowDialog() == true)
            {
                this.IVC.ImageSource = new BitmapImage(new Uri(open.FileName, UriKind.Absolute));
                this.IVC.CanImageDraw = true;
            }
        }

        private void IVC_CutImageDownEvent(object sender, GeneralTool.General.Models.ImageEventArgs e)
        {
            if (e.Sucess)
                e.Source.SaveBitmapSouce("1.jpeg");
        }
    }
}
