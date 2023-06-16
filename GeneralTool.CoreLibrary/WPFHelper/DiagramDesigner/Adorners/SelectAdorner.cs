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
            fillBrush = new SolidColorBrush(Color.FromArgb(100, 234, 12, 223));
            Start = startPoint;
        }

        private Point? start;
        /// <summary>
        /// 
        /// </summary>
        public Point? Start
        {
            get => start;
            set
            {
                if (start == value)
                    return;
                start = value;
                InvalidateVisual();
            }
        }

        private Point? end;
        /// <summary>
        /// 
        /// </summary>
        public Point? End
        {
            get => end;
            set
            {
                if (end == value)
                    return;
                end = value;

                InvalidateVisual();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //
            if (Start == null || End == null) return;

            Rect rect = new Rect(Start.Value, End.Value);
            drawingContext.DrawRectangle(fillBrush, drawingPen, rect);

        }
    }
}
