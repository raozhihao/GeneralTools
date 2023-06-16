using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 形状基类
    /// </summary>
    public abstract class BaseShape : BaseNotifyModel, IDisposable
    {

        /// <summary>
        /// 获取当前图形的所有坐标点
        /// </summary>
        public ObservableCollection<Point> PixelPoints { get; set; } = new ObservableCollection<Point>();

        /// <summary>
        /// 当前图形
        /// </summary>
        public Path Path { get; } = new Path();

        private bool autoScale = true;
        /// <summary>
        /// 是否允许在画面缩放时图形的大小或线条跟随缩放,默认为true
        /// </summary>
        public bool AutoScale
        {
            get => autoScale;
            set => RegisterProperty(ref autoScale, value);
        }

        private bool canMoveShape = true;

        /// <summary>
        /// 是否允许拖动图形位置,默认true
        /// </summary>
        public bool CanMoveShape
        {
            get => canMoveShape;
            set => RegisterProperty(ref canMoveShape, value);
        }

        private bool canMoveOutBound = false;

        /// <summary>
        /// 是否允许图形拖出画面边界,默认false
        /// </summary>
        public bool CanMoveOutBound
        {
            get => canMoveOutBound;
            set => RegisterProperty(ref canMoveOutBound, value);
        }

        private double strokeThickness = 0;

        /// <summary>
        /// 描边大小,默认为0
        /// </summary>
        internal double StrokeThickness
        {
            get => strokeThickness;
            set => RegisterProperty(ref strokeThickness, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public ImageViewControl ImageView { get; set; }

        private List<Point> movePoints = new List<Point>();

        /// <summary>
        /// 鼠标点击时事件
        /// </summary>
        public event MouseButtonEventHandler MouseDown;

        /// <summary>
        /// 
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// 
        /// </summary>
        public event MouseButtonEventHandler MouseUp;

        /// <summary>
        /// 大小更改时的事件通知
        /// </summary>
        public event EventHandler<ResizeEventArgs> ResizeChingingEventHandler;

        private ResizeRectAdorner rectAdorner;
        private AdornerLayer adornerLayer;

        internal void Init(ImageViewControl viewControl)
        {
            ImageView = viewControl;
            Path.IsHitTestVisible = true;

            adornerLayer = AdornerLayer.GetAdornerLayer(Path);
            rectAdorner = new ResizeRectAdorner(this);

            ImageView.PreviewMouseDown += PathGeometry_PreviewMouseDown;
            ImageView.PreviewMouseMove += PathGeometry_PreviewMouseMove;
            ImageView.PreviewMouseUp += PathGeometry_PreviewMouseUp;
            ImageView.CanvasSizeChanged += ViewControl_CanvasSizeChanged;
            Path.MouseEnter += Path_MouseEnter;
            Path.MouseLeave += Path_MouseLeave;
            rectAdorner.ResizeChingingEventHandler += RectAdorner_ResizeChingingEventHandler;
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            Path.StrokeThickness = StrokeThickness / ImageView.ImageScale;
            Path.Cursor = null;
        }

        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            Path.StrokeThickness = (StrokeThickness + 2) / ImageView.ImageScale;
            Path.Cursor = Cursors.SizeAll;
        }

        private void RectAdorner_ResizeChingingEventHandler(object sender, ResizeEventArgs e)
        {
            ResizeChingingEventHandler?.Invoke(sender, e);
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        public abstract void CreateShape();

        private void PathGeometry_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Path.Cursor = null;

            Path.ReleaseMouseCapture();
            if (e.OriginalSource != Path) return;

            Path.StrokeThickness = StrokeThickness / ImageView.ImageScale;
            MouseUp?.Invoke(sender, e);
            if (e.Handled) return;

            if (mouseDownPoint.HasValue && CanMoveShape)
            {
                mouseDownPoint = null;
                //更新坐标
                for (int i = 0; i < movePoints.Count; i++)
                {
                    PixelPoints[i] = movePoints[i];
                }
                e.Handled = true;
            }
        }

        private void PathGeometry_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource != Path) return;
            MouseMove?.Invoke(sender, e);
            if (e.Handled) return;
            if (e.LeftButton == MouseButtonState.Pressed && mouseDownPoint.HasValue && CanMoveShape
                && Path.IsMouseCaptured)
            {
                //移动
                Point curr = ImageView.GetCurrentPixelPoint(e);
                //更改的距离
                Vector vector = curr - mouseDownPoint.Value;
                MovePoints(vector);
            }
        }

        /// <summary>
        /// 鼠标在图形上按下
        /// </summary>
        private Point? mouseDownPoint;
        private void PathGeometry_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != Path) return;
            MouseDown?.Invoke(sender, e);
            if (e.Handled) return;
            if (e.LeftButton == MouseButtonState.Pressed && CanMoveShape && e.OriginalSource == Path)
            {
                Path.Cursor = Cursors.SizeAll;
                mouseDownPoint = ImageView.GetCurrentPixelPoint(e);
                _ = Path.CaptureMouse();

                Path.StrokeThickness = (StrokeThickness + 2) / ImageView.ImageScale;
                if (PixelPoints.Count > 1)
                {
                    if (adornerLayer == null)
                        adornerLayer = AdornerLayer.GetAdornerLayer(Path);
                    if (adornerLayer.GetAdorners(Path) == null || !adornerLayer.GetAdorners(Path).Contains(rectAdorner))
                    {
                        adornerLayer.Add(rectAdorner);
                    }
                }

                e.Handled = true;
            }
        }

        private void ViewControl_CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            MovePoints(new Vector());
            //更新大小

            UpdateScaleSize(ImageView.ImageScale);
            e.Handled = true;
        }

        /// <summary>
        /// 在图像进行尺寸更新及缩放时更新其描边线条的宽度
        /// </summary>
        /// <param name="imageScale"></param>
        public virtual void UpdateScaleSize(double imageScale)
        {
            if (!AutoScale) return;

            //更新大小
            double scale = StrokeThickness / imageScale;
            Path.StrokeThickness = scale;
        }

        /// <summary>
        /// 使用canvaspoints更新pixelPoints
        /// </summary>
        /// <param name="canvasPoints"></param>
        public void UpdateShapePoints(List<Point> canvasPoints)
        {
            //将当前的点都转为canvas点
            List<Point> tmpMovePoints = new List<Point>();

            for (int i = 0; i < canvasPoints.Count; i++)
            {
                Point item = canvasPoints[i];
                Point p = ImageView.TranslateToPixelPoint(item);
                //查看是否越过图像边界
                if (!CanMoveOutBound)
                {
                    if (p.X < 0 || p.Y < 0)
                    {
                        return;
                    }

                    System.Windows.Media.ImageSource img = ImageView.ImageSource;
                    if (img != null)
                    {
                        if (img.Width < p.X || img.Height < p.Y)
                        {
                            return;
                        }
                    }

                }
                tmpMovePoints.Add(p);
            }
            movePoints = tmpMovePoints;

            UpdateShape(canvasPoints);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePixelpoints()
        {
            //更新坐标
            for (int i = 0; i < movePoints.Count; i++)
            {
                PixelPoints[i] = movePoints[i];
            }
        }

        /// <summary>
        /// 更新坐标
        /// </summary>
        /// <param name="vector"></param>
        public void MovePoints(Vector vector)
        {
            //将当前的点都转为canvas点
            List<Point> tmpMovePoints = new List<Point>();
            List<Point> canvasPoints = new List<Point>();
            for (int i = 0; i < PixelPoints.Count; i++)
            {
                Point point = PixelPoints[i];
                Point p = point;
                p.X += vector.X;
                p.Y += vector.Y;
                //查看是否越过图像边界
                if (!CanMoveOutBound)
                {
                    if (p.X < 0 || p.Y < 0)
                    {
                        return;
                    }

                    System.Windows.Media.ImageSource img = ImageView.ImageSource;
                    if (img != null)
                    {
                        if (img.Width < p.X || img.Height < p.Y)
                        {
                            return;
                        }
                    }

                }

                tmpMovePoints.Add(p);
                p = ImageView.TranslateToCanvasPoint(p);
                canvasPoints.Add(p);
            }

            movePoints = tmpMovePoints;
            UpdateShape(canvasPoints);
        }

        /// <summary>
        /// 更新坐标
        /// </summary>
        /// <param name="canvasPoints"></param>
        public abstract void UpdateShape(List<Point> canvasPoints);

        /// <summary>
        /// 重新绘制图形(根据现有属性)
        /// </summary>
        public virtual void ReDrawShape()
        {
            ImageView.RemoveCustomeShape(this);
            CreateShape();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            ImageView.PreviewMouseDown -= PathGeometry_PreviewMouseDown;
            ImageView.PreviewMouseMove -= PathGeometry_PreviewMouseMove;
            ImageView.PreviewMouseUp -= PathGeometry_PreviewMouseUp;
            ImageView.CanvasSizeChanged += ViewControl_CanvasSizeChanged;
            Path.MouseEnter -= Path_MouseEnter;
            Path.MouseLeave -= Path_MouseLeave;
            rectAdorner.ResizeChingingEventHandler -= RectAdorner_ResizeChingingEventHandler;
            ClearResizeRectAdorner();
        }

        /// <summary>
        /// 清除外侧尺寸调整框
        /// </summary>
        public void ClearResizeRectAdorner()
        {
            if (adornerLayer != null)
            {
                Adorner[] adorners = adornerLayer.GetAdorners(Path);
                if (adorners == null || adorners.Length == 0)
                {
                    return;
                }

                foreach (Adorner item in adorners)
                {
                    adornerLayer.Remove(item);
                }
            }
        }

    }

}
