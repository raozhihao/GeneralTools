using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 多边形
    /// </summary>
    public class PolygonShape : BaseShape
    {

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            Point startPoint = canvasPoints[0];
            PathSegmentCollection segs = new PathSegmentCollection();
            for (int i = 1; i < canvasPoints.Count; i++)
            {
                Point p = canvasPoints[i];
                segs.Add(new LineSegment(p, true));
            }

            PathGeometry geo = Path.Data as PathGeometry;
            PathFigureCollection figures = geo.Figures;
            PathFigure figure = figures[0];
            figure.StartPoint = startPoint;
            figure.Segments = segs;
        }

        /// <summary>
        /// 是否闭合
        /// </summary>
        public bool IsClosed { get; set; }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            Point startPoint = ImageView.TranslateToCanvasPoint(PixelPoints[0]);

            PathSegmentCollection segs = new PathSegmentCollection();
            for (int i = 1; i < PixelPoints.Count; i++)
            {
                Point p = PixelPoints[i];
                Point canvasPoint = ImageView.TranslateToCanvasPoint(p);
                segs.Add(new LineSegment(canvasPoint, true));
            }
;

            Path.Data = new PathGeometry()
            {
                Figures = new PathFigureCollection()
                 {
                     new PathFigure()
                     {
                         StartPoint=startPoint,
                         Segments=segs,
                         IsClosed=IsClosed,
                     }
                 }
            };

        }

    }
}
