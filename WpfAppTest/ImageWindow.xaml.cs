using GeneralTool.General.WPFHelper;
using GeneralTool.General.WPFHelper.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            //this.ImageControl.SaveCutImageEvent += ImageControl_SaveCutImageEvent;
            this.ImageControl.CutImageDownEvent += this.ImageControl_CutImageDownEvent;
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

        MemoryStream stream = new MemoryStream();
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var file = new OpenFileDialog();
            if (file.ShowDialog().Value)
            {

                //this.ImageControl.ImageSource = new BitmapImage(new Uri(file.FileName));

                ImageLoad(file.FileName);

                this.ImageControl.CanImageDraw = true;
                this.ImageControl.ResertImageSource();
            }
        }

        BitmapImage Source;
        bool isload = false;
        private void ImageLoad(string fileName)
        {
            //var bitmap = new System.Drawing.Bitmap(fileName);

            //var width = bitmap.Width;
            //var height = bitmap.Height;
            //int stride = bitmap.Width * 3;//24位
            //stride = stride + (4 - stride % 4);//对齐

            //var bitData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            //Source = BitmapImage.Create(width, height, 96d, 96d, PixelFormats.Bgr24, null, bitData.Scan0, width * height * 3, bitData.Stride);
            //bitmap.UnlockBits(bitData);
            //bitmap.Dispose();

            //this.Source?.Freeze();

            //this.Source = bitmap.ToBitmapImage();
            //this.ImageControl.ImageSource = Source;

            //BitmapImage bi = new BitmapImage();
            // this.ImageControl.ImageSource = bi;
            //bi.BeginInit();
            //bi.CacheOption = BitmapCacheOption.OnLoad;
            //bi.CreateOptions = BitmapCreateOptions.None;
            //bi.UriSource = new Uri(fileName);
            //bi.EndInit();
            //bi.Freeze();


            //var bitmap = new System.Drawing.Bitmap(fileName);
            //var bi = bitmap.ToBitmapImage();
            //bi.StreamSource.Dispose();
            //this.ImageControl.ImageSource = bi;
            //BindingOperations.ClearBinding(this.ImageControl, GeneralTool.General.WPFHelper.WPFControls.ImageViewControl.ImageSourceProperty);

            //MyBitmap myBitmap = new MyBitmap();


            BindingOperations.ClearBinding(this.ImageControl, GeneralTool.General.WPFHelper.WPFControls.ImageViewControl.ImageSourceProperty);
            var bitmap = new System.Drawing.Bitmap(fileName);

            if (mystream != null)
            {
                mystream.Close();
                mystream.Dispose();
                GC.Collect();
            }

            mystream = new MemoryStream();
            bitmap.Save(mystream, System.Drawing.Imaging.ImageFormat.Png);
            mystream.Seek(0, SeekOrigin.Begin);
            mystream.Flush();
           

            var decoder = BitmapDecoder.Create(mystream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            bitmap.Dispose();
            decoder.Frames[0].Freeze();
            //stream.Dispose();
            this.ImageControl.ImageSource = decoder.Frames[0];
             var buffer = mystream.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
          
            
        }

        private MemoryStream mystream;
        private void ImageSource_Changed(object sender, EventArgs e)
        {

        }

        private byte[] BitmapImageToByteArray(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            ms.Seek(0, SeekOrigin.Begin); //一定不要忘记将流的初始位置重置
            var arr = ms.ToArray();
            ms.Dispose();
            bitmap.Dispose();
            return arr;
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

        private void LoopImage(object sender, RoutedEventArgs e)
        {
            var file = new OpenFileDialog();
            if (!file.ShowDialog().Value)
            {
                return;
            }

            var fileName = file.FileName;
            this.ImageControl.CanImageDraw = true;



            Task.Run(() =>
            {
                while (true)
                {
                    var bitmap = new System.Drawing.Bitmap(fileName);
                    PixelFormat formats = GetFormat(bitmap.PixelFormat);
                    BitmapImage.Create(bitmap.Width, bitmap.Height, 96d, 96d, formats, null, null, 1);
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        this.ImageControl.ImageSource = new BitmapImage(new Uri(fileName));//image;
                    }));

                    Thread.Sleep(300);
                }
            });
        }

        private PixelFormat GetFormat(System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            throw new NotImplementedException();
        }
    }

    public class MyBitmap : BitmapSource
    {
        protected override Freezable CreateInstanceCore()
        {
            return new MyBitmap();
        }


    }

}
