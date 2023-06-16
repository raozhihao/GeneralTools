using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 线段图形
    /// </summary>
    public class LineShape : BaseShape
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startPixelPoint"></param>
        /// <param name="endPixelPoint"></param>
        public LineShape(Point startPixelPoint, Point endPixelPoint)
        {
            StartPoint = startPixelPoint;
            EndPoint = endPixelPoint;
            PixelPoints.Add(StartPoint);
            PixelPoints.Add(EndPoint);
        }

        private Point startPoint;
        /// <summary>
        /// 起点坐标
        /// </summary>
        public Point StartPoint
        {
            get => startPoint;
            set => RegisterProperty(ref startPoint, value);
        }

        private Point endPoint;
        /// <summary>
        /// 终点坐标
        /// </summary>
        public Point EndPoint
        {
            get => endPoint;
            set => RegisterProperty(ref endPoint, value);
        }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            if (PixelPoints.Count > 1)
            {
                StartPoint = PixelPoints[0];
                EndPoint = PixelPoints[1];
            }
            Point sp = ImageView.TranslateToCanvasPoint(StartPoint);
            Point ep = ImageView.TranslateToCanvasPoint(EndPoint);
            Path.Data = new LineGeometry(sp, ep);
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            if (Path.Data is LineGeometry g)
            {
                g.StartPoint = canvasPoints[0];
                g.EndPoint = canvasPoints[1];
            }
        }
    }
}
