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

        /// <inheritdoc/>
        public PointShape(ImageViewControl viewControl) : base(viewControl)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewControl"></param>
        /// <param name="centerPoint">中心点</param>
        /// <param name="radius">半径</param>
        public PointShape(ImageViewControl viewControl, Point centerPoint, double radius) : base(viewControl)
        {
            this.centerPoint = centerPoint;
            this.Raidus = radius;
            this.PixelPoints.Add(centerPoint);
        }


        private Point centerPoint;
        /// <summary>
        /// 圆在图像上的中心点坐标
        /// </summary>
        public Point CenterImgPixelPoint
        {
            get => this.centerPoint;
            set
            {
                //更新坐标
                (this.Path.Data as EllipseGeometry).Center = value;
                this.centerPoint = value;
                this.PixelPoints[0] = centerPoint;
            }
        }


        /// <summary>
        /// 半径
        /// </summary>
        public double Raidus { get; set; }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            if (this.PixelPoints.Count > 0)
                this.centerPoint = this.PixelPoints[0];
            //转换坐标
            var centerPoint = this.ImageView.TranslateToCanvasPoint(this.centerPoint);
            //生成
            var ellipseGeometry = new EllipseGeometry(centerPoint, this.Raidus, this.Raidus);

            this.Path.Data = ellipseGeometry;
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            var p = canvasPoints[0];
            (this.Path.Data as EllipseGeometry).Center = p;
            this.centerPoint = this.ImageView.TranslateToPixelPoint(p);
        }

        /// <inheritdoc/>
        public override void UpdateScaleSize(double imageScale)
        {
            if (this.Path.Data is EllipseGeometry e)
            {
                //更新大小
                var scale = this.Raidus / imageScale;
                e.RadiusX = e.RadiusY
                    = scale;

            }
        }


    }
}
