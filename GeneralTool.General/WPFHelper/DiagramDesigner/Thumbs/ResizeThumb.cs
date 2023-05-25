using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using GeneralTool.General.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.General.WPFHelper.DiagramDesigner.Models;

namespace GeneralTool.General.WPFHelper.DiagramDesigner.Thumbs
{
    /// <summary>
    /// 用来处理元素大小更改
    /// </summary>
    internal class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            this.DragStarted += ResizeThumb_DragStarted;
            this.DragDelta += ResizeThumb_DragDelta;
            this.DragCompleted += ResizeThumb_DragCompleted;
        }

        private Size? prevSize;
        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (this.DataContext is BlockItem item && VisualTreeHelper.GetParent(item) is Canvas && this.prevSize != null)
            {
                var currentSize = item.DesiredSize;
                if (currentSize != this.prevSize.Value)
                {
                    var x = Canvas.GetLeft(item);
                    var y = Canvas.GetTop(item);

                    var currPoint = new Point(x, y);

                    item.RaiseResizeChanged(currPoint, currentSize);

                }
            }
            this.prevSize = null;
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (this.DataContext is BlockItem item && VisualTreeHelper.GetParent(item) is Canvas)
            {
                this.prevSize = item.DesiredSize;
            }
        }

        public event EventHandler<ResizeArg> ResizeChanged;
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //获取当前附加的控件
            if (this.DataContext is BlockItem item && VisualTreeHelper.GetParent(item) is Canvas canvas)
            {
                if (item.Content == null)
                {
                    return;
                }
                double deltaVertical, deltaHorizontal;

                var resizeArg = new ResizeArg();
                var oldX = Canvas.GetLeft(item);
                var oldY = Canvas.GetTop(item);
                var oldPoint = new Point(oldX, oldY);
                resizeArg.OldCanvasPoint = oldPoint;

                switch (this.VerticalAlignment)
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
                        var top = Canvas.GetTop(item) + deltaVertical;
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
                        var left = Canvas.GetLeft(item) + deltaHorizontal;
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
                this.ResizeChanged?.Invoke(this, resizeArg);
            }


            e.Handled = true;
        }

        private void SetWidth(Control item, double deltaHorizontal, Canvas canvas)
        {
            var tmpWidth = item.ActualWidth - deltaHorizontal;
            //查看width是否会大于等于容器的宽
            if (tmpWidth >= canvas.ActualWidth)
                tmpWidth = canvas.ActualWidth;
            item.Width = tmpWidth >= canvas.ActualWidth ? canvas.ActualWidth : tmpWidth;
            item.Width = tmpWidth <= item.MinWidth ? item.MinWidth : tmpWidth;
        }

        private void SetHeight(Control item, double deltaVertical, Canvas canvas)
        {
            var tmpHeight = item.ActualHeight - deltaVertical;

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
            this.OldCanvasPoint = oldPoint;
            this.NewCanvasPoint = newPoint;
            this.Direction = direction;
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
