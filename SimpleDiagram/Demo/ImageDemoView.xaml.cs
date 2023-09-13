using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

using GeneralTool.CoreLibrary.Extensions;
using GeneralTool.CoreLibrary.Models;
using GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes;

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
            ImgControl.ImageMouseDownEvent += ImgControl_MouseDown;
            ImgControl.MouseUp += ImgControl_MouseUp;
        }

        private bool isDraw = false;
        private Path tmpPath = new Path();
        private Point startPoint;

        private void ImgControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                foreach (BaseShape item in ImgControl.CustomeShapes)
                {
                    item.ClearResizeRectAdorner();
                }
            }
            isDraw = e.RightButton == MouseButtonState.Pressed;

            if (isDraw)
            {
                ImgControl.CanMoveImage = false;
                startPoint = ImgControl.TranslateToCanvasPoint(ImgControl.CurrentMouseDownPixelPoint);
                if (RectRadio.IsChecked.Value || HeartRadio.IsChecked.Value)
                {

                    tmpPath = new Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1 / ImgControl.ImageScale,
                    };
                    //矩形绘制
                    RectangleGeometry geo = new RectangleGeometry();
                    tmpPath.Data = geo;

                    ImgControl.AddElement(tmpPath);
                }
                else if (LineRadio.IsChecked.Value)
                {
                    tmpPath = new Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1 / ImgControl.ImageScale,
                    };
                    //矩形绘制
                    LineGeometry geo = new LineGeometry(startPoint, startPoint);
                    tmpPath.Data = geo;

                    ImgControl.AddElement(tmpPath);
                }
                else if (PolygonRadio.IsChecked.Value)
                {
                    if (tmpPath.Data is PathGeometry p)
                    {
                        if (p.Figures[0].Segments.Count > 0)
                        {
                            p.Figures[0].Segments.Add(new LineSegment(startPoint, true));
                            return;
                        }
                    }

                    tmpPath = new Path()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1 / ImgControl.ImageScale,
                    };
                    //多边形
                    PathGeometry polgyon = new PathGeometry()
                    {
                        Figures = new PathFigureCollection()
                        {
                            new PathFigure()
                            {
                                StartPoint=startPoint,
                                 IsClosed=false,
                            }
                        }
                    };
                    tmpPath.Data = polgyon;

                    ImgControl.AddElement(tmpPath);
                }

            }

        }
        private void ImgControl_ImageMouseMoveEvent(ImageMouseEventArgs obj)
        {
            PosTxt.Text = obj.CurrentPixelPoint.ToDrawPoint() + " _ " + obj.CanvasPoint.ToDrawPoint();

            Point currPoint = obj.CanvasPoint;
            if (isDraw && obj.RightButton == MouseButtonState.Pressed)
            {
                if (RectRadio.IsChecked.Value || HeartRadio.IsChecked.Value)
                {
                    if (tmpPath.Data is RectangleGeometry g)
                    {
                        g.Rect = new Rect(startPoint, currPoint);
                    }
                }
                else if (LineRadio.IsChecked.Value)
                {
                    if (tmpPath.Data is LineGeometry l)
                    {
                        l.EndPoint = currPoint;
                    }
                }
                else if (tmpPath.Data is PathGeometry g)
                {
                    PathSegmentCollection Segments = g.Figures[0].Segments;
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

            ImgControl.RemoveElement(tmpPath);
            if (isDraw)
            {
                isDraw = false;

                ImgControl.CanMoveImage = true;
                if (PointRadio.IsChecked.Value)
                {
                    PointShape pointShape = new PointShape(ImgControl.CurrentMouseDownPixelPoint, 3);
                    pointShape.Path.Fill = Brushes.Red;
                    ImgControl.AddCustomeShape(pointShape);
                    PointRadio.IsChecked = false;
                }
                else if (LineRadio.IsChecked.Value)
                {
                    if (tmpPath.Data is LineGeometry l)
                    {
                        Point start = ImgControl.TranslateToPixelPoint(l.StartPoint);
                        Point end = ImgControl.TranslateToPixelPoint(l.EndPoint);
                        LineShape lineShape = new LineShape(start, end);
                        lineShape.Path.Stroke = Brushes.Red;
                        lineShape.Path.StrokeThickness = 1;
                        ImgControl.AddCustomeShape(lineShape);
                    }
                }
                else if (RectRadio.IsChecked.Value || HeartRadio.IsChecked.Value)
                {
                    if (tmpPath.Data is RectangleGeometry g)
                    {
                        if (g.Rect.IsEmpty) return;
                        Rect rect = g.Rect;
                        Point lt = rect.TopLeft;
                        Point rb = rect.BottomRight;
                        lt = ImgControl.TranslateToPixelPoint(lt);
                        rb = ImgControl.TranslateToPixelPoint(rb);

                        BaseShape shape = null;
                        if (RectRadio.IsChecked.Value)
                        {
                            shape = new RectShape(new Rect(lt, rb));
                        }
                        else if (HeartRadio.IsChecked.Value)
                        {
                            shape = new HeartShape(new Rect(lt, rb));
                        }

                        shape.Path.Stroke = Brushes.Red;
                        shape.Path.StrokeThickness = 1;
                        ImgControl.AddCustomeShape(shape);
                        //shape.MouseDown += Path_MouseDown;
                    }

                    RectRadio.IsChecked = false;
                }
                else if (PolygonRadio.IsChecked.Value)
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
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog().Value)
            {
                using (System.Drawing.Bitmap map = new System.Drawing.Bitmap(dialog.FileName))
                {
                    ImgControl.Bitmap = map;
                }

            }
        }

        private void PointRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PolygonRadio_Unchecked(object sender, RoutedEventArgs e)
        {
            isDraw = false;

            ImgControl.CanMoveImage = true;

            bool isClosed = false;
            if (MessageBox.Show("是否需要闭合", "提示", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                isClosed = true;
            }

            if (tmpPath.Data is PathGeometry g)
            {
                PolygonShape shape = new PolygonShape();
                foreach (PathFigure item in g.Figures)
                {
                    Point start = ImgControl.TranslateToPixelPoint(item.StartPoint);
                    shape.PixelPoints.Add(start);
                    foreach (LineSegment p in item.Segments)
                    {
                        Point tmpp = ImgControl.TranslateToPixelPoint(p.Point);
                        shape.PixelPoints.Add(tmpp);
                    }
                }

                if (isClosed)
                {
                    shape.PixelPoints.Add(shape.PixelPoints[0]);
                }

                shape.Path.Stroke = Brushes.Red;
                shape.Path.StrokeThickness = 1;
                ImgControl.AddCustomeShape(shape);
            }
            ImgControl.RemoveElement(tmpPath);
            tmpPath.Data = null;
        }

        private void ScaleClick(object sender, RoutedEventArgs e)
        {
            System.Collections.ObjectModel.ObservableCollection<Point> points = ImgControl.CustomeShapes[0].PixelPoints;
            Rect rect = new Rect(points[0], points[2]);
            ImgControl.SaveCutRectBitmap(new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height), "1.bmp", GeneralTool.CoreLibrary.Enums.BitmapEncoderEnum.Bmp);
        }

        private bool loop = true;
        private async void LoopImage(object sender, RoutedEventArgs e)
        {
            if (LoopMenu.Header + "" == "Loop")
            {
                LoopMenu.Header = "Stop";
                await Task.Run(() =>
                {
                    string dir = @"C:\Users\raozh\Pictures\Camera";
                    System.Collections.Generic.IEnumerable<string> files = System.IO.Directory.EnumerateFiles(dir);

                    while (loop)
                    {
                        foreach (string item in files)
                        {
                            if (!loop)
                            {
                                break;
                            }

                            //load image
                            Dispatcher.Invoke(() =>
                            {
                                ImgControl.SourcePath = item;
                            });

                            //Thread.Sleep(10);
                        }
                    }
                });
                loop = true;
            }
            else
            {
                loop = false;
                LoopMenu.Header = "Loop";
            }
        }

        private void RemoveFirstClick(object sender, RoutedEventArgs e)
        {
            if (ImgControl.CustomeShapes.Count > 0)
            {
                ImgControl.RemoveCustomeShape(ImgControl.CustomeShapes[0]);
            }
        }
    }
}
