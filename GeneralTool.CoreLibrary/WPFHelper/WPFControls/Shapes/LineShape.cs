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
            this.StartPoint = startPixelPoint;
            this.EndPoint = endPixelPoint;
            this.PixelPoints.Add(this.StartPoint);
            this.PixelPoints.Add(this.EndPoint);
        }

        private Point startPoint;
        /// <summary>
        /// 起点坐标
        /// </summary>
        public Point StartPoint
        {
            get => this.startPoint;
            set => this.RegisterProperty(ref this.startPoint, value);
        }

        private Point endPoint;
        /// <summary>
        /// 终点坐标
        /// </summary>
        public Point EndPoint
        {
            get => this.endPoint;
            set => this.RegisterProperty(ref this.endPoint, value);
        }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            if (this.PixelPoints.Count > 1)
            {
                this.StartPoint = this.PixelPoints[0];
                this.EndPoint = this.PixelPoints[1];
            }
            var sp = this.ImageView.TranslateToCanvasPoint(this.StartPoint);
            var ep = this.ImageView.TranslateToCanvasPoint(this.EndPoint);
            this.Path.Data = new LineGeometry(sp, ep);
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            if (this.Path.Data is LineGeometry g)
            {
                g.StartPoint = canvasPoints[0];
                g.EndPoint = canvasPoints[1];
            }
        }
    }
}
