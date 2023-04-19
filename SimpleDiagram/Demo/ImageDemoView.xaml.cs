using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using GeneralTool.General.Extensions;
using GeneralTool.General.WPFHelper;
using GeneralTool.General.WPFHelper.WPFControls.Shapes;

using Microsoft.Win32;

using SimpleDiagram.Shapes;

namespace SimpleDiagram.Demo
{
    /// <summary>
    /// ImageDemoView.xaml 的交互逻辑
    /// </summary>
    public partial class ImageDemoView : UserControl
    {
        public ImageDemoView()
        {
            InitializeComponent();
            this.ImgControl.ImageMouseDownEvent += ImgControl_MouseDown;
            this.ImgControl.MouseUp += ImgControl_MouseUp;
        }


        private bool isDraw = false;
        private Path tmpPath = new Path();
        private Point startPoint;

        private void ImgControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                foreach (var item in this.ImgControl.CustomeShapes)
                {
                    item.ClearResizeRectAdorner();
                }
            }
            isDraw = e.RightButton == MouseButtonState.Pressed;

            if (isDraw)
            {
                this.ImgControl.CanMoveImage = false;
                this.startPoint = this.ImgControl.TranslateToCanvasPoint(this.ImgControl.CurrentMouseDownPixelPoint);
                if (this.RectRadio.IsChecked.Value || this.HeartRadio.IsChecked.Value)
                {

                    this.tmpPath = new Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1 / this.ImgControl.ImageScale,
                    };
                    //矩形绘制
                    var geo = new RectangleGeometry();
                    tmpPath.Data = geo;

                    this.ImgControl.AddElement(tmpPath);
                }
                else if (this.LineRadio.IsChecked.Value)
                {
                    this.tmpPath = new Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1 / this.ImgControl.ImageScale,
                    };
                    //矩形绘制
                    var geo = new LineGeometry(startPoint, startPoint);
                    tmpPath.Data = geo;

                    this.ImgControl.AddElement(tmpPath);
                }
                else if (this.PolygonRadio.IsChecked.Value)
                {
                    if (this.tmpPath.Data is PathGeometry p)
                    {
                        if (p.Figures[0].Segments.Count > 0)
                        {
                            p.Figures[0].Segments.Add(new LineSegment(this.startPoint, true));
                            return;
                        }
                    }

                    this.tmpPath = new Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1 / this.ImgControl.ImageScale,
                    };
                    //多边形
                    var polgyon = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                StartPoint=this.startPoint,
                                 IsClosed=false,
                            }
                        }
                    };
                    this.tmpPath.Data = polgyon;

                    this.ImgControl.AddElement(tmpPath);
                }

            }

        }
        private void ImgControl_ImageMouseMoveEvent(GeneralTool.General.Models.ImageMouseEventArgs obj)
        {
            this.PosTxt.Text = obj.CurrentPixelPoint.ToDrawPoint() + " _ " + obj.CanvasPoint.ToDrawPoint();

            var currPoint = obj.CanvasPoint;
            if (this.isDraw && obj.RightButton == MouseButtonState.Pressed)
            {
                if (this.RectRadio.IsChecked.Value || this.HeartRadio.IsChecked.Value)
                {
                    if (this.tmpPath.Data is RectangleGeometry g)
                    {
                        g.Rect = new Rect(this.startPoint, currPoint);
                    }
                }
                else if (this.LineRadio.IsChecked.Value)
                {
                    if (this.tmpPath.Data is LineGeometry l)
                    {
                        l.EndPoint = currPoint;
                    }
                }
                else if (this.tmpPath.Data is PathGeometry g)
                {
                    var Segments = g.Figures[0].Segments;
                    if (Segments.Count > 0)
                    {
                        Segments.RemoveAt(Segments.Count - 1);
                    }
                    Segments.Add(new LineSegment(currPoint, true));
                }
            }

        }

        private void ImgControl_MouseUp(object sender, MouseButtonEventArgs e)
        {

            this.ImgControl.RemoveElement(this.tmpPath);
            if (this.isDraw)
            {
                this.isDraw = false;

                this.ImgControl.CanMoveImage = true;
                if (this.PointRadio.IsChecked.Value)
                {
                    var pointShape = new PointShape(this.ImgControl, this.ImgControl.CurrentMouseDownPixelPoint, 3);
                    pointShape.Path.Fill = Brushes.Red;
                    this.ImgControl.AddCustomeShape(pointShape);
                    this.PointRadio.IsChecked = false;
                }
                else if (this.LineRadio.IsChecked.Value)
                {
                    if (this.tmpPath.Data is LineGeometry l)
                    {
                        var start = this.ImgControl.TranslateToPixelPoint(l.StartPoint);
                        var end = this.ImgControl.TranslateToPixelPoint(l.EndPoint);
                        var lineShape = new LineShape(this.ImgControl, start, end);
                        lineShape.Path.Stroke = Brushes.Red;
                        lineShape.Path.StrokeThickness = 1;
                        this.ImgControl.AddCustomeShape(lineShape);
                    }
                }
                else if (this.RectRadio.IsChecked.Value || this.HeartRadio.IsChecked.Value)
                {
                    if (this.tmpPath.Data is RectangleGeometry g)
                    {
                        if (g.Rect.IsEmpty) return;
                        var rect = g.Rect;
                        var lt = rect.TopLeft;
                        var rb = rect.BottomRight;
                        lt = this.ImgControl.TranslateToPixelPoint(lt);
                        rb = this.ImgControl.TranslateToPixelPoint(rb);

                        BaseShape shape = null;
                        if (this.RectRadio.IsChecked.Value)
                        {
                            shape = new RectShape(this.ImgControl, new Rect(lt, rb));
                        }
                        else if (this.HeartRadio.IsChecked.Value)
                        {
                            shape = new HeartShape(this.ImgControl, new Rect(lt, rb));
                        }

                        shape.Path.Stroke = Brushes.Red;
                        shape.Path.StrokeThickness = 1;
                        this.ImgControl.AddCustomeShape(shape);
                        //shape.MouseDown += Path_MouseDown;
                    }


                    this.RectRadio.IsChecked = false;
                }
                else if (this.PolygonRadio.IsChecked.Value)
                {
                    return;
                }
            }


        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void LoadImage(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog().Value)
            {
                using (var map = new System.Drawing.Bitmap(dialog.FileName))
                {
                    this.ImgControl.Bitmap = map;
                }

            }
        }

        private void PointRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PolygonRadio_Unchecked(object sender, RoutedEventArgs e)
        {
            this.isDraw = false;

            this.ImgControl.CanMoveImage = true;

            var isClosed = false;
            if (MessageBox.Show("是否需要闭合", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                isClosed = true;
            }

            if (this.tmpPath.Data is PathGeometry g)
            {
                var shape = new PolygonShape(this.ImgControl);
                foreach (var item in g.Figures)
                {
                    var start = this.ImgControl.TranslateToPixelPoint(item.StartPoint);
                    shape.PixelPoints.Add(start);
                    foreach (LineSegment p in item.Segments)
                    {
                        var tmpp = this.ImgControl.TranslateToPixelPoint(p.Point);
                        shape.PixelPoints.Add(tmpp);
                    }
                }

                if (isClosed)
                {
                    shape.PixelPoints.Add(shape.PixelPoints[0]);
                }

                shape.Path.Stroke = Brushes.Red;
                shape.Path.StrokeThickness = 1;
                this.ImgControl.AddCustomeShape(shape);
            }
            this.ImgControl.RemoveElement(this.tmpPath);
            this.tmpPath.Data = null;
        }

        private void ScaleClick(object sender, RoutedEventArgs e)
        {
            var points = this.ImgControl.CustomeShapes[0].PixelPoints;
            var rect = new Rect(points[0], points[2]);
            this.ImgControl.ImageSource.SaveBitmapSouce(new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), "1.bmp");
        }

        private bool loop = true;
        private async void LoopImage(object sender, RoutedEventArgs e)
        {
            if (this.LoopMenu.Header + "" == "Loop")
            {
                this.LoopMenu.Header = "Stop";
                await Task.Run(() =>
                {
                    var dir = @"C:\Users\raozh\Pictures\Camera";
                    var files = System.IO.Directory.EnumerateFiles(dir);

                    while (loop)
                    {
                        foreach (var item in files)
                        {
                            if (!loop)
                            {
                                break;
                            }

                            //load image
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ImgControl.SourcePath = item;
                            });

                            //Thread.Sleep(10);
                        }
                    }
                });
                this.loop = true;
            }
            else
            {
                this.loop = false;
                this.LoopMenu.Header = "Loop";
            }
        }

        private void RemoveFirstClick(object sender, RoutedEventArgs e)
        {
            if (this.ImgControl.CustomeShapes.Count > 0)
            {
                this.ImgControl.RemoveCustomeShape(this.ImgControl.CustomeShapes[0]);
            }
        }
    }
}
