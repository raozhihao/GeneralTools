using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

using GeneralTool.General.WPFHelper.WPFControls;

namespace PropertyGridTest
{
    /// <summary>
    /// ImageTest.xaml 的交互逻辑
    /// </summary>
    public partial class ImageTest : Window
    {
        public ImageTest()
        {
            InitializeComponent();
            this.Loaded += ImageTest_Loaded;
        }

        private void ImageTest_Loaded(object sender, RoutedEventArgs e)
        {
            this.Img.CanvasSizeChanged += Img_CanvasSizeChanged;
            this.Img.ImageSizeChangedEvent += Img_SizeChanged;
        }

        private void Img_CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
         
            if (e.PreviousSize == Size.Empty)
            {
                return;
            }
            //Trace.WriteLine($"width changed : [{e.WidthChanged}] -- height changed : [{e.HeightChanged}]");
            if (e.WidthChanged)
            {

            }
            else
            {

            }
            var vw = e.NewSize.Width / e.PreviousSize.Width;
            var vh = e.NewSize.Height / e.PreviousSize.Height;

            Trace.WriteLine($"-- canvas changed {vw}  ||  {vh}");
            foreach (var item in this.Img.shapes)
            {
                //Trace.WriteLine("tag -> " + item.Tag);
                //var oldP = (Point)item.Tag;
                //var np = new Point(oldP.X * vw, oldP.Y * vh);
                //item.SetValue(Canvas.LeftProperty, np.X);
                //item.SetValue(Canvas.TopProperty, np.Y);
                //item.Tag = np;
                //Trace.WriteLine("pase -> " + item.Tag);

                //缩放后直接将以前保存的像素位置转过来
                var oldP = (Point)item.Tag;
                var np = this.Img.TranslatePoint(oldP);
                item.SetValue(Canvas.LeftProperty, np.X);
                item.SetValue(Canvas.TopProperty, np.Y);
            }


        }

        private void Img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var vw = e.NewSize.Width / e.PreviousSize.Width;
            var vh = e.NewSize.Height / e.PreviousSize.Height;

            //Trace.WriteLine($"-- Image changed {vw}  ||  {vh}");
        }

        private void ImageViewControl_ImageMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton== MouseButtonState.Pressed)
            {
                return;
            }
            if (sender is ImageViewControl v)
            {
                //var el = v.SetPoint(v.CurrentMouseDownPixelPoint, Brushes.Red, 10);
                //el.MouseDown += El_MouseDown;
                //el.MouseMove += El_MouseMove;
                //el.MouseUp += El_MouseUp;

                //var shpae = new System.Windows.Shapes.Polygon()
                //{
                //    Points = new PointCollection()
                //     {
                //         new Point(112,12),
                //         new Point(300,100),
                //         new Point(40,250),
                //         new Point(140,250),
                //         new Point(240,250),
                //         new Point(112,12)
                //     },
                //    Fill = Brushes.Red,
                //    Opacity = 0.5
                //};


                //var shpae = new System.Windows.Shapes.Rectangle()
                //{
                //    Width = 100,
                //    Height=50,
                //    Fill = Brushes.Red,
                //    Opacity = 0.5,
                //    Tag=v.CurrentMouseDownPixelPoint
                //};


                var shpae = new System.Windows.Shapes.Ellipse()
                {
                    Width = 5,
                    Height = 5,
                    Fill = Brushes.Red,
                    Opacity = 0.5,
                    Tag = v.CurrentMouseDownPixelPoint
                };


                shpae.MouseDown += El_MouseDown;
                shpae.MouseMove += El_MouseMove;
                shpae.MouseUp += El_MouseUp;
                v.SetShape(shpae, v.CurrentMouseDownPixelPoint);
            }
        }


        private void El_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.start.HasValue && sender is Shape el)
            {
                this.start = null;
                el.ReleaseMouseCapture();
                this.Img.CanMoveImage = true;
                this.startCanvas = null;
                el.Fill = Brushes.Red;
                var x = Canvas.GetLeft(el);
                var y = Canvas.GetTop(el);
                el.Tag =this.Img.TranslateToPixelPoint( new Point(x, y));//保存其在image上的位置
            }
        }

        private void El_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is Shape el)
            {
                if (this.start.HasValue)
                {
                    var cp = this.Img.GetCurrentPixelPoint(e);
                    var curr = this.Img.TranslatePoint(cp);
                    var ve = curr - start;

                    var np = new Point(startCanvas.Value.X + ve.Value.X, startCanvas.Value.Y + ve.Value.Y);
                    el.SetValue(Canvas.LeftProperty,np.X );
                    el.SetValue(Canvas.TopProperty, np.Y);
                    el.Tag = this.Img.TranslateToPixelPoint(np);//保存其在image上的位置
                }
            }
        }

        private Point? start;
        private Point? startCanvas;
        private void El_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed && sender is Shape el)
            {
                this.start = this.Img.TranslatePoint(this.Img.GetCurrentPixelPoint(e));
                var x = (double)el.GetValue(Canvas.LeftProperty);
                var y = (double)el.GetValue(Canvas.TopProperty);
                this.startCanvas = new Point(x, y);
                this.Img.CanMoveImage = false;
                el.Fill = Brushes.Orange;
                el.CaptureMouse();
            }
        }
    }
}
