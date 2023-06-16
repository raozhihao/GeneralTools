using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 点形状
    /// </summary>
    public class PointShape : BaseShape
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="centerPoint">中心点</param>
        /// <param name="radius">半径</param>
        public PointShape(Point centerPoint, double radius)
        {
            this.centerPoint = centerPoint;
            Raidus = radius;
            PixelPoints.Add(centerPoint);
        }

        private Point centerPoint;
        /// <summary>
        /// 圆在图像上的中心点坐标
        /// </summary>
        public Point CenterImgPixelPoint
        {
            get => centerPoint;
            set
            {
                //更新坐标
                (Path.Data as EllipseGeometry).Center = value;
                centerPoint = value;
                PixelPoints[0] = centerPoint;
            }
        }

        /// <summary>
        /// 半径
        /// </summary>
        public double Raidus { get; set; }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            if (PixelPoints.Count > 0)
                this.centerPoint = PixelPoints[0];
            //转换坐标
            Point centerPoint = ImageView.TranslateToCanvasPoint(this.centerPoint);
            //生成
            EllipseGeometry ellipseGeometry = new EllipseGeometry(centerPoint, Raidus, Raidus);

            Path.Data = ellipseGeometry;
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            Point p = canvasPoints[0];
            (Path.Data as EllipseGeometry).Center = p;
            centerPoint = ImageView.TranslateToPixelPoint(p);
        }

        /// <inheritdoc/>
        public override void UpdateScaleSize(double imageScale)
        {
            if (Path.Data is EllipseGeometry e)
            {
                //更新大小
                double scale = Raidus / imageScale;
                e.RadiusX = e.RadiusY
                    = scale;

            }
        }

    }
}
