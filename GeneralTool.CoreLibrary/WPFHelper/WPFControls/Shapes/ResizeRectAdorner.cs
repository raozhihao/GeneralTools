using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 
    /// </summary>
    public class ResizeRectAdorner : Adorner
    {
        private readonly BaseShape shape;
        private Pen pen;
        private const int InflateScale = 10;
        /// <summary>
        /// 大小更改时的事件通知
        /// </summary>
        public event EventHandler<ResizeEventArgs> ResizeChingingEventHandler;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shape"></param>
        public ResizeRectAdorner(BaseShape shape) : base(shape.Path)
        {
            this.shape = shape;
        }

        /// <inheritdoc/>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!shape.Path.IsMouseOver) return;
            Rect data = shape.Path.Data.Bounds;
            data.Inflate(InflateScale / shape.ImageView.ImageScale, InflateScale / shape.ImageView.ImageScale);
            base.OnRender(drawingContext);

            pen = new Pen(Brushes.Beige, 4 / shape.ImageView.ImageScale)
            {
                DashStyle = new DashStyle()
                {
                    Dashes = new DoubleCollection() { 0, 2 },
                    Offset = 1
                }
            };
            drawingContext.DrawRectangle(null, pen, data);
        }

        private Point? mouseStartDown;
        private DrawType drawType;

        /// <inheritdoc/>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _ = CaptureMouse();

            mouseStartDown = e.GetPosition(this);

            e.Handled = true;

            base.OnPreviewMouseLeftButtonDown(e);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Resize
                ResizePath(e);
            }
            else
            {
                //show resize cursor
                ShowReizeCuursors();
            }
            e.Handled = true;

            base.OnPreviewMouseMove(e);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {

            ReleaseMouseCapture();

            mouseStartDown = null;

            shape.UpdatePixelpoints();

            base.OnPreviewMouseUp(e);
        }

        private void ResizePath(MouseEventArgs e)
        {
            if (shape.PixelPoints.Count == 1) return;

            if (e.LeftButton != MouseButtonState.Pressed || mouseStartDown == null || Cursor == null) return;

            //鼠标当前的位置
            Point currPoint = e.GetPosition(this);

            //偏移量
            Vector vector = currPoint - mouseStartDown.Value;

            //确保只能向一个方向调整
            switch (drawType)
            {
                case DrawType.Left:
                case DrawType.Right:
                    vector = new Vector(vector.X, 0);
                    break;
                case DrawType.Top:
                case DrawType.Bottom:
                    vector = new Vector(0, vector.Y);
                    break;
            }

            //将当前像素坐标全部转为canvas坐标,并找出其四个方向的最小最大坐标
            StringBuilder canvasGeoBuilder = new StringBuilder();
            List<Point> canvasPoints = new List<Point>();
            System.Collections.ObjectModel.ObservableCollection<Point> pixelPoints = shape.PixelPoints;
            Point first = pixelPoints[0];
            Point firstCanvas = shape.ImageView.TranslateToCanvasPoint(first);
            canvasPoints.Add(firstCanvas);
            _ = canvasGeoBuilder.Append($"M{firstCanvas}");

            int i;
            for (i = 1; i < pixelPoints.Count; i++)
            {
                Point p = shape.ImageView.TranslateToCanvasPoint(pixelPoints[i]);
                canvasPoints.Add(p);
                _ = canvasGeoBuilder.Append($"L{p}");
            }

            Geometry geo = Geometry.Parse(canvasGeoBuilder.ToString());
            Rect geoBounds = geo.Bounds;
            int left = (int)geoBounds.Left;
            int right = (int)geoBounds.Right;
            int top = (int)geoBounds.Top;
            int bottom = (int)geoBounds.Bottom;

            int count = 0;
            //判断偏移方向
            for (i = 0; i < canvasPoints.Count; i++)
            {
                Point item = canvasPoints[i];

                if (drawType == DrawType.Left && (int)item.X >= right)
                {
                    count++;
                    continue;
                }
                else if (drawType == DrawType.Right && (int)item.X <= left)
                {
                    count++;
                    continue;
                }
                else if (drawType == DrawType.Top && (int)item.Y >= bottom)
                {
                    count++;
                    continue;
                }
                else if (drawType == DrawType.Bottom && (int)item.Y <= top)
                {
                    count++;
                    continue;
                }

                item = new Point(item.X + vector.X, item.Y + vector.Y);
                canvasPoints[i] = item;
            }

            ResizeEventArgs resizeInfo = new ResizeEventArgs()
            {
                PixelPoints = ParsePixelPoints(canvasPoints)
            };
            ResizeChingingEventHandler?.Invoke(this, resizeInfo);
            if (resizeInfo.Handled) return;

            //进行更新
            shape.UpdateShapePoints(canvasPoints);
            InvalidateVisual();
        }

        private List<Point> ParsePixelPoints(List<Point> canvasPoints)
        {
            List<Point> list = new List<Point>();
            foreach (Point item in canvasPoints)
            {
                list.Add(shape.ImageView.TranslateToPixelPoint(item));
            }
            return list;
        }

        private void ShowReizeCuursors()
        {
            //确定当前鼠标点的位置
            if (shape.PixelPoints.Count == 1) return;

            Point mousePoint = Mouse.GetPosition(this);

            Rect bound = shape.Path.Data.Bounds;
            bound.Inflate(InflateScale / shape.ImageView.ImageScale, InflateScale / shape.ImageView.ImageScale);

            //判断
            double xDis = bound.Right - mousePoint.X;
            double leftXDis = bound.Left - mousePoint.X;
            double yDis = bound.Bottom - mousePoint.Y;
            double topYDis = bound.Top - mousePoint.Y;

            double scale = shape.ImageView.ImageScale;
            //判断鼠标是否在右边框
            bool mouseInRightLine = xDis > -1 / scale && xDis < 3 / scale && mousePoint.Y > (bound.Top + 1 / scale) && mousePoint.Y < bound.Bottom - 1 / scale;

            //判断鼠标是否在左边框
            bool mouseInLeftLine = leftXDis > -1 / scale && leftXDis < 3 / scale && mousePoint.Y > (bound.Top + 1 / scale) && mousePoint.Y < bound.Bottom - 1 / scale;

            //判断鼠标是否在下边框
            bool mouseInBottomLine = yDis > -1 / scale && yDis < 3 / scale && mousePoint.X > bound.Left + 1 / scale && mousePoint.X < bound.Right - 1 / scale;

            //判断鼠标是否在上边框
            bool mouseInTopLine = topYDis > -1 / scale && topYDis < 3 / scale && mousePoint.X > bound.Left + 1 / scale && mousePoint.X < bound.Right - 1 / scale;

            //右边的显示应该处于右顶点到右底点之间
            if (mouseInRightLine)
            {
                //处于右边
                Cursor = Cursors.SizeWE;
                drawType = DrawType.Right;
            }
            else if (mouseInLeftLine)
            {
                //处于左边
                Cursor = Cursors.SizeWE;
                drawType = DrawType.Left;
            }
            else if (mouseInBottomLine)
            {
                //处于下方
                Cursor = Cursors.SizeNS;
                drawType = DrawType.Bottom;
            }
            else if (mouseInTopLine)
            {
                //处于上方
                Cursor = Cursors.SizeNS;
                drawType = DrawType.Top;
            }
            else
            {
                drawType = DrawType.None;
                Cursor = null;
            }
        }
    }
}
