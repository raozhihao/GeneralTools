using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace GeneralTool.CoreLibrary.WPFHelper.WPFControls.Shapes
{
    /// <summary>
    /// 矩形图形
    /// </summary>
    public class RectShape : BaseShape
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        public RectShape(Rect rect)
        {
            this.rect = rect;
        }

        private Rect rect;
        /// <summary>
        /// 矩形大小
        /// </summary>
        public Rect Rect
        {
            get => rect;
            set
            {
                UpdateShape(ParseToPoints(value).ToList());
                rect = value;
            }
        }

        private ObservableCollection<Point> ParseToPoints(Rect value)
        {
            PixelPoints.Clear();
            PixelPoints.Add(value.Location);
            PixelPoints.Add(value.TopRight);
            PixelPoints.Add(value.BottomRight);
            PixelPoints.Add(value.BottomLeft);
            return PixelPoints;
        }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            _ = ParseToPoints(rect);
            Rect canvasRect = ParseToCanvasRect(rect);
            RectangleGeometry rectGeo = new RectangleGeometry(canvasRect);
            Path.Data = rectGeo;
        }

        private Rect ParseToCanvasRect(Rect rect)
        {
            Point lt = rect.TopLeft;
            Point rb = rect.BottomRight;
            lt = ImageView.TranslateToCanvasPoint(lt);
            rb = ImageView.TranslateToCanvasPoint(rb);
            return new Rect(lt, rb);
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            if (Path.Data is RectangleGeometry g)
            {
                g.Rect = new Rect(canvasPoints[0], canvasPoints[2]);
            }
        }
    }
}
