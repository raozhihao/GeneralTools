using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace GeneralTool.General.WPFHelper.WPFControls.Shapes
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
            var data = this.shape.Path.Data.Bounds;
            data.Inflate(InflateScale / this.shape.ImageView.ImageScale, InflateScale / this.shape.ImageView.ImageScale);
            base.OnRender(drawingContext);

            pen = new Pen(Brushes.Beige, 4 / this.shape.ImageView.ImageScale)
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
            this.CaptureMouse();


            this.mouseStartDown = e.GetPosition(this);

            e.Handled = true;


            base.OnPreviewMouseLeftButtonDown(e);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Resize
                this.ResizePath(e);
            }
            else
            {
                //show resize cursor
                this.ShowReizeCuursors();
            }
            e.Handled = true;

            base.OnPreviewMouseMove(e);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {

            this.ReleaseMouseCapture();

            this.mouseStartDown = null;

            this.shape.UpdatePixelpoints();

            base.OnPreviewMouseUp(e);
        }

        private void ResizePath(MouseEventArgs e)
        {
            if (this.shape.PixelPoints.Count == 1) return;

            if (e.LeftButton != MouseButtonState.Pressed || this.mouseStartDown == null || this.Cursor == null) return;

            //鼠标当前的位置
            var currPoint = e.GetPosition(this);

            //偏移量
            var vector = currPoint - this.mouseStartDown.Value;

            //确保只能向一个方向调整
            switch (this.drawType)
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
            var canvasGeoBuilder = new StringBuilder();
            var canvasPoints = new List<Point>();
            var pixelPoints = this.shape.PixelPoints;
            var first = pixelPoints[0];
            var firstCanvas = this.shape.ImageView.TranslateToCanvasPoint(first);
            canvasPoints.Add(firstCanvas);
            canvasGeoBuilder.Append($"M{firstCanvas}");

            int i = 0;
            for (i = 1; i < pixelPoints.Count; i++)
            {
                var p = this.shape.ImageView.TranslateToCanvasPoint(pixelPoints[i]);
                canvasPoints.Add(p);
                canvasGeoBuilder.Append($"L{p}");
            }

            var geo = Geometry.Parse(canvasGeoBuilder.ToString());
            var geoBounds = geo.Bounds;
            var left = (int)geoBounds.Left;
            var right = (int)geoBounds.Right;
            var top = (int)geoBounds.Top;
            var bottom = (int)geoBounds.Bottom;

            var count = 0;
            //判断偏移方向
            for (i = 0; i < canvasPoints.Count; i++)
            {
                var item = canvasPoints[i];

                if (this.drawType == DrawType.Left && (int)item.X >= right)
                {
                    count++;
                    continue;
                }
                else if (this.drawType == DrawType.Right && (int)item.X <= left)
                {
                    count++;
                    continue;
                }
                else if (this.drawType == DrawType.Top && (int)item.Y >= bottom)
                {
                    count++;
                    continue;
                }
                else if (this.drawType == DrawType.Bottom && (int)item.Y <= top)
                {
                    count++;
                    continue;
                }

                item = new Point(item.X + vector.X, item.Y + vector.Y);
                canvasPoints[i] = item;
            }

            var resizeInfo = new ResizeEventArgs()
            {
                PixelPoints = this.ParsePixelPoints(canvasPoints)
            };
            this.ResizeChingingEventHandler?.Invoke(this, resizeInfo);
            if (resizeInfo.Handled) return;

            //进行更新
            this.shape.UpdateShapePoints(canvasPoints);
            this.InvalidateVisual();
        }

        private List<Point> ParsePixelPoints(List<Point> canvasPoints)
        {
            var list = new List<Point>();
            foreach (var item in canvasPoints)
            {
                list.Add(this.shape.ImageView.TranslateToPixelPoint(item));
            }
            return list;
        }

        private void ShowReizeCuursors()
        {
            //确定当前鼠标点的位置
            if (this.shape.PixelPoints.Count == 1) return;

            var mousePoint = Mouse.GetPosition(this);

            var bound = this.shape.Path.Data.Bounds;
            bound.Inflate(InflateScale / this.shape.ImageView.ImageScale, InflateScale / this.shape.ImageView.ImageScale);

            //判断
            var xDis = bound.Right - mousePoint.X;
            var leftXDis = bound.Left - mousePoint.X;
            var yDis = bound.Bottom - mousePoint.Y;
            var topYDis = bound.Top - mousePoint.Y;

            var scale = this.shape.ImageView.ImageScale;
            //判断鼠标是否在右边框
            var mouseInRightLine = xDis > -1 / scale && xDis < 3 / scale && mousePoint.Y > (bound.Top + 1 / scale) && mousePoint.Y < bound.Bottom - 1 / scale;

            //判断鼠标是否在左边框
            var mouseInLeftLine = leftXDis > -1 / scale && leftXDis < 3 / scale && mousePoint.Y > (bound.Top + 1 / scale) && mousePoint.Y < bound.Bottom - 1 / scale;

            //判断鼠标是否在下边框
            var mouseInBottomLine = yDis > -1 / scale && yDis < 3 / scale && mousePoint.X > bound.Left + 1 / scale && mousePoint.X < bound.Right - 1 / scale;

            //判断鼠标是否在上边框
            var mouseInTopLine = topYDis > -1 / scale && topYDis < 3 / scale && mousePoint.X > bound.Left + 1 / scale && mousePoint.X < bound.Right - 1 / scale;


            //右边的显示应该处于右顶点到右底点之间
            if (mouseInRightLine)
            {
                //处于右边
                this.Cursor = Cursors.SizeWE;
                this.drawType = DrawType.Right;
            }
            else if (mouseInLeftLine)
            {
                //处于左边
                this.Cursor = Cursors.SizeWE;
                this.drawType = DrawType.Left;
            }
            else if (mouseInBottomLine)
            {
                //处于下方
                this.Cursor = Cursors.SizeNS;
                this.drawType = DrawType.Bottom;
            }
            else if (mouseInTopLine)
            {
                //处于上方
                this.Cursor = Cursors.SizeNS;
                this.drawType = DrawType.Top;
            }
            else
            {
                this.drawType = DrawType.None;
                this.Cursor = null;
            }
        }
    }
}
