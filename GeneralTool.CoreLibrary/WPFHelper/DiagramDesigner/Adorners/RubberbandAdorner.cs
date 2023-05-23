using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Adorners
{
    /// <summary>
    /// 
    /// </summary>
    public class RubberbandAdorner : Adorner
    {
        private Point? startPoint;
        private Point? endPoint;
        private readonly Pen rubberbandPen;

        private readonly DesignerCanvas designerCanvas;

        /// <summary>
        /// 
        /// </summary>
        public RubberbandAdorner(DesignerCanvas designerCanvas, Point? dragStartPoint)
            : base(designerCanvas)
        {
            this.designerCanvas = designerCanvas;
            this.startPoint = dragStartPoint;
            rubberbandPen = new Pen(Brushes.LightSlateGray, 1)
            {
                DashStyle = new DashStyle(new double[] { 2 }, 1)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured)
                    this.CaptureMouse();

                endPoint = e.GetPosition(this);
                UpdateSelection();
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured) this.ReleaseMouseCapture();
            }

            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            // release mouse capture
            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            // remove this adorner from adorner layer
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.designerCanvas);
            adornerLayer?.Remove(this);
            designerCanvas.RaiseSelectionChanged();
            e.Handled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

            if (this.startPoint.HasValue && this.endPoint.HasValue)
                dc.DrawRectangle(Brushes.Transparent, rubberbandPen, new Rect(this.startPoint.Value, this.endPoint.Value));
        }

        private void UpdateSelection()
        {
            Rect rubberBand = new Rect(startPoint.Value, endPoint.Value);
            foreach (Control item in designerCanvas.Children)
            {
                if (item is BlockItem b)
                {
                    Rect itemRect = VisualTreeHelper.GetDescendantBounds(item);
                    Rect itemBounds = item.TransformToAncestor(designerCanvas).TransformBounds(itemRect);
                    if (rubberBand.Contains(itemBounds))
                    {
                        designerCanvas.Selection.AddItem(b);
                    }
                }

            }

            designerCanvas.RaiseSelectionChanged();
        }

    }
}
