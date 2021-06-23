using GeneralTool.General.WPFHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfAppTest
{
    /// <summary>
    /// ImageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ImageWindow : Window
    {
        public ImageWindow()
        {
            InitializeComponent();
            this.ImageControl.ImageMouseMoveEvent += ImageControl_ImageMouseMoveEvent;
            //this.ImageControl.SaveCutImageEvent += ImageControl_SaveCutImageEvent;
            this.ImageControl.CutImageDownEvent += this.ImageControl_CutImageDownEvent;
            this.ImageControl.CutPanelVisibleChanged += this.ImageControl_CutPanelVisibleChanged;
            this.ImageControl.SizeChanged += this.ImageControl_SizeChanged;
            this.Loaded += this.ImageWindow_Loaded;
        }

        private void ImageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ImageControl.ImageSource == null)
                this.ImageControl.CanImageDraw = false;
        }

        private void ImageControl_CutPanelVisibleChanged(object sender, Int32Rect e)
        {
            this.CutRect.Text = e + "";
        }

        private void ImageControl_CutImageDownEvent(object sender, GeneralTool.General.Models.ImageEventArgs e)
        {
            if (e.Sucess)
            {
                if (this.clip)
                {
                    var re = e.Source.SaveBitmapSouce("tempelte.jpeg");
                    if (re)
                        MessageBox.Show("保存成功");
                    this.clip = false;
                    this.ClipMenu.IsEnabled = true;
                    return;
                }
                this.ImageControl.ImageSource = e.Source;
            }

            else
                MessageBox.Show(e.ErroMsg);


            this.ImageControl.CanImageDraw = e.Sucess;
            this.tgBtn.IsChecked = false;
        }

        private void ImageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.ImageControl.ClearAll(ImageDrawType.Rectangle);
            //this.int32Rects.ForEach(i =>
            //{
            //    this.DrawRect(i);
            //});

        }

        private void ImageControl_ImageMouseMoveEvent(Point obj)
        {
            this.PosText.Text = obj + "";
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var file = new OpenFileDialog();
            if (file.ShowDialog().Value)
            {
                this.ImageControl.ImageSource = new BitmapImage(new Uri(file.FileName));
                this.ImageControl.ResertImageSource();
                this.ImageControl.CanImageDraw = true;
            }
        }

        private void ClipRect(object sender, RoutedEventArgs e)
        {
            this.ImageControl.SendMouseCutRectStart();
        }

        private List<Int32Rect> int32Rects = new List<Int32Rect>();

        private void DrawRect_Click(object sender, RoutedEventArgs e)
        {
            var rect = new Int32Rect(2234, 1077, 400, 500);
            this.DrawRect(rect);
            this.int32Rects.Add(rect);
        }

        private void DrawRect(Int32Rect rect)
        {
            this.ImageControl.DrawRect(rect, Brushes.Green, new SolidColorBrush(Color.FromArgb(30, 2, 3, 4)));
        }

        private void ClearRect_Click(object sender, RoutedEventArgs e)
        {
            this.ImageControl.ClearRect(new Int32Rect(2234, 1077, 400, 500));
        }

        private void DrawPoint(object sender, RoutedEventArgs e)
        {
            this.ImageControl.SetPoint(new Point(2230, 768), new SolidColorBrush(Colors.Red), 5);
        }

        private void Clear_click(object sender, RoutedEventArgs e)
        {
            this.ImageControl.ClearAll();
        }

        private void SetPoint(object sender, RoutedEventArgs e)
        {
            this.SetPosText.Text = this.ImageControl.CurrentMouseDownPixelPoint + "";
            this.ImageControl.SetPoint(this.ImageControl.CurrentMouseDownPixelPoint, new SolidColorBrush(Colors.Red));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.ImageControl.SendMouseCutRectStart();

        }

        private bool clip;
        private void ClipImage(object sender, RoutedEventArgs e)
        {
            clip = true;
            (sender as MenuItem).IsEnabled = false;
            this.ImageControl.SendMouseCutRectStart();
        }
    }
}
