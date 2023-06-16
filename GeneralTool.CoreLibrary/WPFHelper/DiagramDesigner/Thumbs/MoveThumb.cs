using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs
{
    /// <summary>
    /// 用来处理元素拖动
    /// </summary>
    public class MoveThumb : Thumb
    {
        /// <summary>
        /// 
        /// </summary>
        public MoveThumb()
        {
            DragStarted += MoveThumb_DragStarted;
            DragDelta += MoveThumb_DragDelta;
            DragCompleted += MoveThumb_DragCompleted;
        }

        private Point? startPoint;
        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {

            /* 项目“GeneralTool.CoreLibrary (net452)”的未合并的更改
            在此之前:
                        if (!(this.TemplatedParent is BlockItem block))
            在此之后:
                        if (!(TemplatedParent is BlockItem block))
            */
            if (!(TemplatedParent is  BlockItem block))
                return;
            if (!(block.Parent is  DesignerCanvas))
            {
                return;
            }

            double x = Canvas.GetLeft(block);
            double y = Canvas.GetTop(block);
            startPoint = new Point(x, y);
        }

        private void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {

            /* 项目“GeneralTool.CoreLibrary (net452)”的未合并的更改
            在此之前:
                        if (!(this.TemplatedParent is BlockItem block))
            在此之后:
                        if (!(TemplatedParent is BlockItem block))
            */
             if (!(this.TemplatedParent is BlockItem block))
                return;
            if (!(block.Parent is  DesignerCanvas canvas))
            {
                return;
            }

            if (startPoint != null)
            {
                double x = Canvas.GetLeft(block);
                double y = Canvas.GetTop(block);

                Point currPoint = new Point(x, y);
                if (startPoint.Value != currPoint)
                {
                    canvas.ClearSection();
                    block.RaiseUpdatePosition(currPoint);
                }
            }

            startPoint = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public const double MinTop = 20;
        /// <summary>
        /// 
        /// </summary>
        public const double MinLeft = 20;
        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (TemplatedParent is BlockItem designerItem && VisualTreeHelper.GetParent(designerItem) is Canvas designer)
            {
                // we only move DesignerItems
                System.Collections.Generic.IEnumerable<BlockItem> designerItems = designer.Children.OfType<BlockItem>().Where(b => b.IsSelected);

                if (designerItems.Count() > 0)
                {
                    double minLeft = double.MaxValue;
                    double minTop = double.MaxValue;
                    foreach (BlockItem item in designerItems)
                    {
                        double left = Canvas.GetLeft(item);
                        double top = Canvas.GetTop(item);

                        minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                        minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                    }

                    double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                    double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                    foreach (BlockItem item in designerItems)
                    {
                        double left = Canvas.GetLeft(item);
                        double top = Canvas.GetTop(item);

                        if (double.IsNaN(left)) left = 0;
                        if (double.IsNaN(top)) top = 0;

                        Canvas.SetLeft(item, Math.Round(left + deltaHorizontal, 0));
                        Canvas.SetTop(item, Math.Round(top + deltaVertical, 0));

                    }

                }
                else
                {
                    Move(designerItem, e);
                }

                designer.InvalidateMeasure();

                e.Handled = true;
            }

        }
        private void Move(BlockItem block, DragDeltaEventArgs e)
        {
            double minLeft = double.MaxValue;
            double minTop = double.MaxValue;

            double left = Canvas.GetLeft(block);
            double top = Canvas.GetTop(block);

            minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
            minTop = double.IsNaN(top) ? 15 : Math.Min(top, minTop);
            double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
            double deltaVertical = Math.Max(-minTop, e.VerticalChange);

            if (double.IsNaN(left)) left = 0;
            if (double.IsNaN(top)) top = 15;

            top += deltaVertical;
            top = top < MinTop ? MinTop : top;

            left += deltaHorizontal;
            left = left < MinLeft ? MinLeft : left;

            Canvas.SetLeft(block, left);
            Canvas.SetTop(block, top);

        }
    }
}
