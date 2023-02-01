using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using GeneralTool.General.Logs;

namespace PropertyGridTest
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.ImgControl.CutMenuCancelEvent += ImgControl_CutMenuCancelEvent;
            this.ImgControl.CutImageDownEvent += ImgControl_CutImageDownEvent;



            this.ImgControl.ImageSizeChangedEvent += ImgControl_SizeChanged;
            this.Loaded += Window1_Loaded;
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            this.I2.SetPoint(new Point(500, 100), Brushes.Red, 7.0);
            var log = new FileInfoLog("aaa", "C:\\");
            log.Log("test");

            var log2 = new FileInfoLog("aaa");
            log2.Info("aa");
        }

        private void ImgControl_CutImageDownEvent(object sender, GeneralTool.General.Models.ImageEventArgs e)
        {
            this.Continue();
        }

        private void Continue()
        {
            this.ImgControl.CanImageDraw = true;
            this.ImgControl.SendMouseCutRectStart();
            this.CutBtn.IsChecked = true;
        }

        private void ImgControl_CutMenuCancelEvent(object sender, EventArgs e)
        {
            this.Continue();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.ImgControl.CanMoveImage = !this.ImgControl.CanMoveImage;
            this.ImgControl.ResertImageSource();
        }

        private void CutBtn_Click(object sender, RoutedEventArgs e)
        {
            EnableControl(sender, !this.CutBtn.IsChecked.Value);

            if (this.CutBtn.IsChecked.Value)
            {
                this.UpdateChecked(sender);

                this.ImgControl.CanImageDraw = true;
                this.ImgControl.SendMouseCutRectStart();
            }

        }

        private void EnableControl(object sender, bool value)
        {
            foreach (var item in this.ToolButtons)
            {
                if (item is ToggleButton b)
                {
                    if (!b.Equals(sender))
                    {
                        b.IsEnabled = value;
                    }
                }
            }
        }

        private void UpdateChecked(object sender)
        {
            foreach (var item in this.ToolButtons)
            {
                if (item is ToggleButton b)
                {
                    if (!b.Equals(sender))
                    {
                        b.IsChecked = false;
                    }
                }
            }
        }

        private void ABtn_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateChecked(sender);
            this.ImgControl.CanImageDraw = false;

        }

        private void CutBtn_Checked(object sender, RoutedEventArgs e)
        {
            this.UpdateChecked(sender);

            Continue();
        }

        private void CutBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            this.ImgControl.SendMouseCutRectStart();
            this.ImgControl.CanImageDraw = false;
        }

        private void ABtn_Checked(object sender, RoutedEventArgs e)
        {
            this.ImgControl.CanImageDraw = false;
        }

        List<Point> points = new List<Point>();

        private Path path;
        private LineGeometry lineGeometry;
        private bool isMouseDown;
        private void ImgControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                points.Clear();
                points.Add(this.ImgControl.CurrentMouseDownPixelPoint);
                this.ImgControl.CanMoveImage = false;
                this.ImgControl.CanImageDraw = true;
                if (this.path != null)
                {
                    this.ImgControl.RemoveElement(this.path);
                    this.lineGeometry = null;
                    this.path = null;
                }

                this.ImgControl.ClearAll();
                var point = this.ImgControl.TranslatePoint(this.ImgControl.CurrentMouseDownPixelPoint);
                //line = new Line()
                //{
                //    Stroke = Brushes.Red,
                //    StrokeThickness = 3,
                //    X1 = point.X,
                //    Y1 = point.Y,
                //    X2 = point.X,
                //    Y2 = point.Y
                //};

                var p1 = this.ImgControl.TranslateToPixelPoint(point);

                this.path = new Path()
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 3,
                    Tag=this.points
                };
                this.lineGeometry = new LineGeometry()
                {
                    StartPoint = point,
                    EndPoint = point,
                    
                };
                this.path.Data = this.lineGeometry;
                this.ImgControl.AddElement(this.path);


                Trace.WriteLine("MouseDown");
               
                isMouseDown = true;
            }

        }

        private void ImgControl_ImageMouseMoveEvent(GeneralTool.General.Models.ImageMouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                points.Add(this.ImgControl.CurrentMouseDownPixelPoint);
                Trace.WriteLine("MouseMove -- point  = " + this.ImgControl.CurrentMouseDownPixelPoint);
                if (this.lineGeometry != null)
                {
                    var point = this.ImgControl.TranslatePoint(this.ImgControl.CurrentMouseDownPixelPoint);
                    if (Keyboard.GetKeyStates(Key.LeftCtrl) == KeyStates.Down)
                    {
                        Trace.WriteLine("------------------------");
                        //直线
                        var sp = this.lineGeometry.StartPoint;
                        var x = Math.Abs(point.X - sp.X);
                        var y = Math.Abs(point.Y - sp.Y);
                        if (x > y)
                        {
                            point.Y = sp.Y;
                        }
                        else
                        {
                            point.X = sp.X;
                        }
                    }

                    //this.line.X2 = point.X;
                    //this.line.Y2 = point.Y;
                    this.lineGeometry.EndPoint = point;
                    this.path.Tag = new List<Point>() { this.points.First(), this.points.Last() };
                }
            }

            var pos = this.ImgControl.CurrentMouseDownPixelPoint;
            this.PosTxt.Text = $"{(int)pos.X},{(int)pos.Y}";
        }


        private void ImgControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ImgControl.CanMoveImage = true;
            if (e.LeftButton == MouseButtonState.Released && this.isMouseDown)
            {
                points.Add(this.ImgControl.CurrentMouseDownPixelPoint);

                Trace.WriteLine("MouseUp  -- point  = " + this.ImgControl.CurrentMouseDownPixelPoint);

               
                isMouseDown = false;
            }
        }

        double widthScale = 1; double heightScale;
        private void ImgControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (this.lineGeometry!=null)
            //{
            ////计算出比例宽高
            if (e.PreviousSize.Width == 0 || e.PreviousSize.Height == 0)
            {
                return;
            }

            if (this.lineGeometry == null) return;

          var ps=this.path.Tag as List<Point>;

            var p11 = this.ImgControl.TranslatePoint(ps.First());
            var p22 = this.ImgControl.TranslatePoint(ps.Last());

            this.lineGeometry.StartPoint = p11;
            this.lineGeometry.EndPoint = p22;
        }

        Point point = new Point(1165.39297124601, 2125.9552715655);
        private void MenuItem_Click1(object sender, RoutedEventArgs e)
        {

            var p1 = this.ImgControl.TranslatePoint(point);
            var p2 = this.ImgControl.TranslateToPixelPoint(p1);
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            this.ImgControl.ResertImageSource();
        }
    }
}
