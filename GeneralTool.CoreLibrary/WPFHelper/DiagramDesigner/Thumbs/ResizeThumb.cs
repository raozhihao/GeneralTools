using System;
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
                if (item.Content == null)
                {
                    return;
                }
                double deltaVertical, deltaHorizontal;

                ResizeArg resizeArg = new ResizeArg();
                double oldX = Canvas.GetLeft(item);
                double oldY = Canvas.GetTop(item);
                Point oldPoint = new Point(oldX, oldY);
                resizeArg.OldCanvasPoint = oldPoint;

                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,
                            item.ActualHeight - item.MinHeight);

                        SetHeight(item, deltaVertical, canvas);
                        resizeArg.Direction = Direction.Bottom;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,
                            item.ActualHeight - item.MinHeight);

                        //查看top是否会达到0
                        double top = Canvas.GetTop(item) + deltaVertical;
                        if (top <= 0)
                            return;
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
                        double left = Canvas.GetLeft(item) + deltaHorizontal;
                        if (left <= 0)
                            return;
                        Canvas.SetLeft(item, Canvas.GetLeft(item) + deltaHorizontal);
                        SetWidth(item, deltaHorizontal, canvas);

                        resizeArg.Direction = Direction.Left;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            item.ActualWidth - item.MinWidth);
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

        private void SetWidth(Control item, double deltaHorizontal, Canvas canvas)
        {
            double tmpWidth = item.ActualWidth - deltaHorizontal;
            //查看width是否会大于等于容器的宽
            if (tmpWidth >= canvas.ActualWidth)
                tmpWidth = canvas.ActualWidth;
            item.Width = tmpWidth >= canvas.ActualWidth ? canvas.ActualWidth : tmpWidth;
            item.Width = tmpWidth <= item.MinWidth ? item.MinWidth : tmpWidth;
        }

        private void SetHeight(Control item, double deltaVertical, Canvas canvas)
        {
            double tmpHeight = item.ActualHeight - deltaVertical;

            //查看height是否会大于等于容器的高
            if (tmpHeight >= canvas.ActualHeight)
                tmpHeight = canvas.ActualHeight;

            item.Height = tmpHeight;
            item.Height = tmpHeight <= item.MinHeight ? item.MinHeight : tmpHeight;
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
