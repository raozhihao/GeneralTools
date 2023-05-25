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
            get => this.autoScale;
            set => this.RegisterProperty(ref this.autoScale, value);
        }

        private bool canMoveShape = true;

        /// <summary>
        /// 是否允许拖动图形位置,默认true
        /// </summary>
        public bool CanMoveShape
        {
            get => this.canMoveShape;
            set => this.RegisterProperty(ref this.canMoveShape, value);
        }

        private bool canMoveOutBound = false;

        /// <summary>
        /// 是否允许图形拖出画面边界,默认false
        /// </summary>
        public bool CanMoveOutBound
        {
            get => this.canMoveOutBound;
            set => this.RegisterProperty(ref this.canMoveOutBound, value);
        }


        private double strokeThickness = 0;

        /// <summary>
        /// 描边大小,默认为0
        /// </summary>
        internal double StrokeThickness
        {
            get => this.strokeThickness;
            set => this.RegisterProperty(ref this.strokeThickness, value);
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
            this.ImageView = viewControl;
            this.Path.IsHitTestVisible = true;

            this.adornerLayer = AdornerLayer.GetAdornerLayer(this.Path);
            this.rectAdorner = new ResizeRectAdorner(this);

            this.ImageView.PreviewMouseDown += PathGeometry_PreviewMouseDown;
            this.ImageView.PreviewMouseMove += PathGeometry_PreviewMouseMove;
            this.ImageView.PreviewMouseUp += PathGeometry_PreviewMouseUp;
            this.ImageView.CanvasSizeChanged += ViewControl_CanvasSizeChanged;
            this.Path.MouseEnter += Path_MouseEnter;
            this.Path.MouseLeave += Path_MouseLeave;
            this.rectAdorner.ResizeChingingEventHandler += RectAdorner_ResizeChingingEventHandler;
        }


        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Path.StrokeThickness = this.StrokeThickness / this.ImageView.ImageScale;
            this.Path.Cursor = null;
        }

        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Path.StrokeThickness = (this.StrokeThickness + 2) / this.ImageView.ImageScale;
            this.Path.Cursor = Cursors.SizeAll;
        }

        private void RectAdorner_ResizeChingingEventHandler(object sender, ResizeEventArgs e)
        {
            this.ResizeChingingEventHandler?.Invoke(sender, e);
        }


        /// <summary>
        /// 创建图形
        /// </summary>
        public abstract void CreateShape();

        private void PathGeometry_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Path.Cursor = null;

            this.Path.ReleaseMouseCapture();
            if (e.OriginalSource != this.Path) return;

            this.Path.StrokeThickness = this.StrokeThickness / this.ImageView.ImageScale;
            this.MouseUp?.Invoke(sender, e);
            if (e.Handled) return;

            if (this.mouseDownPoint.HasValue && this.CanMoveShape)
            {
                this.mouseDownPoint = null;
                //更新坐标
                for (int i = 0; i < this.movePoints.Count; i++)
                {
                    this.PixelPoints[i] = this.movePoints[i];
                }
                e.Handled = true;
            }
        }

        private void PathGeometry_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource != this.Path) return;
            this.MouseMove?.Invoke(sender, e);
            if (e.Handled) return;
            if (e.LeftButton == MouseButtonState.Pressed && this.mouseDownPoint.HasValue && this.CanMoveShape
                && this.Path.IsMouseCaptured)
            {
                //移动
                var curr = this.ImageView.GetCurrentPixelPoint(e);
                //更改的距离
                var vector = curr - this.mouseDownPoint.Value;
                this.MovePoints(vector);
            }
        }


        /// <summary>
        /// 鼠标在图形上按下
        /// </summary>
        private Point? mouseDownPoint;
        private void PathGeometry_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != this.Path) return;
            this.MouseDown?.Invoke(sender, e);
            if (e.Handled) return;
            if (e.LeftButton == MouseButtonState.Pressed && this.CanMoveShape && e.OriginalSource == this.Path)
            {
                this.Path.Cursor = Cursors.SizeAll;
                this.mouseDownPoint = this.ImageView.GetCurrentPixelPoint(e);
                this.Path.CaptureMouse();

                this.Path.StrokeThickness = (this.StrokeThickness + 2) / this.ImageView.ImageScale;
                if (this.PixelPoints.Count > 1)
                {
                    if (this.adornerLayer == null)
                        this.adornerLayer = AdornerLayer.GetAdornerLayer(this.Path);
                    if (this.adornerLayer.GetAdorners(this.Path) == null || !this.adornerLayer.GetAdorners(this.Path).Contains(this.rectAdorner))
                    {
                        adornerLayer.Add(rectAdorner);
                    }
                }

                e.Handled = true;
            }
        }



        private void ViewControl_CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.MovePoints(new Vector());
            //更新大小

            this.UpdateScaleSize(this.ImageView.ImageScale);
            e.Handled = true;
        }

        /// <summary>
        /// 在图像进行尺寸更新及缩放时更新其描边线条的宽度
        /// </summary>
        /// <param name="imageScale"></param>
        public virtual void UpdateScaleSize(double imageScale)
        {
            if (!this.AutoScale) return;

            //更新大小
            var scale = this.StrokeThickness / imageScale;
            this.Path.StrokeThickness = scale;
        }

        /// <summary>
        /// 使用canvaspoints更新pixelPoints
        /// </summary>
        /// <param name="canvasPoints"></param>
        public void UpdateShapePoints(List<Point> canvasPoints)
        {
            //将当前的点都转为canvas点
            var tmpMovePoints = new List<Point>();

            for (int i = 0; i < canvasPoints.Count; i++)
            {
                var item = canvasPoints[i];
                var p = this.ImageView.TranslateToPixelPoint(item);
                //查看是否越过图像边界
                if (!this.CanMoveOutBound)
                {
                    if (p.X < 0 || p.Y < 0)
                    {
                        return;
                    }

                    var img = this.ImageView.ImageSource;
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
            this.movePoints = tmpMovePoints;

            this.UpdateShape(canvasPoints);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePixelpoints()
        {
            //更新坐标
            for (int i = 0; i < this.movePoints.Count; i++)
            {
                this.PixelPoints[i] = this.movePoints[i];
            }
        }

        /// <summary>
        /// 更新坐标
        /// </summary>
        /// <param name="vector"></param>
        public void MovePoints(Vector vector)
        {
            //将当前的点都转为canvas点
            var tmpMovePoints = new List<Point>();
            var canvasPoints = new List<Point>();
            for (int i = 0; i < this.PixelPoints.Count; i++)
            {
                var point = this.PixelPoints[i];
                var p = point;
                p.X += vector.X;
                p.Y += vector.Y;
                //查看是否越过图像边界
                if (!this.CanMoveOutBound)
                {
                    if (p.X < 0 || p.Y < 0)
                    {
                        return;
                    }

                    var img = this.ImageView.ImageSource;
                    if (img != null)
                    {
                        if (img.Width < p.X || img.Height < p.Y)
                        {
                            return;
                        }
                    }

                }

                tmpMovePoints.Add(p);
                p = this.ImageView.TranslateToCanvasPoint(p);
                canvasPoints.Add(p);
            }

            this.movePoints = tmpMovePoints;
            this.UpdateShape(canvasPoints);
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
            this.ImageView.RemoveCustomeShape(this);
            this.CreateShape();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.ImageView.PreviewMouseDown -= PathGeometry_PreviewMouseDown;
            this.ImageView.PreviewMouseMove -= PathGeometry_PreviewMouseMove;
            this.ImageView.PreviewMouseUp -= PathGeometry_PreviewMouseUp;
            this.ImageView.CanvasSizeChanged += ViewControl_CanvasSizeChanged;
            this.Path.MouseEnter -= Path_MouseEnter;
            this.Path.MouseLeave -= Path_MouseLeave;
            this.rectAdorner.ResizeChingingEventHandler -= RectAdorner_ResizeChingingEventHandler;
            this.ClearResizeRectAdorner();
        }

        /// <summary>
        /// 清除外侧尺寸调整框
        /// </summary>
        public void ClearResizeRectAdorner()
        {
            if (this.adornerLayer != null)
            {
                var adorners = this.adornerLayer.GetAdorners(this.Path);
                if (adorners == null || adorners.Length == 0)
                {
                    return;
                }

                foreach (var item in adorners)
                {
                    this.adornerLayer.Remove(item);
                }
            }
        }

    }

}
