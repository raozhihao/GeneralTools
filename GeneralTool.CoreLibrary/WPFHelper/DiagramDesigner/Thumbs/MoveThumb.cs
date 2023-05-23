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
            this.DragStarted += MoveThumb_DragStarted;
            this.DragDelta += MoveThumb_DragDelta;
            this.DragCompleted += MoveThumb_DragCompleted;
        }

        private Point? startPoint;
        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!(this.TemplatedParent is BlockItem block))
                return;
            if (!(block.Parent is DesignerCanvas))
            {
                return;
            }

            var x = Canvas.GetLeft(block);
            var y = Canvas.GetTop(block);
            this.startPoint = new Point(x, y);
        }

        private void MoveThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {

            if (!(this.TemplatedParent is BlockItem block))
                return;
            if (!(block.Parent is DesignerCanvas canvas))
            {
                return;
            }


            if (this.startPoint != null)
            {
                var x = Canvas.GetLeft(block);
                var y = Canvas.GetTop(block);

                var currPoint = new Point(x, y);
                if (this.startPoint.Value != currPoint)
                {
                    canvas.ClearSection();
                    block.RaiseUpdatePosition(currPoint);
                }
            }

            this.startPoint = null;
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
            if (this.TemplatedParent is BlockItem designerItem && VisualTreeHelper.GetParent(designerItem) is Canvas designer)
            {
                // we only move DesignerItems
                var designerItems = designer.Children.OfType<BlockItem>().Where(b => b.IsSelected);

                if (designerItems.Count() > 0)
                {
                    double minLeft = double.MaxValue;
                    double minTop = double.MaxValue;
                    foreach (var item in designerItems)
                    {
                        double left = Canvas.GetLeft(item);
                        double top = Canvas.GetTop(item);

                        minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                        minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
                    }

                    double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
                    double deltaVertical = Math.Max(-minTop, e.VerticalChange);

                    foreach (var item in designerItems)
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
