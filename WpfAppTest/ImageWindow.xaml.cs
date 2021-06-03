﻿using GeneralTool.General.WPFHelper.WPFControls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
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

            this.ImageControl.SizeChanged += this.ImageControl_SizeChanged;
        }

        private void ImageControl_CutImageDownEvent(object sender, GeneralTool.General.Models.ImageEventArgs e)
        {
            if (e.Sucess)
                this.ImageControl.ImageSource = e.Source;
            else
                MessageBox.Show(e.ErroMsg);
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
    }
}
