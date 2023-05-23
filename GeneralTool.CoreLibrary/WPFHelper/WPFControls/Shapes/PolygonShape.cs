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
        public PolygonShape(ImageViewControl viewControl) : base(viewControl)
        {
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            var startPoint = canvasPoints[0];
            var segs = new PathSegmentCollection();
            for (int i = 1; i < canvasPoints.Count; i++)
            {
                var p = canvasPoints[i];
                segs.Add(new LineSegment(p, true));
            }

            var geo = this.Path.Data as PathGeometry;
            var figures = geo.Figures;
            var figure = figures[0];
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
            var startPoint = this.ImageView.TranslateToCanvasPoint(this.PixelPoints[0]);

            var segs = new PathSegmentCollection();
            for (int i = 1; i < this.PixelPoints.Count; i++)
            {
                var p = this.PixelPoints[i];
                var canvasPoint = this.ImageView.TranslateToCanvasPoint(p);
                segs.Add(new LineSegment(canvasPoint, true));
            }
;

            this.Path.Data = new PathGeometry()
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
