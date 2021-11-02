using GeneralTool.General.Adb;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageTest
{
    /// <summary>
    /// AdbWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AdbWindow : Window
    {
        AdbHelper adb = new AdbHelper(@"C:\Code\Ruizi\AndroidAgingTest\App_Debug\adb\adb.exe");
        DateTime mouseDownTime;
        Point mouseDownPoint;
        public AdbWindow()
        {
            InitializeComponent();
            this.Loaded += AdbWindow_Loaded;
            this.pic.ImageDownEvent += Pic_MouseDown;
            this.pic.ImageUpEvent += Pic_ImageUpEvent;
        }

     
        private void Pic_ImageUpEvent(object sender, Point e)
        {
            this.adb.Swipe((int)this.mouseDownPoint.X, (int)this.mouseDownPoint.Y, (int)e.X, (int)e.Y);
        }

        private void Pic_MouseDown(object sender, Point e)
        {
            this.mouseDownTime = DateTime.Now;
            this.mouseDownPoint = e;

        }

        private void AdbWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadImage();
        }

        void LoadImage()
        {
            new Thread(LoopLoad) { IsBackground = true }.Start();
           
        }

        private void LoopLoad()
        {
            while (true)
            {
                lock (this.adb)
                {
                    //加载图片
                    var result = this.adb.GetScreen();
                    if (result.ResultBool)
                    {
                        this.Dispatcher.Invoke(new Action(() => {
                            this.pic.ImageSource = GeneralTool.General.WPFHelper.Extensions.BitmapExtensions.ToBitmapImage((System.Drawing.Bitmap)result.ResultItem);
                        }));

                    }
                }
              
            }
        }
    }
}
