using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Adorners
{
    /// <summary>
    /// 
    /// </summary>
    public class SelectAdorner : Adorner
    {
        private readonly Pen drawingPen;
        private readonly SolidColorBrush fillBrush;
        /// <summary>
        /// 
        /// </summary>
        public SelectAdorner(UIElement adornedElement, Point startPoint) : base(adornedElement)
        {
            drawingPen = new Pen(Brushes.LightSlateGray, 1.5);
            this.fillBrush = new SolidColorBrush(Color.FromArgb(100, 234, 12, 223));
            this.Start = startPoint;
        }

        private Point? start;
        /// <summary>
        /// 
        /// </summary>
        public Point? Start
        {
            get => this.start;
            set
            {
                if (this.start == value)
                    return;
                this.start = value;
                this.InvalidateVisual();
            }
        }

        private Point? end;
        /// <summary>
        /// 
        /// </summary>
        public Point? End
        {
            get => this.end;
            set
            {
                if (end == value)
                    return;
                this.end = value;

                this.InvalidateVisual();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //
            if (this.Start == null || this.End == null) return;

            var rect = new Rect(this.Start.Value, this.End.Value);
            drawingContext.DrawRectangle(this.fillBrush, this.drawingPen, rect);

        }
    }
}
