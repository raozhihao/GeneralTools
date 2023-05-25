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
            get => this.rect;
            set
            {
                this.UpdateShape(this.ParseToPoints(value).ToList());
                this.rect = value;
            }
        }

        private ObservableCollection<Point> ParseToPoints(Rect value)
        {
            this.PixelPoints.Clear();
            this.PixelPoints.Add(value.Location);
            this.PixelPoints.Add(value.TopRight);
            this.PixelPoints.Add(value.BottomRight);
            this.PixelPoints.Add(value.BottomLeft);
            return this.PixelPoints;
        }

        /// <inheritdoc/>
        public override void CreateShape()
        {
            this.ParseToPoints(this.rect);
            var canvasRect = this.ParseToCanvasRect(this.rect);
            var rectGeo = new RectangleGeometry(canvasRect);
            this.Path.Data = rectGeo;
        }

        private Rect ParseToCanvasRect(Rect rect)
        {
            var lt = rect.TopLeft;
            var rb = rect.BottomRight;
            lt = this.ImageView.TranslateToCanvasPoint(lt);
            rb = this.ImageView.TranslateToCanvasPoint(rb);
            return new Rect(lt, rb);
        }

        /// <inheritdoc/>
        public override void UpdateShape(List<Point> canvasPoints)
        {
            if (this.Path.Data is RectangleGeometry g)
            {
                g.Rect = new Rect(canvasPoints[0], canvasPoints[2]);
            }
        }
    }
}
