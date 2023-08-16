using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs
{
    /// <summary>
    /// 用来处理元素大小更改
    /// </summary>
    internal class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragStarted += ResizeThumb_DragStarted;
            DragDelta += ResizeThumb_DragDelta;
            DragCompleted += ResizeThumb_DragCompleted;
        }

        private Size? prevSize;
        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (DataContext is BlockItem item && VisualTreeHelper.GetParent(item) is Canvas && prevSize != null)
            {
                Size currentSize = item.DesiredSize;
                if (currentSize != prevSize.Value)
                {
                    double x = Canvas.GetLeft(item);
                    double y = Canvas.GetTop(item);

                    Point currPoint = new Point(x, y);

                    item.RaiseResizeChanged(currPoint, currentSize);

                }
            }
            prevSize = null;
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (DataContext is BlockItem item && VisualTreeHelper.GetParent(item) is Canvas)
            {
                prevSize = item.DesiredSize;
            }
        }

        public event EventHandler<ResizeArg> ResizeChanged;
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //获取当前附加的控件
            if (DataContext is BlockItem item && VisualTreeHelper.GetParent(item) is Canvas canvas)
            {
                if (!item.CanResize) return;

                double deltaVertical, deltaHorizontal;

                ResizeArg resizeArg = new ResizeArg();
                double oldX = Canvas.GetLeft(item);
                double oldY = Canvas.GetTop(item);
                Point oldPoint = new Point(oldX, oldY);
                resizeArg.OldCanvasPoint = oldPoint;
                //获取真实外包矩形
                var bounds = item.GetBoundRect();
                var top1 = Canvas.GetTop(item);
                var left1 = Canvas.GetLeft(item);
                var heightInc = top1 - bounds.Y;
                var widthInc = left1 - bounds.X;
                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,
                            item.ActualHeight - item.MinHeight);

                        if (e.VerticalChange > 0)
                        {
                            //往下拉时,查看是否会大于画布的高
                            var height = bounds.Bottom + e.VerticalChange;
                            if (height >= canvas.ActualHeight)
                                 return;
                            if (left1 - widthInc < 0)
                                return;

                        }

                        SetHeight(item, deltaVertical, canvas);
                        resizeArg.Direction = Direction.Bottom;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,
                            item.ActualHeight - item.MinHeight);

                        //查看top是否会达到0
                        double top = top1 + deltaVertical;
                        if (top <= 0)
                            return;

                        if (e.VerticalChange < 0)
                        {
                            if (top1 - heightInc < 0 && e.VerticalChange < 0)
                                return;
                            if (left1 - widthInc < 0)
                                return;
                        }
                       
                        Canvas.SetTop(item, top);
                        SetHeight(item, deltaVertical, canvas);
                        resizeArg.Direction = Direction.Top;
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange,
                            item.ActualWidth - item.MinWidth);
                        //查看左边是否超出
                        double left = left1 + deltaHorizontal;
                        if (left <= 0)
                            return;

                        if (e.HorizontalChange<0)
                        {
                            if (left1 - widthInc < 0 && e.HorizontalChange < 0)
                                return;
                            var height = bounds.Bottom - e.HorizontalChange;
                            if (height >= canvas.ActualHeight)
                            {
                                return;
                            }
                        }
                       
                        Canvas.SetLeft(item, left1 + deltaHorizontal);
                        SetWidth(item, deltaHorizontal, canvas);

                        resizeArg.Direction = Direction.Left;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            item.ActualWidth - item.MinWidth);

                        //往右拉
                        if (e.HorizontalChange > 0)
                        {
                            if (bounds.Right + e.HorizontalChange >= canvas.ActualWidth)
                                return;
                            var height = bounds.Bottom + e.HorizontalChange;
                            if (height >= canvas.ActualHeight)
                            {
                                return;
                            }
                        }
                        SetWidth(item, deltaHorizontal, canvas);

                        resizeArg.Direction = Direction.Right;
                        break;

                }
                resizeArg.BlockItem = item;
                resizeArg.NewCanvasPoint = new Point(Canvas.GetLeft(item), Canvas.GetTop(item));
                ResizeChanged?.Invoke(this, resizeArg);
            }

            e.Handled = true;
        }

        private void SetWidth(BlockItem item, double deltaHorizontal, Canvas canvas)
        {
            var left = Canvas.GetLeft(item);
            double tmpWidth = item.ActualWidth - deltaHorizontal;
            if (tmpWidth <= 0) return;
            //查看width是否会大于等于容器的宽
            if (tmpWidth + left >= canvas.ActualWidth)
                tmpWidth = canvas.ActualWidth - left;

            // var width = tmpWidth >= canvas.ActualWidth ? canvas.ActualWidth : tmpWidth;
            var width = tmpWidth <= item.MinWidth ? item.MinWidth : tmpWidth;

            width = Math.Round(width);
            item.Width = width;
            Trace.WriteLine($"{item.Width}     {canvas.ActualWidth}");
        }

        private void SetHeight(BlockItem item, double deltaVertical, Canvas canvas)
        {
            var top = Canvas.GetTop(item);

            double tmpHeight = item.ActualHeight - deltaVertical;
            if (tmpHeight <= 0) return;

            //查看height是否会大于等于容器的高
            if (tmpHeight + top >= canvas.ActualHeight)
                tmpHeight = canvas.ActualHeight - top;

            // var height = tmpHeight;
            var height = tmpHeight <= item.MinHeight ? item.MinHeight : tmpHeight;
            item.Height = Math.Round(height);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResizeArg : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public ResizeArg()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public ResizeArg(Point oldPoint, Point newPoint, Direction direction)
        {
            OldCanvasPoint = oldPoint;
            NewCanvasPoint = newPoint;
            Direction = direction;
        }

        /// <summary>
        /// 
        /// </summary>
        public Point OldCanvasPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Point NewCanvasPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Direction Direction { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BlockItem BlockItem { get; internal set; }
    }
}
