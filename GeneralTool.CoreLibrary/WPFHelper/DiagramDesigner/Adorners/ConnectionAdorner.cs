using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Controls;
using GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Thumbs;

namespace GeneralTool.CoreLibrary.WPFHelper.DiagramDesigner.Adorners
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionAdorner : Adorner
    {
        private readonly Pen drawingPen;
        /// <summary>
        /// 
        /// </summary>
        public BlockItem SourceItem { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ConnectorThumb SourceThumb { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="adornedElement"></param>
        /// <param name="startPoint"></param>
        /// <param name="connectorThumb"></param>
        public ConnectionAdorner(UIElement adornedElement, Point startPoint, ConnectorThumb connectorThumb) : base(adornedElement)
        {
            drawingPen = new Pen(new SolidColorBrush(Colors.Gray) { Opacity = 0.7 }, 1.5)
            {
                LineJoin = PenLineJoin.Round
            };
            Cursor = Cursors.Cross;
            IsHitTestVisible = true;

            SourceItem = adornedElement as BlockItem;
            SourceThumb = connectorThumb;

            Start = startPoint;
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectorThumb DestThumb { get; set; }

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

        private readonly StreamGeometry streamGeometry = new StreamGeometry();
        /// <summary>
        /// 
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            //
            if (Start == null || End == null) return;

            UpdateArrow();
            //UpdatePolygon();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(Start.Value, true, false);

                geometryContext.PolyLineTo(Points, true, false);

            }

            drawingContext.DrawGeometry(null, drawingPen, streamGeometry);
            if (DestThumb != null)
            {
                //画个圈
                drawingContext.DrawEllipse(Brushes.Red, drawingPen, End.Value, 5, 5);
            }
        }

        private PointCollection Points;
        /// <summary>
        /// 
        /// </summary>
        public void UpdateArrow(double arrowAngle = Math.PI / 12, double arrowLength = 20)
        {
            double x1 = Start.Value.X;
            double y1 = Start.Value.Y;
            double x2 = End.Value.X;
            double y2 = End.Value.Y;
            Point point2 = new Point(x2, y2);     // 箭头终点
            double angleOri = Math.Atan((y2 - y1) / (x2 - x1));      // 起始点线段夹角
            double angleDown = angleOri - arrowAngle;   // 箭头扩张角度
            double angleUp = angleOri + arrowAngle;     // 箭头扩张角度
            int directionFlag = (x2 > x1) ? -1 : 1;     // 方向标识
            double x3 = x2 + (directionFlag * arrowLength * Math.Cos(angleDown));   // 箭头第三个点的坐标
            double y3 = y2 + (directionFlag * arrowLength * Math.Sin(angleDown));
            double x4 = x2 + (directionFlag * arrowLength * Math.Cos(angleUp));     // 箭头第四个点的坐标
            double y4 = y2 + (directionFlag * arrowLength * Math.Sin(angleUp));
            Point point3 = new Point(x3, y3);   // 箭头第三个点
            Point point4 = new Point(x4, y4);   // 箭头第四个点
            Point[] points = new Point[] { point2, point3, point4, point2 };   // 多边形，起点 --> 终点 --> 第三点 --> 第四点 --> 终点

            Points = new PointCollection(points);
        }
    }
}
